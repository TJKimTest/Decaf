
using UnityEngine;
using Zenject;

namespace TJ.Decaf.Installer
{
    using System.Collections.Generic;
    using TJ.Decaf.Base;
    using TJ.Decaf.Infra;
    using TJ.Decaf.Interface;
    using TJ.Decaf.Manager;
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SetupNavivationManager();
        }

        private void SetupNavivationManager()
        {
            var navigationManager = Container.Resolve<UINavigationManager>();
            if (navigationManager)
            {
                navigationManager.Dispose();

                var curTargetModuleNames = ModuleConstants.MainScene.ModuleResourceNames;
                InstancingModules(null, curTargetModuleNames, navigationManager);
                //curTargetModuleNames.ForEach(element =>
                //{
                //    var loadedModuleObject = Container.InstantiatePrefabResource(element.RootModuleResourceName);

                //    if (loadedModuleObject.TryGetComponent<Canvas>(out var canvas))
                //        canvas.sortingOrder = element.Order;

                //    if (loadedModuleObject.TryGetComponent<IModule>(out var module))
                //        module.RegisterModule(navigationManager);
                //    else
                //    {
                //        if (loadedModuleObject.TryGetComponent<ViewBase>(out var view) && loadedModuleObject.TryGetComponent<ViewModelBase>(out var viewModelBase))
                //            viewModelBase.OnInitialized(view);
                        
                //    }
                //});
            }
        }

        private void InstancingModules(RectTransform root, List<ModuleConstants.ModuleData> sources, UINavigationManager navigationManager)
        {
            if (sources != null)
            {
                sources.ForEach(element =>
                {
                    var loadedModuleObject = Container.InstantiatePrefabResource(element.RootModuleResourceName);

                    if (root)
                    {
                        loadedModuleObject.transform.parent = root.transform;
                        loadedModuleObject.transform.SetAsLastSibling();
                    }

                    if (loadedModuleObject.TryGetComponent<Canvas>(out var canvas))
                        canvas.sortingOrder = element.Order;

                    if (loadedModuleObject.TryGetComponent<IModule>(out var module))
                        module.RegisterModule(navigationManager);
                    else
                    {
                        if (loadedModuleObject.TryGetComponent<ViewBase>(out var view) && loadedModuleObject.TryGetComponent<ViewModelBase>(out var viewModelBase))
                            viewModelBase.OnInitialized(view);

                    }

                    InstancingModules(loadedModuleObject.transform as RectTransform, element.SubModuleResourceNames, navigationManager);
                });
            }
        }
    }
}

