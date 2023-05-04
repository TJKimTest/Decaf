using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TJ.Decaf.MVVM.Main
{
    using System;
    using TJ.Decaf.Base;

    public class MainView : ViewBase
    {
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

