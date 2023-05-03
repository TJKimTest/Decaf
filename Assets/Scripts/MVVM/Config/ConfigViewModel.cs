using System.Collections;
using System.Collections.Generic;
using TJ.Decaf.Base;
using TJ.Decaf.Data;
using UnityEngine;

namespace TJ.Decaf.MVVM.Config
{
    public class ConfigViewModel : ViewModelBase
    {
        public void Start()
        {
            deviceConfigManager.SetTargetFrame(60);
            sceneManager.ChangeScene(Manager.SceneType.Splash, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        public override void OnInitialized(ViewBase view)
        {
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
        }
    }

}
