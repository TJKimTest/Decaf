using System;
using System.Collections;
using System.Collections.Generic;
using TJ.Decaf.Base;
using UnityEngine;
using UnityEngine.UI;

namespace TJ.Decaf.MVVM.Home
{
    public class HomeView : ViewBase
    {

        [Header("[Editor setting] - UI components")]
        [SerializeField] private Button openNearbyCafesRootButton;

        public Button GetOpenNearbyCafesRootButton()
            => openNearbyCafesRootButton;

        public override void Hide(Action onComplete)
        {
        }

        public override void HideImmediate()
        {
        }

        public override void Show(Action onComplete)
        {
            Visible = VisibleState.Appeared;
            onComplete?.Invoke();
        }

        public override void ShowImmediate()
        {
        }
    }

}
