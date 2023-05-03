
namespace TJ.Decaf.Interface
{

    using TJ.Decaf.Base;
    using TJ.Decaf.Manager;
    using UnityEngine;

    public interface IModule
    {
        ViewBase View { get; }
        ViewModelBase ViewModel { get; }

        Transform RegisterModule(UINavigationManager navigationManager);
    }

}
