using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleSceneLoader : MonoBehaviour
{
    public string[] scenes;
    // void GetScenes()
    // {

    // IEnumerator Start()
    // {

    //     int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
    //     string[] scenes = new string[sceneCount]; //
    //     // default build scenes
    //     for (int i = 1; i < 3; i++)
    //     {
    //         yield return new WaitForSeconds(2);
    //         scenes[i] = Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
    //         StartCoroutine(LoadFromBuildRoutine(scenes[i]));
    //     }

    // }
    [ExposeMethodInEditor]
    public void LoadSelected()
    {
        selectedScene = selectedScene;
        if (selectedScene < 1)
        {
            selectedScene = 0;
            return;
        }

        var async = SceneManager.LoadSceneAsync(selectedName, LoadSceneMode.Additive);
    }

    [Range(0, 10)]
    [SerializeField] int _selectedScene;
    public int selectedScene
    {
        get { return _selectedScene; }
        set
        {
            _selectedScene = value;
            if (_selectedScene < 0) _selectedScene = 0;
            if (_selectedScene >= scenes.Length) _selectedScene = scenes.Length - 1;
            if (scenes.Length > 0)
                selectedName = scenes[selectedScene];
        }
    }
    public string selectedName;
    void Start()
    {
        GetScenes();
    }

    [ExposeMethodInEditor]
    void GetScenes()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        scenes = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i - 1));
        }
    }
    // }
    // IEnumerator LoadFromBuildRoutine(string sceneName)
    // {
    //     var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    //     yield return null;
    // }

}