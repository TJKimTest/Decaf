
using UnityEngine;
using Zenject;


namespace TJ.Decaf.Installer
{
    using TJ.Decaf.Manager;

    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindingNavigationManager();
            BindingSceneManager();
        }

        private void BindingNavigationManager()
        {
            Container.Bind<UINavigationManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }

        private void BindingSceneManager()
        {
            Container.Bind<SceneManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }
    }
}

