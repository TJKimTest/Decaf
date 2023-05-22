using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.MVVM.Home
{
    using TJ.Decaf.Base;
    using TJ.Decaf.Data;
    using TJ.Decaf.Infra;
    using UniRx;

    public class HomeViewModel : ViewModelBase
    {
        [SerializeField] private HomeView view;
        public override void OnInitialized(ViewBase view)
        {
            this.view = view as HomeView;
            this.view.Show(() => 
            {
                RegisterOpenNearbyCafesButtonClicked();
            });
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
        }

        #region [ Register events ]
        private void RegisterOpenNearbyCafesButtonClicked()
            => view.GetOpenNearbyCafesRootButton().OnClickAsObservable()
            .ThrottleFirst(GeneralConstants.ValueChangeThrottleTimeSeconds)
            .SkipWhile(_ => view.Visible != ViewBase.VisibleState.Appeared)
            .Subscribe(_ => 
            {
                SendData(new BridgeData(
                        fromPageKey: PageConstants.HomeSubPage,
                        toPageKey: PageConstants.MainPage,
                        new BridgeData.Data(
                                name: "OpenNearbyCafes",
                                dataType: typeof(bool),
                                null
                            )
                    ));
            })
            .AddTo(Disposables);
        #endregion
    }

}
