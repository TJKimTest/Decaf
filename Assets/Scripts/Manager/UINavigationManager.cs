using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.Manager
{
    using TJ.Decaf.Base;
    using System;
    using System.Linq;

    public class UINavigationManager : MonoBehaviour
    {
        [Serializable]
        public class Page
        {
            public class PageOption
            {
                public bool IsForceOpenAlways { get; set; }
                public Action NavigateTo { get; set; }
                public Action OnBack { get; set; }
            }

            public string Key { get; }
            public ViewBase View { get; }
            public ViewModelBase ViewModel { get; }
            public PageOption Option { get; }

            public Page(string key, ViewBase view, ViewModelBase viewModel, PageOption option)
            {
                Key = key;
                View = view;
                ViewModel = viewModel;
                Option = option;
            }

            public void Destroy()
            {
                if (View)
                    DestroyImmediate(View.gameObject);
                if (ViewModel)
                    DestroyImmediate(ViewModel.gameObject);
            }


            public static implicit operator bool(Page a) => a != null;
        }

        [Serializable]
        public class MemorisePage
        {
            [field: SerializeField] public string Key { get; }
            [field: SerializeField] public bool IsOverrideView { get; }

            public MemorisePage(string key, bool isOverrideVew)
            {
                Key = key;
                IsOverrideView = isOverrideVew;
            }

            public static implicit operator bool(MemorisePage p) => p != null;

            public override string ToString()
                => $"Key : {Key} / IsOverrideView : {IsOverrideView}";

        }

        private List<Page> container
             = new List<Page>();

        private Stack<MemorisePage> memoriseOpendPageContainer = new Stack<MemorisePage>();

        [Header("[ Auto setting ] - Debugging properties")]
        [SerializeField] private MemorisePage curOpenPage;

        public Page this[string pageKey]
        {
            get => container.FirstOrDefault(o => o.Key == pageKey);
        }


        #region [ Public methods ]
        public MemorisePage GetCurrentOpenPageInfo()
            => curOpenPage;

        public UINavigationManager RegisterForNavigation(Page page)
        {
            page.ViewModel.Dispose();
            page.View.Hide(page.Option?.OnBack);

            bool isContainerInSamePage = container.Count(o => o.Key == page.Key) > 0;

            if (isContainerInSamePage == false)
                container.Add(page);

            return this;
        }


        public void ChangePage(string openPageKey, params string[] disablePageKeys)
        {
            if (disablePageKeys != null)
            {
                foreach (var disablePageKey in disablePageKeys)
                {
                    HidePage(disablePageKey);
                }
            }

            var openPage = container.FirstOrDefault(o => o.Key == openPageKey);
            if (openPage != default)
            {
                //openPage.View.Show(null);
                openPage.ViewModel.OnInitialized(openPage.View);
            }
        }

        public void HidePage(string disablePageKey)
        {
            var disablePage = container.FirstOrDefault(o => o.Key == disablePageKey);
            if (disablePage != default)
            {
                disablePage.ViewModel.Dispose();
                disablePage.View.Hide(null);
            }
        }

        public void HideAll()
        {
            container.ForEach(element =>
            {
                element.ViewModel.Dispose();
                element.View.Hide(null);
            });
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.ForEach(element => 
                {
                    if(element.ViewModel)
                        element.ViewModel.Dispose();
                    if(element.View)
                        element.View.HideImmediate();
                    
                    element.Destroy();
                });

                container.Clear();
            }
        }

        public void OnAppStart(string key)
        {
            var target = container.Where(o => o.Key == key).FirstOrDefault();
            if (target)
            {
                curOpenPage = new MemorisePage(key, false);

                //target.View.Show(target.Option.NavigateTo);
                target.ViewModel.Dispose();
                target.ViewModel.OnInitialized(target.View);
            }
        }
        #endregion
    }

}

