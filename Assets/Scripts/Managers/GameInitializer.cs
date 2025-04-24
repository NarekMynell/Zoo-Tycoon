using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private string mainSceneAddress = "MainScene";
    [SerializeField] private ProgressView _progressView;

    private AsyncOperationHandle<SceneInstance>? _loadHandle;
    private bool _isSceneLoaded = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        LoadScene(mainSceneAddress);
    }

    public void LoadScene(string sceneAddress)
    {
        if (_isSceneLoaded)
        {
            Debug.LogWarning("A scene is already loaded. Unload it before loading a new one.");
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneAddress));
    }

    private IEnumerator LoadSceneAsync(string sceneAddress)
    {
        var handle = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Additive, activateOnLoad: false);
        _loadHandle = handle;

        while (!handle.IsDone)
        {
            OnProgressUpdated(handle.PercentComplete);
            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            OnProgressUpdated(100);
            _isSceneLoaded = true;
            OnSceneLoaded();
        }
        else
        {
            SceneLoadExceptionHandler(new Exception($"Failed to load scene: {sceneAddress}"));
        }
    }

    public void ActivateLoadedScene()
    {
        if (_isSceneLoaded && _loadHandle.HasValue)
        {
            _loadHandle.Value.Result.ActivateAsync().completed += _ =>
            {
                SceneManager.SetActiveScene(_loadHandle.Value.Result.Scene);

                SceneManager.UnloadSceneAsync("InitScene");
            };
        }
        else
        {
            SceneLoadExceptionHandler(new Exception("No scene is loaded or handle is invalid."));
        }
    }

    private void OnProgressUpdated(float progress)
    {
        _progressView.Set(progress, 100);
    }

    private void OnSceneLoaded()
    {
        ActivateLoadedScene();
    }

    private void SceneLoadExceptionHandler(Exception exception)
    {
        Debug.LogError(exception.Message);
        // You can expand this with UI alert or fallback logic
    }
}
