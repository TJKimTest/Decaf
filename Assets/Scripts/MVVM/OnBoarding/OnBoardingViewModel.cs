using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace TJ.Decaf.MVVM.OnBoarding
{
    using UniRx;

    using TJ.Decaf.Base;
    using TJ.Decaf.Data;
    using TJ.Decaf.Infra;

    public class OnBoardingViewModel : ViewModelBase
    {
        [SerializeField] private OnBoardingView view;
        public override void OnInitialized(ViewBase view)
        {
            this.view = view as OnBoardingView;
            this.view.Show(() => 
            {
                this.view.GoToImmediate(OnBoardingView.PageType.First);

                RegisterNextButtonClicked();
                RegisterBackButtonClicked();
                RegisterRetryButtonClicked();
            });
            
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
            throw new System.NotImplementedException();
        }

        #region [ Register events ]
        private void RegisterNextButtonClicked()
            => view.GetNextButton().OnClickAsObservable()
            .ThrottleFirst(GeneralConstants.ValueChangeThrottleTimeSeconds)
            .SkipWhile(_ => view.Visible != ViewBase.VisibleState.Appeared)
            .Subscribe(_ => 
            {
                var curPage = view.GetCurrentPage();
                view.GoTo(curPage + 1, onComplete: () => 
                {
                    view.ChangeViewObjectStates(view.GetCurrentPage());
                });
            })
            .AddTo(Disposables);

        private void RegisterBackButtonClicked()
            => view.GetBackButton().OnClickAsObservable()
            .ThrottleFirst(GeneralConstants.ValueChangeThrottleTimeSeconds)
            .SkipWhile(_ => view.Visible != ViewBase.VisibleState.Appeared)
            .Subscribe(_ => 
            {
                view.BackTo(onComplete: () => 
                {
                    view.ChangeViewObjectStates(view.GetCurrentPage(), isClearToggleValues: false);
                });
            })
            .AddTo(Disposables);

        private void RegisterRetryButtonClicked()
            => view.GetRetryButton().OnClickAsObservable()
            .ThrottleFirst(GeneralConstants.ValueChangeThrottleTimeSeconds)
            .SkipWhile(_ => view.Visible != ViewBase.VisibleState.Appeared)
            .Subscribe(_ => 
            {
                view.GoTo(OnBoardingView.PageType.First, onComplete: () =>
                {
                    view.ChangeViewObjectStates(view.GetCurrentPage());
                });
            })
            .AddTo(Disposables);
        #endregion
    }

}
