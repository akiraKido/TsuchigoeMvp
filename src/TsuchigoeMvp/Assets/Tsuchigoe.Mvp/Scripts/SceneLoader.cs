#nullable enable

using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuchigoe.Mvp
{
    public static class SceneLoader
    {
        private static Action<Model>? _currentInitializer;

        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(0, 1);

#if UNITY_EDITOR
        private static string? _currentLockSceneName;
#endif

        internal static void PresenterLoadComplete(Model model)
        {
            _currentInitializer?.Invoke(model);
            if (Semaphore.CurrentCount > 0)
            {
                Semaphore.Release();
            }
        }

        public static void LoadScene(
            string         sceneName,
            Action<Model>? modelInitializer = null,
            LoadSceneMode  loadSceneMode    = LoadSceneMode.Single)
        {
            // データ受け渡しが必要無ければ同時にシーンをロードされても問題ない
            // データ受け渡しが発生するなら、シーンのロードが終わってからでないとアンロックさせない

            if (modelInitializer != null)
            {
                LoadSceneLocked(sceneName, modelInitializer, loadSceneMode);
            }
            else
            {
                LoadSceneNoLock(sceneName, loadSceneMode);
            }
        }

        private static async void LoadSceneLocked(
            string        sceneName,
            Action<Model> modelInitializer,
            LoadSceneMode loadSceneMode)
        {
#if UNITY_EDITOR
            if (Semaphore.CurrentCount != 0)
            {
                Debug.Log($"Waiting for scene to load: {_currentLockSceneName}");
            }

            _currentLockSceneName = sceneName;
#endif

            await Semaphore.WaitAsync();

            _currentInitializer = modelInitializer;

            SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        private static void LoadSceneNoLock(string sceneName, LoadSceneMode loadSceneMode)
        {
            SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }
    }
}