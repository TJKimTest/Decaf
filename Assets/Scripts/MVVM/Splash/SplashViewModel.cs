using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.MVVM.Splash
{
    using TJ.Decaf.Base;
    using TJ.Decaf.Data;

    public class SplashViewModel : ViewModelBase
    {
        public override void OnInitialized(ViewBase view)
        {
            view.Show(() => 
            {
                IEnumerator Delay()
                {
                    yield return new WaitForSecondsRealtime(2f);

                    view.Hide(() => 
                    {
                        sceneManager.ChangeScene(Manager.SceneType.OnBoarding, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    });
                }

                StartCoroutine(Delay());
            });
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
        }
    }

}

