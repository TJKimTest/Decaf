using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.UI
{

    using DG.Tweening;

    public class TransitionBrick : MonoBehaviour
    {
        public enum VisibleState
        {
            None,

            Appearing,
            Appeared,

            Disappearing,
            Disappeared
        }

        public VisibleState Visible { get; private set; } = VisibleState.None;

        public void Show()
        {
            transform.localScale = Vector3.one;
            transform.localRotation = new Quaternion(0, 0, 90, 0);

            Visible = VisibleState.Appeared;
        }

        public void Hide()
        {
            transform.localScale = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Visible = VisibleState.Disappeared;
        }

        public void ShowAnimation(float delayTime = 0)
        {
            Visible = VisibleState.Appearing;

            transform.DOKill(true);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delayTime);
            sequence.Append(transform.DOScale(Vector3.one, 0.25f));
            sequence.Join(transform.DORotate(Vector3.forward * 90, 0.5f));
            sequence.AppendCallback(() => 
            {
                Visible = VisibleState.Appeared;
            });
        }

        public void HideAnimation(float delayTime = 0)
        {
            Visible = VisibleState.Disappearing;

            transform.DOKill(true);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delayTime);
            sequence.Append(transform.DORotate(Vector3.zero, 0.5f));
            sequence.Join(transform.DOScale(Vector3.zero, 0.5f));
            sequence.AppendCallback(() => 
            {
                Visible = VisibleState.Disappeared;
            });
        }
    }

}
