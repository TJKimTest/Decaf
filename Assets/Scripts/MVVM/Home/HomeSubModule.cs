namespace TJ.Decaf.MVVM.Home
{
    using TJ.Decaf.Base;
    using TJ.Decaf.Infra;
    using TJ.Decaf.Interface;
    using TJ.Decaf.Manager;
    using UnityEngine;

    public class HomeSubModule : MonoBehaviour, IModule
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

                navigationManager.RegisterForNavigation(new UINavigationManager.Page(PageConstants.HomeSubPage, view, viewModelBase, new UINavigationManager.Page.PageOption()
                {
                    IsForceOpenAlways = true,
                }));

                this.ViewModel.OnInitialized(view);
            }

            return transform;
        }
    }

}

