using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.MVVM.BottomTab
{
    using UniRx;

    using System.Linq;

    using TJ.Decaf.Base;
    using TJ.Decaf.Data;

    public class BottomTabViewModel : ViewModelBase
    {
        [SerializeField] private BottomTabView view;
        public override void OnInitialized(ViewBase view)
        {
            this.view = view as BottomTabView;
            this.view.Show(() => 
            {
                RegisterTabBarToggleValueChanged();
            });
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
        }

        #region [ Register events ]
        private void RegisterTabBarToggleValueChanged()
            => view.GetTabBarToggleGroup()
            .ObserveEveryValueChanged(selectedToggle => selectedToggle.ActiveToggles().FirstOrDefault())
            .Subscribe(selectedToggle => 
            {
                if (selectedToggle)
                {
                    view.ChangeCirclePositionForActiveToggle();
                }
            })
            .AddTo(Disposables);
        #endregion
    }

}
