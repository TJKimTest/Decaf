using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.Manager
{
    using TJ.Decaf.UI;

    public class SceneTransitionFactory : MonoBehaviour
    {
        [SerializeField] private BrickTransitionPage brickTransitionPage;

        public BrickTransitionPage GetBrickTransitionPage() => brickTransitionPage;

        private void Start()
        {
            brickTransitionPage.HideImmediate();
        }
    }

}
