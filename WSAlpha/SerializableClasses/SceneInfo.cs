using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class WSHierarchySceneResponse : WSFrameworkMessage
{
    public List<SceneInfo> sceneInfos;
    public WSHierarchySceneResponse()
    {

    }
    public static WSHierarchySceneResponse Report()
    {
        var newinfo = new WSHierarchySceneResponse();
        newinfo.sceneInfos = new List<SceneInfo>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var thisScene = SceneManager.GetSceneAt(i);
            newinfo.sceneInfos.Add(new SceneInfo(thisScene));
            Debug.Log("scene " + thisScene.name);
        }
        return newinfo;
    }

}

[System.Serializable]
public class SceneInfo
{
    public string name;
    public SceneInfo(Scene scene)
    {
        name = scene.name;
    }
}