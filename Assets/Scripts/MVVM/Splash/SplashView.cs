using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.MVVM.Splash
{
    using System;
    using UnityEngine.UI;

    using DG.Tweening;

    using TJ.Decaf.Base;

    
    public class SplashView : ViewBase
    {
        [Header("[Editor setting] - UI components")]
        [SerializeField] private Image logo;

        public override void Hide(Action onComplete)
        {
            OnHide = onComplete;

            Visible = VisibleState.Disappearing;

            logo.DOFade(0, 0.5f)
                .Play()
                .OnComplete(() => 
                {
                    Visible = VisibleState.Disappeared;

                    OnHide?.Invoke();
                });
        }

        public override void HideImmediate()
        {
            this.gameObject.SetActive(false);
            this.logo.gameObject.SetActive(false);
        }

        public override void Show(Action onComplete)
        {
            OnShow = onComplete;

            Visible = VisibleState.Appearing;

            logo.DOFade(1, 0.5f)
                .Play()
                .OnComplete(() => 
                {
                    Visible = VisibleState.Appeared;

                    OnShow?.Invoke();
                });
        }

        public override void ShowImmediate()
        {
            this.gameObject.SetActive(true);
            this.logo.gameObject.SetActive(true);
        }
    }

}
