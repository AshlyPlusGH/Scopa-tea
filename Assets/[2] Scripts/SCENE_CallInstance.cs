using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCENE_CallInstance : MonoBehaviour
{
    [SerializeField] private bool debug;

    [Space(10)]

    [SerializeField, Scene] private string presetScene;

    public void LoadScene(Scene scene){ SCENE.LoadScene(scene.name); }
    public void LoadSceneByName(string sceneName){ if (debug){ Debug.Log("Attempting to Load Scene: " + sceneName); } SCENE.LoadScene(sceneName); }

    public void UnloadScene(Scene scene){ SCENE.UnloadScene(scene.name); }
    public void UnloadSceneByName(string sceneName){ if (debug){ Debug.Log("Attempting to Unload Scene: " + sceneName); } SCENE.UnloadScene(sceneName); }

    public void ChangeToSceneByName(string sceneName){ if (debug){ Debug.Log("Attempting to Change to Scene: " + sceneName); } SCENE.LoadScene(sceneName); SCENE.UnloadScene(SceneManager.GetActiveScene().name); }
        public void SwitchToSceneByName(string sceneName){ ChangeToSceneByName(sceneName); }
    public void ChangeToScenePreset(){ if (debug){ Debug.Log("Attempting to Change to Scene: " + presetScene); } SCENE.LoadScene(presetScene); SCENE.UnloadScene(SceneManager.GetActiveScene().name); }
        public void SwitchToScenePreset(){ ChangeToScenePreset(); }
}