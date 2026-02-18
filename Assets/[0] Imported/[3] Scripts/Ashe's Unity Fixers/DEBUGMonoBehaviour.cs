using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEBUGMonoBehaviour : MonoBehaviour
{
    //Debug Debugging!
    [OnValueChanged("DEBUGResetDebugOptions")]
    public bool showDebugOptions;

    [ShowIf("showDebugOptions")]
    [Button("Disable All Debugging!")]
    private void DEBUGDisableAll()
    {
        foreach (DEBUGMonoBehaviour debugScript in FindObjectsByType<DEBUGMonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            debugScript.DEBUGDisableThis(); // Example: disable debug on all

            debugScript.showDebugOptions = false;
        }
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (DEBUGMonoBehaviour debugScript in root.GetComponentsInChildren<DEBUGMonoBehaviour>(true))
                {
                    debugScript.DEBUGDisableThis();

                    debugScript.showDebugOptions = false;
                }
            }
        }
    }

    [ShowIf("showDebugOptions")]
    [Button("Enable All Debugging!")]
    private void DEBUGEnableAll()
    {
        foreach (DEBUGMonoBehaviour debugScript in FindObjectsByType<DEBUGMonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            debugScript.DEBUGEnableThis(); // Example: disable debug on all
        }
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (DEBUGMonoBehaviour debugScript in root.GetComponentsInChildren<DEBUGMonoBehaviour>(true))
                {
                    debugScript.DEBUGEnableThis();
                }
            }
        }
    }

    [ShowIf("showDebugOptions")]
    [SerializeField] public bool debug;

    private float value;

    protected virtual void DEBUGResetDebugOptions()
    {
        debug = false;
    }

    private void DEBUGDisableThis()
    {
        debug = false;
        showDebugOptions = false;
    }

    private void DEBUGEnableThis()
    {
        debug = true;
        showDebugOptions = true;
    }
}