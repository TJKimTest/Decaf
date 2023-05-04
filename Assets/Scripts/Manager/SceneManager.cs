using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TJ.Decaf.Manager
{
    using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

    public enum SceneType
    {
        None,

        Config,
        
        Splash,

        OnBoarding,
        Main,
    }

    public class SceneManager : MonoBehaviour
    {
        private const string Header = " [ SceneManager ] ";

        [field: SerializeField] public SceneType CurrentSceneType { get; private set; } = SceneType.None;
        [field: SerializeField] public SceneType PrevSceneType { get; private set; } = SceneType.None;
        [field: SerializeField] public SceneType NextSceneType { get; private set; } = SceneType.None;

        private Action onCompleteLoaded;

        public override string ToString()
        {
            return Header;//base.ToString();
        }

        #region [ Get & Set ]
        public void SetCurrentSceneType(SceneType type) => CurrentSceneType = type;
        public void SetCurrentSceneType(SceneType curType, SceneType nextType)
        {
            CurrentSceneType = curType;
            NextSceneType = nextType;
        }
        #endregion

        #region  [ Public methods ]
        public void ChangeScene(SceneType sceneType, LoadSceneMode loadSceneMode)
        {
            SwapCurrentTypeWithPreviousType(sceneType);

            var convScenceName = sceneType.ToString().Split('_')[0];

            UnitySceneManager.LoadScene(convScenceName, loadSceneMode);
        }
        public void ChangeScene(SceneType sceneType, LoadSceneMode loadSceneMode, Action onComplete)
        {
            SwapCurrentTypeWithPreviousType(sceneType);

            var convScenceName = sceneType.ToString().Split('_')[0];

            UnitySceneManager.LoadScene(convScenceName, loadSceneMode);
            onCompleteLoaded = onComplete;

            UnitySceneManager.sceneLoaded -= OnCompleteLoaded;
            UnitySceneManager.sceneLoaded += OnCompleteLoaded;
        }
        public void ChangeNextScene(LoadSceneMode loadSceneMode)
        {
            if (NextSceneType != SceneType.None)
            {
                SwapCurrentTypeWithPreviousType(NextSceneType);

                var convScenceName = NextSceneType.ToString().Split('_')[0];

                UnitySceneManager.LoadScene(convScenceName, loadSceneMode);
            }
        }
        public void ChangeSceneAsync(SceneType sceneType)
        {
            SwapCurrentTypeWithPreviousType(sceneType);

            IEnumerator LoadSceneAsync()
            {
                var convScenceName = sceneType.ToString().Split('_')[0];

                var loadOperation = UnitySceneManager.LoadSceneAsync(convScenceName, LoadSceneMode.Single);
                loadOperation.allowSceneActivation = false;

                while (loadOperation.isDone == false)
                {
                    yield return null;
                    if (loadOperation.progress >= 0.9f)
                    {
                        loadOperation.allowSceneActivation = true;
                    }
                }
            }

            StartCoroutine(LoadSceneAsync());
        }

        public void ChangeNextSceneAsync()
        {
            if (NextSceneType != SceneType.None)
            {
                SwapCurrentTypeWithPreviousType(NextSceneType);
                ChangeSceneAsync(CurrentSceneType);
            }
        }
        #endregion

        #region [ Private methods ]
        private void OnCompleteLoaded(Scene curLoadedScene, LoadSceneMode curLoadedSceneMode)
        {
            onCompleteLoaded?.Invoke();
            onCompleteLoaded = null;
        }
        private void SwapCurrentTypeWithPreviousType(SceneType sceneType)
        {
            PrevSceneType = CurrentSceneType;
            CurrentSceneType = sceneType;
        }
        #endregion

    }

}

