using System;
using System.Collections;
using System.Collections.Generic;
using TJ.Decaf.Base;
using UnityEngine;
using UnityEngine.UI;

namespace TJ.Decaf.MVVM.BottomTab
{
    using DG.Tweening;
    using System.Linq;

    public class BottomTabView : ViewBase
    {
        [Header("[Editor setting] - UI components")]
        [SerializeField] private ToggleGroup tabBarToggleGroup;

        [Space(20)]
        [SerializeField] private Image activeCircleImage;

        public ToggleGroup GetTabBarToggleGroup() => tabBarToggleGroup;

        public override void Hide(Action onComplete)
        {
        }

        public override void HideImmediate()
        {
        }

        public override void Show(Action onComplete)
        {
            Visible = VisibleState.Appeared;
            ChangeCirclePositionForActiveToggle();
            onComplete?.Invoke();
        }

        public override void ShowImmediate()
        {
        }

        #region [ Public methods ]
        public void ChangeCirclePositionForActiveToggle()
        {
            IEnumerator DelayShot()
            {
                yield return new WaitForEndOfFrame();

                var curActiveToggle = tabBarToggleGroup
                .ActiveToggles()
                .FirstOrDefault();

                if (curActiveToggle)
                {
                    var toggleTransform = curActiveToggle.transform as RectTransform;

                    var circleImageTransform = activeCircleImage.transform as RectTransform;
                    circleImageTransform
                        .DOAnchorPosX(toggleTransform.anchoredPosition.x - circleImageTransform.sizeDelta.x, 0.1f)
                        .Play();
                }
            }

            StartCoroutine(DelayShot());
        }
        #endregion
    }

}
