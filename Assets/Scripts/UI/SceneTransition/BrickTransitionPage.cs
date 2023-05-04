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

        [SerializeField] private AnimationState curAnimationState = AnimationState.None;
        [SerializeField] private List<TransitionBrick> bricks;

        public void ShowImmediate()
            => bricks.ForEach(element => element.Show());
        public void HideImmediate()
            => bricks.ForEach(element => element.Hide());

        public void Show(Action onComplete)
        {
            if (curAnimationState == AnimationState.Playing)
                return;

            curAnimationState = AnimationState.Playing;

            bricks.ForEach(element => 
            {
                if(element.Visible == TransitionBrick.VisibleState.Disappeared)
                    element.ShowAnimation(0.25f);
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

                onComplete?.Invoke();

            }

            StartCoroutine(CheckingFinished());
        }

        public void Hide(Action onComplete)
        {
            if (curAnimationState == AnimationState.Playing)
                return;

            curAnimationState = AnimationState.Playing;

            bricks.ForEach(element =>
            {
                if(element.Visible == TransitionBrick.VisibleState.Appeared)
                    element.HideAnimation(0.25f);
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

                onComplete?.Invoke();

            }

            StartCoroutine(CheckingFinished());
        }
    }

}

