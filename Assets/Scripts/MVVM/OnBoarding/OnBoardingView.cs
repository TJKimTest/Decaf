using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.MVVM.OnBoarding
{
    using System;
    using System.Linq;
    using UnityEngine.UI;
    using TMPro;

    using DG.Tweening;

    using TJ.Decaf.Base;
    using TJ.Decaf.UI;
    
    public class OnBoardingView : ViewBase
    {
        [Serializable]
        public class OnBoardingPage
        {
            [SerializeField] private PageType pageType;
            public PageType PageType 
            {
                get
                {
                    return pageType;
                }
            }
            [SerializeField] private RectTransform root;
            public RectTransform Root
            {
                get
                {
                    return root;
                }
            }
            [SerializeField] private ToggleGroup group;
            public ToggleGroup Group
            {
                get
                {
                    return group;
                }
            }
            [SerializeField] private List<OnBoardingToggle> toggles;
            public List<OnBoardingToggle> Toggles
            {
                get
                {
                    return toggles;
                }
            }

            public static implicit operator bool(OnBoardingPage o) => o != null;
        }


        public enum PageType
        {
            First = 0,
            Second = 1,
            Third = 2,
            Last = 3,

            Max = 4,
        }


        [Header("[Editor setting] - Transforms")]
        [SerializeField] private List<OnBoardingPage> onBoardingPages;
        [SerializeField] private RectTransform circleFullGaugeCheck;
        [SerializeField] private RectTransform circleGaugeRoot;
        [SerializeField] private RectTransform finishedParticle;

        [Header("[Editor setting] - UI components")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button confirmButton;

        [Space(20)]
        [SerializeField] private Image circleGauge;

        [Space(20)]
        [SerializeField] private TMP_Text lastInfo;
        

        [Header("[Others] - Debugging properties")]
        [SerializeField] private PageType curOpenedPageType = PageType.First;

        public RectTransform this[PageType type]
        {
            get
            {
                var targetPage = onBoardingPages.Where(o => o.PageType == type).FirstOrDefault();
                if (targetPage)
                    return targetPage.Root;
                else
                    return default;
            }
        }
        public List<OnBoardingPage> GetPages() => onBoardingPages;
        public PageType GetCurrentPageType() => curOpenedPageType;
        public Button GetNextButton() => nextButton;
        public Button GetBackButton() => backButton;
        public Button GetRetryButton() => retryButton;
        public Button GetConfirmButton() => confirmButton;

        public override void Hide(Action onComplete)
        {
        }

        public override void HideImmediate()
        {
        }

        public override void Show(Action onComplete)
        {
            OnShow = onComplete;

            Visible = VisibleState.Appearing;

            ChangeCircleGauge(PageType.First);

            circleGaugeRoot.gameObject.SetActive(true);

            circleFullGaugeCheck.DOScale(0, 0).Play();
            finishedParticle.DOScale(0, 0).Play();

            foreach (var page in onBoardingPages)
            {
                page.Root.DOAnchorPosX(-Screen.width, 0f)
                    .Play();

                page.Root.gameObject.SetActive(false);
            }

            onComplete?.Invoke();
            Visible = VisibleState.Appeared;
        }

        public override void ShowImmediate()
        {
        }

        #region [ Public methods ]
        public int RefreshCafSize()
        {
            var curSecondPage = onBoardingPages
                .FirstOrDefault(o => o.PageType == PageType.Second);

            if (curSecondPage)
            {
                int cafValue = 0;
                var curSelectedToggle = curSecondPage.Toggles.FirstOrDefault(o => o.GetToggleComponent().isOn);
                if (curSelectedToggle)
                {
                    if (curSelectedToggle.GetValue() == "Youth")
                        cafValue = 150;
                    else
                        cafValue = 200;

                    lastInfo.text = $"하루 카페인 섭취량을\n<color=#557705>{cafValue}mg</color>로 추천드립니다.";
                }

                return cafValue;
            }
            else
                return -1;
        }

        public void ShowFinishedParticle()
        {
            finishedParticle.DOScale(1, 1f)
                .SetEase(Ease.InOutElastic)
                .Play();
        }

        public void HideFinishedParticle()
        {
            finishedParticle.DOScale(0, 0f)
                .Play();
        }
        public void ChangeCircleGauge(PageType curPageType)
        {
            var curFillAmountValue = curPageType switch
            {
                PageType.First => 0.25f,
                PageType.Second => 0.5f,
                PageType.Third => 0.75f,
                PageType.Last => 1f,
                _ => 0f,
            };

            circleGauge.DOFillAmount(curFillAmountValue, 0.25f)
                .Play()
                .OnComplete(() => 
                {
                    if (curPageType == PageType.Last)
                    {
                        circleGaugeRoot.gameObject.SetActive(false);
                        circleFullGaugeCheck.DOScale(1, 0.25f)
                        .Play()
                        .OnComplete(() => circleGaugeRoot.gameObject.SetActive(false));
                    }
                    else
                    {
                        circleFullGaugeCheck.DOScale(0, 0.25f)
                        .Play()
                        .OnComplete(() => circleGaugeRoot.gameObject.SetActive(true));
                    }
                });
        }
        public void ChangeViewObjectStates(PageType curPageType, bool isClearToggleValues = true)
        {
            ChangeCircleGauge(curPageType);

            backButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(false);

            switch (curPageType)
            {
                case PageType.First:
                    {
                        nextButton.gameObject.SetActive(true);
                    }
                    break;
                case PageType.Second:
                case PageType.Third:
                    {
                        backButton.gameObject.SetActive(true);
                        nextButton.gameObject.SetActive(true);
                    }
                    break;
                case PageType.Last:
                    {
                        backButton.gameObject.SetActive(true);
                        confirmButton.gameObject.SetActive(true);
                        retryButton.gameObject.SetActive(true);
                    }
                    break;
            }

            if (isClearToggleValues)
            {
                var curPageSet = onBoardingPages.FirstOrDefault(o => o.PageType == curPageType);
                if (curPageSet)
                {
                    curPageSet.Group?.SetAllTogglesOff();
                    //curPageSet.Toggles.ForEach(element => 
                    //{
                    //    element.GetToggleComponent().isOn = false;
                    //});
                }
            }
        }
        public void GoTo(PageType type, Action onComplete)
        {
            if (type == curOpenedPageType || type > PageType.Last)
                return;

            Visible = VisibleState.Appearing;

            var curOpenedPageRoot = this[curOpenedPageType];
            var targetPageRoot = this[type];

            curOpenedPageRoot.DOAnchorPosX(-Screen.width, 0.25f)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => curOpenedPageRoot.gameObject.SetActive(false));

            targetPageRoot.DOAnchorPosX(Screen.width, 0f)
                .Play()
                .OnComplete(() => 
                {
                    targetPageRoot.gameObject.SetActive(true);
                    targetPageRoot.DOAnchorPosX(0, 0.25f)
                    .Play();

                    curOpenedPageType = type;

                    if (curOpenedPageType == PageType.Last)
                        ShowFinishedParticle();
                    else
                        HideFinishedParticle();

                    Visible = VisibleState.Appeared;

                    onComplete?.Invoke();
                });
        }

        public void BackTo(Action onComplete)
        {
            if (curOpenedPageType == PageType.First)
                return;

            Visible = VisibleState.Appearing;

            HideFinishedParticle();

            var curOpenedPageRoot = this[curOpenedPageType];
            var targetPageRoot = this[curOpenedPageType - 1];

            curOpenedPageRoot.DOAnchorPosX(Screen.width, 0.25f)
                            .SetEase(Ease.Linear)
                            .Play()
                            .OnComplete(() => curOpenedPageRoot.gameObject.SetActive(false));

            targetPageRoot.DOAnchorPosX(-Screen.width, 0f)
                            .Play()
                            .OnComplete(() =>
                            {
                                targetPageRoot.gameObject.SetActive(true);
                                targetPageRoot.DOAnchorPosX(0, 0.25f)
                                .Play();

                                curOpenedPageType = curOpenedPageType - 1;

                                Visible = VisibleState.Appeared;

                                onComplete?.Invoke();
                            });
        }

        public void GoToImmediate(PageType type)
        {
            var curOpenedPageRoot = this[curOpenedPageType];
            var targetPageRoot = this[type];

            curOpenedPageRoot.DOAnchorPosX(-Screen.width, 0f)
                .Play()
                .OnComplete(() => curOpenedPageRoot.gameObject.SetActive(false));


            targetPageRoot.gameObject.SetActive(true);
            targetPageRoot.DOAnchorPosX(0, 0f)
                .Play()
                .OnComplete(() => targetPageRoot.gameObject.SetActive(true));
        }
        #endregion

    }

}

