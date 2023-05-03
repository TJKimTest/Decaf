using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.Base
{
    using TJ.Decaf.Manager;
    using TJ.Decaf.Data;

    using UniRx;
    using Zenject;

    public abstract class ViewModelBase : MonoBehaviour
    {
        #region [ Cores ]
        protected SceneManager sceneManager;
        protected UINavigationManager navigationManager;
        protected DeviceConfigManager deviceConfigManager;
        #endregion

        protected CompositeDisposable Disposables { get; private set; } 
            = new CompositeDisposable();

        [Inject]
        private void InjectionCores(SceneManager sceneManager,
                                    UINavigationManager navigationManager,
                                    DeviceConfigManager deviceConfigManager)
        {
            this.sceneManager = sceneManager;
            this.navigationManager = navigationManager;
            this.deviceConfigManager = deviceConfigManager;
        }

        public abstract void OnInitialized(ViewBase view);
        public abstract void ReceiveData(params BridgeData[] datas);

        protected void SendData(string receivePageKey, params BridgeData[] datas)
        {
            var receiveTargetPage = navigationManager[receivePageKey];
            if (receiveTargetPage)
                receiveTargetPage.ViewModel.ReceiveData(datas);
            else
            {
                Debug.Log($"[ Send data ]Target page is null");
            }
        }

        protected void SendData(BridgeData data)
        {
            var receiveTargetPage = navigationManager[data.To];
            if (receiveTargetPage)
                receiveTargetPage.ViewModel.ReceiveData(data);
            else
            {
                Debug.Log($"[ Send data ]Target page is null");
            }
        }

        public void OnDestroy()
        {
            Dispose();
        }


        public virtual void Dispose()
        {
            if (Disposables != null && Disposables.IsDisposed == false)
                Disposables.Clear();
        }
    }

}
