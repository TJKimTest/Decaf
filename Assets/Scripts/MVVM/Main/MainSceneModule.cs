namespace TJ.Decaf.MVVM.Splash
{
    using TJ.Decaf.Base;
    using TJ.Decaf.Infra;
    using TJ.Decaf.Interface;
    using TJ.Decaf.Manager;
    using UnityEngine;

    public class MainSceneModule : MonoBehaviour, IModule
    {
        private ViewBase view;
        public ViewBase View => view;

        private ViewModelBase viewModel;
        public ViewModelBase ViewModel => viewModel;

        public Transform RegisterModule(UINavigationManager navigationManager)
        {
            if (TryGetComponent<ViewBase>(out var view) && TryGetComponent<ViewModelBase>(out var viewModelBase))
            {
                this.view = view;
                this.viewModel = viewModelBase;

                navigationManager.RegisterForNavigation(new UINavigationManager.Page(PageConstants.MainPage, view, viewModelBase, new UINavigationManager.Page.PageOption()
                {
                    IsForceOpenAlways = true,
                }))
                .OnAppStart(PageConstants.MainPage);
            }

            return transform;
        }
    }

}
