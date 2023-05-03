using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.Base
{
    using System;
    using UniRx;

    public abstract class ViewBase : MonoBehaviour
    {
        public enum VisibleState
        {
            None,

            Appearing, 
            Appeared, 
            
            Disappearing, 
            Disappeared
        }

        [field: SerializeField] public VisibleState Visible { get; protected set; } = VisibleState.None;

        protected CompositeDisposable Disposables { get; set; } = new CompositeDisposable();

        public Action OnShow { get; set; }
        public Action OnHide { get; set; }

        public abstract void Show(Action onComplete);
        public abstract void ShowImmediate();
        public abstract void Hide(Action onComplete);
        public abstract void HideImmediate();
    }
}

