using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace TJ.Decaf.MVVM.OnBoarding
{
    using UniRx;

    using System.Linq;

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
                RegisterConfirmButtonClicked();

                RegisterCheckingNextButtonActiveStatus();
            });
            
        }

        public override void ReceiveData(params BridgeData[] datas)
        {
            throw new System.NotImplementedException();
        }

        #region [ Register events ]
        public void RegisterCheckingNextButtonActiveStatus()
            => Observable.EveryUpdate()
            .Subscribe(_ => 
            {
                if (view.GetCurrentPageType() != OnBoardingView.PageType.Last)
                {
                    var curOpenPage = view.GetPages().FirstOrDefault(o => o.PageType == view.GetCurrentPageType());
                    if (curOpenPage)
                    {
                        var selectedToggleCount = curOpenPage.Group.ActiveToggles().Count();
                        if (selectedToggleCount >= 1)
                            view.GetNextButton().gameObject.SetActive(true);
                        else
                            view.GetNextButton().gameObject.SetActive(false);
                    }
                }
            })
            .AddTo(Disposables);

        private void RegisterNextButtonClicked()
            => view.GetNextButton().OnClickAsObservable()
            .ThrottleFirst(GeneralConstants.ValueChangeThrottleTimeSeconds)
            .SkipWhile(_ => view.Visible != ViewBase.VisibleState.Appeared)
            .Subscribe(_ => 
            {
                var curPage = view.GetCurrentPageType();
                if (curPage + 1 == OnBoardingView.PageType.Last)
                {
                    var curCaffeine = view.RefreshCafSize();
                    globalDataContainer.SetRecoommendedAmmoutCaffeine(curCaffeine);
                }

                view.GoTo(curPage + 1, onComplete: () => 
                {
                    view.ChangeViewObjectStates(view.GetCurrentPageType());
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
                    view.ChangeViewObjectStates(view.GetCurrentPageType(), isClearToggleValues: false);
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
                    view.ChangeViewObjectStates(view.GetCurrentPageType());
                });
            })
            .AddTo(Disposables);

        private void RegisterConfirmButtonClicked()
            => view.GetConfirmButton().OnClickAsObservable()
            .ThrottleFirst(GeneralConstants.ValueChangeThrottleTimeSeconds)
            .SkipWhile(_ => view.Visible != ViewBase.VisibleState.Appeared)
            .Subscribe(_ => 
            {
                sceneTransitionFactory
                .GetBrickTransitionPage()
                .Show(() => 
                {
                    sceneTransitionFactory.GetBrickTransitionPage()
                    .Show(() => 
                    {
                        sceneManager.ChangeScene(Manager.SceneType.Main, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    });
                });
            })
            .AddTo(Disposables);
        #endregion
    }

}
