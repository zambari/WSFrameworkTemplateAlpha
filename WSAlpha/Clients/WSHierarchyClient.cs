using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
using WSFrameworkConst;
using zUI;
//zbr 2020

public class WSHierarchyClient : WSOSCClient
{
    public RectTransform content;

    public string chidlTarget;
    // public Dictionary<string, Transform> scenePanelDict = new Dictionary<string, Transform>();
    public Dictionary<string, PanelInfo> scenePanelDict = new Dictionary<string, PanelInfo>();
    public Dictionary<ulong, PanelInfo> sceneParentDict = new Dictionary<ulong, PanelInfo>();
    public System.Action<ulong> onItemClicked;

    PanelInfo CreateScenePanel(string sceneName)
    {
        // var scenepanel = UIBuilder.BuildBasicPanelTopOnly(content as RectTransform, sceneName);
        //        scenepanel.top.rect.gameObject.AddComponent<Button>();
        var scenepanel = panelInfo.AddExpandable(sceneName);
        scenepanel.SetLabel(sceneName);
        scenepanel.name = "Panel for " + sceneName;
        scenepanel.mainRect.name += "SCENEPNALE";
        scenePanelDict.Add(sceneName, scenepanel);
        return scenepanel;
    }
    void HandleSceneList(WSHierarchySceneResponse scenes)
    {
        for (int i = 0; i < scenes.sceneInfos.Count; i++)
        {
            var thisscene = scenes.sceneInfos[i];
            if (!scenePanelDict.ContainsKey(thisscene.name))
            {
                CreateScenePanel(thisscene.name);
                OSCMessage scenereeq = new OSCMessage(Const.hierarchyRootRequest);
                scenereeq.Append(thisscene.name);
                Send(scenereeq);
            }
        }

    }

    void RequestHierarchyChildren(ulong id)
    {
        OSCMessage message = new OSCMessage(Const.hierarchyChildrenRequest);
        message.Append(id);
        Send(message);
    }
    public void SendSccenesRequest()
    {
        OSCMessage message = new OSCMessage(Const.hierarchyScenesRequest);
        Send(message);
    }

    [ExposeMethodInEditor]
    public void SendRootsRequest(string sceneName = null)
    {
        OSCMessage message = new OSCMessage(Const.hierarchyRootRequest);
        Send(message);
    }
    protected override void OnOSCMessage(OSCMessage message)
    {
        string address = message.Address;
        if (address.StartsWith(Const.hierarchyScenesRequest))
        {
            if (message.GetPayloadType(0) == typeof(string))
            {
                string payload = message.GetString(0);
                // Debug.Log("decoding roots " + payload);
                var response = JsonUtility.FromJson<WSHierarchySceneResponse>(payload);
                if (response != null)
                {
                    HandleSceneList(response);
                }
            }
        }
        else
        if (address.StartsWith(Const.hierarchyRootRequest))
        {
            if (message.GetPayloadType(0) == typeof(string))
            {
                string payload = message.GetString(0);
                // Debug.Log("decoding roots " + payload);
                var response = JsonUtility.FromJson<WSHierarchyResponse>(payload);
                if (response != null)
                    UpdateRoots(response);
            }
            else
            {
                Debug.Log("no string payload");
            }
        }
        else
        if (address.StartsWith(Const.hierarchyChildrenRequest))
        {
            if (message.GetPayloadType(0) == typeof(string))
            {
                string payload = message.GetString(0);
                var response = JsonUtility.FromJson<WSHierarchyResponse>(payload);
                if (response != null)
                    UpdateChilrenHierarchy(response);
            }
        }
        else
        {
            DebugClient("unknown address " + message.Address);
        }
    }

    protected override void OnDisconnected()
    {

        GameObject.Destroy(panelInfo.mainRect.gameObject);

    }
    protected override void OnConnected()
    {
        base.OnConnected();
        SendSccenesRequest();
        if (panelInfo.IsEmptyOrNull())
            panelInfo = content.CreateScrollRectPanel("Hierrraa", true);
        scenePanelDict = new Dictionary<string, PanelInfo>();
        sceneParentDict = new Dictionary<ulong, PanelInfo>();
    }

