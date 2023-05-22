using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.MVVM.Main
{
    using TJ.Decaf.Base;
    using TJ.Decaf.Data;

    public class MainViewModel : ViewModelBase
    {
        [SerializeField] private MainView view;
        public override void OnInitialized(ViewBase view)
        {
            if (sceneTransitionFactory.GetBrickTransitionPage().GetVisible() == UI.BrickTransitionPage.VisibleState.Appeared)
                sceneTransitionFactory.GetBrickTransitionPage().Hide(() => { });

            this.view = view as MainView;
            this.view.Show(() => 
            {
            });
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
            if (BridgeData.TryExists("OpenNearbyCafes", out var target, datas))
            {
                Debug.Log($"Receive data : OpenNearbyCafes");   
            }
        }
    }

}
