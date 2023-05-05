using System.Collections;
using System.Collections.Generic;
using TJ.Decaf.Base;
using TJ.Decaf.Data;
using UnityEngine;

namespace TJ.Decaf.MVVM.Home
{
    public class HomeViewModel : ViewModelBase
    {
        [SerializeField] private HomeView view;
        public override void OnInitialized(ViewBase view)
        {
            this.view = view as HomeView;
            this.view.Show(() => 
            {
            });
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
        }
    }

}