    void RequestAciveState(ulong id, bool active)
    {

        OSCMessage message = new OSCMessage(Const.active);
        message.Append(id);
        message.Append(active?1 : 0);
        Send(message);
        Debug.Log("sent " + message.ToReadableString());

    }
    private void OnValidate()
    {
        // if (content==null) content=transform as RectTransform;
        if (panelInfo.IsEmptyOrNull())
            panelInfo = content.CreateScrollRectPanel(name+"_panel", true);
    }
    PanelInfo CrateItem(TransformNodeInfo thisNode, RectTransform target)
    {
        //PanelInfo panelInfo = new PanelInfo(target);
        // Debug.Log("creating " + thisNode.name + " in " + target.name);
        // panelInfo = UIBuilder.BuildHierarchyPanel(target, "a" + thisNode.name);
        //  panelInfo = UIBuilder.BuildBasicPanelTopOnly(target, "a" + thisNode.name);
        var panelInfo = target.AddExpandable(thisNode.name, true);
        panelInfo.AddToggle();
        panelInfo.top.button = panelInfo.top.rect.AddOrGetComponent<Button>();
        var fold = panelInfo.top.rect.GetComponent<SimpleFoldController>();
        if (fold != null)
            fold._foldLabelText.enabled = thisNode.childCount > 0;
        panelInfo.SetLabel(thisNode.name + ((thisNode.childCount > 1) ? ("    (" + thisNode.childCount + ")") : ""));
        panelInfo.top.button.AddCallback(() => RequestHierarchyChildren(thisNode.id));
        panelInfo.top.button.AddCallback(() => { if (onItemClicked != null) onItemClicked(thisNode.id); });
        var toggle = panelInfo.top.rect.GetComponentInChildren<Toggle>();
        if (toggle != null)
        {
            toggle.isOn = thisNode.active;
            toggle.onValueChanged.AddListener((x) => RequestAciveState(thisNode.id, x));
        }
        else
        {
            Debug.Log("no toggle");
        }
        //  var fold = panelInfo.top.rect.GetComponent<SimpleFoldController>();
        // if (fold != null)
        // fold._foldLabelText.enabled = thisNode.childCount > 0;
        // top.SetColor(Color.Lerp(colorA, colorB, i * 1f / repsonse.nodes.Count));
        // panelInfo.SetLabel(thisNode.name);
        // panelInfo.top.button.AddCallback(() => RequestHierarchyChildren(thisNode.id));
        try
        {
            sceneParentDict.Add(thisNode.id, panelInfo);
        }
        catch (System.Exception e)
        {
            Debug.Log("duplicate key " + thisNode.id + " n " + thisNode.name + " " + e.Message);
        }
        return panelInfo;
    }
    void UpdateRoots(WSHierarchyResponse repsonse)
    {
        var filler = GetComponent<HierachyFiller>();
        if (!scenePanelDict.ContainsKey(repsonse.sceneName))
        {
            CreateScenePanel(repsonse.sceneName);
        }
        var scenepanel = scenePanelDict[repsonse.sceneName];
        var colorA = new Color(Random.Range(0.05f, 0.2f), Random.Range(0.3f, 0.8f), Random.Range(0.2f, .7f));
        var colorB = new Color(Random.Range(0.05f, 0.2f), Random.Range(0.3f, 0.8f), Random.Range(0.2f, .7f));
        colorA = colorA.SquareColor();
        colorB = colorB.SquareColor();
        for (int i = 0; i < repsonse.nodes.Count; i++)
        {
            var thisInfo = CrateItem(repsonse.nodes[i], scenepanel.content);
            Color c = Color.Lerp(colorA, colorB, i * 1f / repsonse.nodes.Count);
            c.a = 0.5f;
            thisInfo.top.SetColor(c);
        }
    }
    //Dictionary<ulong, List<ulong>> childrenIdDict= new Dictionary<ulong, List<ulong>>();

    public void UpdateChilrenHierarchy(WSHierarchyResponse repsonse)
    {
        PanelInfo target;
        if (sceneParentDict.TryGetValue(repsonse.id, out  target))
        {
            Color baseColor = target.color;
            if (baseColor.a > 0.8f) baseColor.a = 0.7f;
            // baseColor = baseColor.ShiftHue(0.05f);
            // baseColor = baseColor.ShiftSat(-.1f);
            baseColor.a -= -.2f;
            if (baseColor.a < .2f) baseColor.a = 0.2f;
            for (int i = 0; i < repsonse.nodes.Count; i++)
            {
                var thisNode = repsonse.nodes[i];
                if (sceneParentDict.ContainsKey(thisNode.id))
                {
                    Debug.Log("duplicate key??" + thisNode.id + " " + thisNode.id.ToFingerprintString() + " ->" + sceneParentDict[thisNode.id].name);

                }
                else
                {
                    var thisInfo = CrateItem(repsonse.nodes[i], target.mainRect);
                    //    PanelInfo panelInfo = UIBuilder.BuildHierarchyPanel(target.mainRect, "b" + thisNode.name);
                    //   panelInfo.top.button = panelInfo.top.rect.GetComponent<Button>();
                    baseColor = baseColor.ShiftHue(.1f / repsonse.nodes.Count);
                    baseColor = baseColor.ShiftSat(-.2f / repsonse.nodes.Count);
                    thisInfo.color = baseColor;

                }
            }
        }
        else
        {
            DebugClient("strange, unknown child");
        }

    }
    public PanelInfo panelInfo;

    protected override void Start()
    {
        base.Start();

    }

}