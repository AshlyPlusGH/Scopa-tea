using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Centralized scene management system with its own internal coroutine queue.
/// Handles loading, unloading, reloading, and tracking scenes safely.
/// </summary>
public class SCENE : DEBUGMonoBehaviour
{
    private static bool classDebug = true;

    [Scene] private static readonly List<string> builtScenes = new List<string>();
    [Scene] private static readonly List<string> loadedScenes = new List<string>();

    private static SCENE instance;

    // Internal coroutine queue
    private static readonly Queue<IEnumerator> routineQueue = new Queue<IEnumerator>();
    private static bool isRunningQueue = false;

    // ---------------------------------------------------------
    // SETUP
    // ---------------------------------------------------------
    #region Setup

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Setup();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes built scene list and registers the active scene as loaded.
    /// </summary>
    private static void Setup()
    {
        UpdateBuiltScenes();
        loadedScenes.Add(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Populates the list of scenes included in the build.
    /// </summary>
    private static void UpdateBuiltScenes()
    {
        builtScenes.Clear();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string name = SceneManager.GetSceneByBuildIndex(i).name;
            builtScenes.Add(name);
        }
    }

    #endregion

    // ---------------------------------------------------------
    // QUEUE SYSTEM
    // ---------------------------------------------------------
    #region Queue System

    /// <summary>
    /// Adds a coroutine to the internal queue and starts processing if idle.
    /// </summary>
    private static void QueueRoutine(IEnumerator routine)
    {
        routineQueue.Enqueue(routine);

        if (!isRunningQueue)
            instance.StartCoroutine(ProcessQueue());
    }

    /// <summary>
    /// Processes queued coroutines sequentially.
    /// </summary>
    private static IEnumerator ProcessQueue()
    {
        isRunningQueue = true;

        while (routineQueue.Count > 0)
        {
            yield return instance.StartCoroutine(routineQueue.Dequeue());
        }

        isRunningQueue = false;
    }

    #endregion

    // ---------------------------------------------------------
    // PUBLIC API — LOAD / UNLOAD / RELOAD
    // ---------------------------------------------------------
    #region Public API

    /// <summary>
    /// Loads a scene additively and sets it active.
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        QueueRoutine(COROUTINE_LoadScene(sceneName));
    }

    /// <summary>
    /// Unloads a specific scene by name.
    /// </summary>
    public static void UnloadScene(string sceneName)
    {
        QueueRoutine(COROUTINE_UnloadScene(sceneName, null));
    }

    /// <summary>
    /// Unloads the last loaded scene.
    /// </summary>
    public static void UnloadLastScene()
    {
        QueueRoutine(COROUTINE_UnloadLastScene(null));
    }

    /// <summary>
    /// Unloads the first loaded scene.
    /// </summary>
    public static void UnloadFirstScene()
    {
        QueueRoutine(COROUTINE_UnloadFirstScene(null));
    }

    /// <summary>
    /// Reloads a scene by unloading and loading it again.
    /// </summary>
    public static void ReloadScene(string sceneName)
    {
        QueueRoutine(COROUTINE_UnloadScene(sceneName));
        QueueRoutine(COROUTINE_LoadScene(sceneName));
    }

    /// <summary>
    /// Sets the current Active Scene.
    /// </summary>
    public static void SetActiveScene(string sceneName)
    {
        QueueRoutine(COROUTINE_SetActiveScene(sceneName));
    }

    #endregion

    // ---------------------------------------------------------
    // COROUTINES — LOAD / UNLOAD IMPLEMENTATION
    // ---------------------------------------------------------
    #region Coroutines

    /// <summary>
    /// Coroutine that loads a scene additively and sets it active.
    /// </summary>
    private static IEnumerator COROUTINE_LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        yield return null; // wait one frame

        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Scene not loaded: {sceneName}");
            yield break;
        }

        loadedScenes.Add(sceneName);
        if (classDebug) Debug.Log($"{sceneName} Scene Loaded!");

        SceneManager.SetActiveScene(scene);
    }

    /// <summary>
    /// Coroutine that unloads a scene by name.
    /// </summary>
    private static IEnumerator COROUTINE_UnloadScene(string sceneName, Action onSceneUnloaded = null)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Scene not loaded: {sceneName}");
            yield break;
        }

        yield return SceneManager.UnloadSceneAsync(sceneName);

        if (classDebug) Debug.Log($"{sceneName} Scene Unloaded!");

        loadedScenes.Remove(sceneName);
        onSceneUnloaded?.Invoke();
    }

    /// <summary>
    /// Coroutine that unloads the most recently loaded scene.
    /// </summary>
    private static IEnumerator COROUTINE_UnloadLastScene(Action onSceneUnloaded = null)
    {
        string sceneName = loadedScenes[loadedScenes.Count - 1];
        yield return COROUTINE_UnloadScene(sceneName, onSceneUnloaded);
    }

    /// <summary>
    /// Coroutine that unloads the earliest loaded scene.
    /// </summary>
    private static IEnumerator COROUTINE_UnloadFirstScene(Action onSceneUnloaded = null)
    {
        string sceneName = loadedScenes[0];
        yield return COROUTINE_UnloadScene(sceneName, onSceneUnloaded);
    }

    /// <summary>
    /// Coroutine that sets the current Active Scene.
    /// </summary>
    private static IEnumerator COROUTINE_SetActiveScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        yield return null;
    }

    #endregion

}