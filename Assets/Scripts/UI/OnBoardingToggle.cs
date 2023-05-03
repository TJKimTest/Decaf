using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.UI
{
    using UniRx;
    using DG.Tweening;
    using UnityEngine.UI;
    using TJ.Decaf.Infra;

    public class OnBoardingToggle : MonoBehaviour
    {
        [Header("[Editor setting] - UI components")]
        [SerializeField] private Toggle myToggle;

        [Space(20)]
        [SerializeField] private Image check;

        [Space(20)]
        [SerializeField] private string value;

        private void Start()
        {
            RegisterToggleValueChanged();
        }

        public string GetValue()
            => value;

        public Toggle GetToggleComponent()
            => myToggle;

        #region [ Register observers ]
        private void RegisterToggleValueChanged()
            => myToggle.OnValueChangedAsObservable()
            .ThrottleFirst(GeneralConstants.ValueChangeThrottleTimeSeconds)
            .Subscribe(changedValue => 
            {
                var checkingImageTransform = check.transform as RectTransform;
                if (changedValue)
                {
                    checkingImageTransform.DOScale(1, 0.25f)
                    .SetEase(Ease.OutBounce)
                    .Play();
                }
                else
                {
                    checkingImageTransform.DOScale(0, 0.25f)
                    .SetEase(Ease.OutBounce)
                    .Play();
                }
            });
        #endregion

    }
}
