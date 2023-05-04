using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.UI
{
    using System;
    using System.Linq;
    
    public class BrickTransitionPage : MonoBehaviour
    {
        public enum AnimationState
        {
            None,
            Playing,
        }

        public enum VisibleState
        {
            None,

            Appearing,
            Appeared,

            Disappearing,
            Disappeared
        }

        [SerializeField] private AnimationState curAnimationState = AnimationState.None;
        [SerializeField] private VisibleState curVisibleState = VisibleState.None;
        [SerializeField] private List<TransitionBrick> bricks;

        public VisibleState GetVisible() => curVisibleState;

        public void ShowImmediate()
            => bricks.ForEach(element => element.Show());
        public void HideImmediate()
            => bricks.ForEach(element => element.Hide());

        public void Show(Action onComplete)
        {
            if (curAnimationState == AnimationState.Playing)
                return;

            curAnimationState = AnimationState.Playing;
            curVisibleState = VisibleState.Appearing;

            bricks.ForEach(element => 
            {
                if(element.Visible == TransitionBrick.VisibleState.Disappeared)
                    element.ShowAnimation(0.5f);
            });
            
            IEnumerator CheckingFinished()
            {
                bool playLooping = true;

                while (playLooping)
                {
                    var appearedBrickCount = bricks.Count(o => o.Visible == TransitionBrick.VisibleState.Appeared);
                    if (appearedBrickCount == bricks.Count)
                        playLooping = false;

                    yield return null;
                }

                curAnimationState = AnimationState.None;
                curVisibleState = VisibleState.Appeared;

                onComplete?.Invoke();

            }

            StartCoroutine(CheckingFinished());
        }

        public void Hide(Action onComplete)
        {
            if (curAnimationState == AnimationState.Playing)
                return;

            curAnimationState = AnimationState.Playing;
            curVisibleState = VisibleState.Disappearing;

            bricks.ForEach(element =>
            {
                if(element.Visible == TransitionBrick.VisibleState.Appeared)
                    element.HideAnimation(0.5f);
            });

            IEnumerator CheckingFinished()
            {
                bool playLooping = true;

                while (playLooping)
                {
                    var appearedBrickCount = bricks.Count(o => o.Visible == TransitionBrick.VisibleState.Appeared);
                    if (appearedBrickCount == bricks.Count)
                        playLooping = false;

                    yield return null;
                }

                curAnimationState = AnimationState.None;
                curVisibleState = VisibleState.Disappeared;

                onComplete?.Invoke();

            }

            StartCoroutine(CheckingFinished());
        }
    }

}

