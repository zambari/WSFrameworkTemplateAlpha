using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
using WSFrameworkConst;

public class WSHierarchyService : WSOSCService
{
#if UNITY_EDITOR
    const bool pretty = true;
#else
    const bool pretty = false;
#endif
    public bool onlyRoots = true;
    public bool addObjectIDs = true;
    protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
    {
        //    	DebugService(" got :" + s.Data);

        string address = message.Address;
        if (address.StartsWith(Const.hierarchyScenesRequest))
        {
            SendSceneResponse(beh);
        }
        else
        if (address.StartsWith(Const.hierarchyRootRequest))
        {
            if (message.GetPayloadType(0) == typeof(string))
            {
                string scenename = message.GetString(0);
                SendRootsResponse(beh, scenename);
            }
            else
            {
                Debug.Log("no scene specified in request");
                SendRootsResponse(beh, null);
            }
        }
        else
        if (address.StartsWith(Const.hierarchyChildrenRequest))
        {
            ulong id = message.GetULong(0);
            SendChildrenResponse(id, beh);
        }
        else
        if (address.StartsWith(Const.active))
        {
            ulong id = message.GetULong(0);
            bool state = message.GetInt(1) == 1;
            var thisObject = id.FindObject();
            if (thisObject != null)
                thisObject.SetActive(state);
            else
            {
                DebugService("unknown id " + id.ToFingerprintString());
            }
        }
        else
        {
            DebugService("unknowna adress " + message.Address);
        }

    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    List<GameObject> FindObjectsFromScene(Scene scene)
    {
        var transforms = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        var objectsLoadedWithThisScene = new List<GameObject>();
        for (int i = 0; i < transforms.Length; i++)
            if (transforms[i].gameObject.scene == scene)
                objectsLoadedWithThisScene.Add(transforms[i]);
        return objectsLoadedWithThisScene;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var objectsLoadedWithThisScene = FindObjectsFromScene(scene);
        // StartCoroutine(SpreadProcessingScene(objectsLoadedWithThisScene, scene));
    }
    void OnSceneUnloaded(Scene scene)
    {
        // if (printDebugInfo) Debug.Log("onload " + scene.name);
        // lastLoadedSceneName = scene.name;
        var objectsLoadedWithThisScene = FindObjectsFromScene(scene);
        // StartCoroutine(SpreadProcessingScene(objectsLoadedWithThisScene, scene));
    }

    [ExposeMethodInEditor]
    void ListScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var thisccene = SceneManager.GetSceneAt(i);
            Debug.Log("scene " + thisccene);
        }

    }

    void SendSceneResponse(WSServiceBehaviour beh)
    {
        var response = WSHierarchySceneResponse.Report();
        OSCMessage message = new OSCMessage(Const.hierarchyScenesRequest);
        string serializerResponse = JsonUtility.ToJson(response, pretty);
        message.Append(serializerResponse);
        beh.Send(message);
    }
    void SendRootsResponse(WSServiceBehaviour beh, string sceneName)
    {
        if (sceneName == null) sceneName = SceneManager.GetSceneAt(0).name;

        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        var response = new WSHierarchyResponse();
        response.sceneName = sceneName;
        var alltransforms = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        var transformList = new List<Transform>();
        foreach (var t in alltransforms)
        {
            if ((!onlyRoots || t.parent == null) &&
                t.hideFlags == HideFlags.None
                // layer
            )
            {
                var oid = t.GetComponent<ObjectID>();
                if (oid == null && addObjectIDs)
                {
                    oid = t.gameObject.AddOrGetComponent<ObjectID>();
                    oid.Init(this);
                }
                if (oid != null && sceneName == null || t.gameObject.scene.name == sceneName)
                    transformList.Add(t);
            }
        }

        transformList.Sort(delegate(Transform go1, Transform go2)
        {
            return go1.GetSiblingIndex().CompareTo(go2.GetSiblingIndex());
        });

        response.nodes = new List<TransformNodeInfo>();
        for (int i = 0; i < transformList.Count; i++)
        {
            response.nodes.Add(new TransformNodeInfo(transformList[i]));
        }

        OSCMessage message = new OSCMessage(Const.hierarchyRootRequest);
        string serializerResponse = JsonUtility.ToJson(response, pretty);
        message.Append(serializerResponse);
        beh.Send(message);
        stopwatch.Stop();
    }

    void SendChildrenResponse(ulong id, WSServiceBehaviour beh)
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        Transform transform = id.FindTransform();
        if (transform == null)
        {
            DebugService("transform not found " + id);
            return;
        }
        WSHierarchyResponse response = new WSHierarchyResponse(transform, id);

        bool pretty = false;
#if UNITY_EDITOR
        pretty = true;
#endif
        OSCMessage message = new OSCMessage(Const.hierarchyChildrenRequest);
        string serializerResponse = JsonUtility.ToJson(response, pretty);
        message.Append(serializerResponse);
        beh.Send(message);
        stopwatch.Stop();
    }

}