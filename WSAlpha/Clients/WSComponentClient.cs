using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;
using zUI;
using Z.Reflection;

public class WSComponentClient : WSOSCClient
{
    // public WSComponentPopulator componentHandler;
    Dictionary<int, RectTransform> compoenntPanels;
    public WSHierarchyClient hierarchyClient;
    ulong currentObject;
    RectTransform content;
    ulong lastID;

    protected override void OnOSCMessage(OSCMessage message)
    {
        string address = message.Address;
        DebugClient("got message " + address + " " + message.typeTag);
        if (message.typeTag.Length > 0) {}
        //		ulong oid = message.GetULong(0);
        //	string id = oid.ToFingerprintString();

        if (address.StartsWith(Const.invalid))
        {
            DebugClient("id not found ");
        }

        if (address.StartsWith(Const.objectComponentsAddress))
        {
            GameObjectInfo info = JsonUtility.FromJson<GameObjectInfo>(message.GetString(0));
            if (info == null)
            {
                Debug.Log(" nofinf");
            }
            else
            {
                OnComponentList(info);
            }
            return;
        }

        if (address.StartsWith(Const.objectComponentsDetailsAddress))
        {
            DebugClient("got message detaols ===============");
            Debug.Log("in objectComponentsDetailsAddress :" + message.GetString(2));
            var descriptor = JsonUtility.FromJson<ComponentDescriptorWithHandles>(message.GetString(2));
            int componentID = message.GetInt(1);
            if (descriptor != null)
                OnComponentDetails(descriptor, componentID);
            else
            {
                Debug.Log("deserialkiation failed");;
            }
        }
        // for (int i = 1; i < message.typeTag.Length / 2; i++)
        // {
        // 	string componenName = message.GetString(i * 2);
        // 	int flags = message.GetInt(i * 2 + 1);
        // 	// Debug.Log(" component " + componenName + " flags " + flags);
        // }
    }
    public void RequestComponentDetails(ulong objectid, int componentId)
    {
        OSCMessage message = new OSCMessage(Const.objectComponentsDetailsAddress);
        message.Append(objectid);
        message.Append(componentId);
        SendAsync(message);
    }
    public void Release(ulong objectid, int componentID)
    {
        OSCMessage msg = new OSCMessage(Const.release);
        msg.Append(objectid);
        msg.Append(componentID);
        Send(msg);
    }
    List<GameObject> creteFields = new List<GameObject>();
    void ClearFields()
    {

        for (int i = 0; i < creteFields.Count; i++)
            GameObject.Destroy(creteFields[i]);
        creteFields.Clear();
    }
    void AddComponentField(RectTransform target, ValueProxy memberDescription)
    {
        ValueRemote value = new ValueRemote(memberDescription);
        switch (memberDescription.fieldType)
        {
            case MemberDescription.FieldType.FloatField:
                //var slider = prefabs.GetSlider(memberDescription.name);
                var slider = UIBuilder.BuildSlider(target, memberDescription.name);
                creteFields.Add(slider.slider.gameObject);
                value.BindSlider(slider);
                break;
            case MemberDescription.FieldType.IntField:
                var sliderint = UIBuilder.BuildSlider(target, memberDescription.name);
                //var sliderint = prefabs.GetSlider(memberDescription.name);
                creteFields.Add(sliderint.slider.gameObject);
                value.BindSlider(sliderint);
                sliderint.slider.wholeNumbers = true;
                break;
            case MemberDescription.FieldType.BoolField:
                var toggle = UIBuilder.BuildToggle(target, memberDescription.name); /// labeled tot
                creteFields.Add(toggle.gameObject);
                value.BindToggle(toggle);
                break;
            case MemberDescription.FieldType.StringField:
                var inputField = UIBuilder.BuildInputField(target, memberDescription.name);
                value.BindInputField(inputField);
                creteFields.Add(inputField.transform.parent.gameObject);
                break;
            case MemberDescription.FieldType.Void:
                Debug.Log("is void");
                var button = UIBuilder.BuildButton(target, memberDescription.name);
                creteFields.Add(button.gameObject);
                value.BindButton(button);
                break;
            default:
                Debug.Log("unknown fieldtype " + memberDescription.fieldType, gameObject);
                break;
        }

    }
    public void OnComponentDetails(ComponentDescriptorWithHandles info, int componentID)
    {
        RectTransform dest = null;
        currentObject = info.objectID;

        if (compoenntPanels.TryGetValue(componentID, out dest))
        {
            Debug.Log("for " + info.name + " the pnael is " + dest.name + " info.memberInstances" + info.memberInstances.Count, dest);
            //  UIPrefabHelper prefabs = UIPrefabProvider.Get(this, dest);
            for (int i = 0; i < info.memberInstances.Count; i++)
            {
                AddComponentField(dest, info.memberInstances[i]);
            }
        }
        else
        {
            Debug.Log("infalid request?");
        }
    }
    List<GameObject> createdComponentPanels = new List<GameObject>();

    public void OnComponentList(GameObjectInfo info)
    {

        if (info.id == lastID) return;
        lastID = info.id;

        ClearList();
        compoenntPanels = new Dictionary<int, RectTransform>();
        panelInfo.SetLabel("Components: " + info.name);
        for (int i = 0; i < info.componentInfos.Count; i++)
        {
            var thisInfo = info.componentInfos[i];
            string thiscompnentname = thisInfo.name;
            var img = panelInfo.mainRect.gameObject.AddOrGetComponent<Image>();
            img.color = Color.black * .4f;
            var thispanel = panelInfo.AddExpandable(thiscompnentname);
            createdComponentPanels.Add(thispanel.mainRect.gameObject);
            compoenntPanels.Add(thisInfo.componentID, thispanel.mainRect);
            thispanel.top.button.AddCallback(() =>
            {
                RequestComponentDetails(info.id, thisInfo.componentID);
                thispanel.top.button.onClick.RemoveAllListeners(); //only do int once

            }); // uwaga bug, dalej jest tu podczepiony toggle od folda i colliduje )
            Toggle activeToggle = thispanel.AddToggle();
            activeToggle.isOn = thisInfo.enabled;
            activeToggle.onValueChanged.AddListener((x) =>
            {
                OSCMessage message = new OSCMessage(Const.componentActive);
                message.Append(info.id);
                message.Append(thisInfo.componentID);
                message.Append(x?1 : 0);
                Send(message);
            });
        }
    }

 private void OnValidate()
    {
        // if (content==null) content=transform as RectTransform;
        if (panelInfo.IsEmptyOrNull())
            panelInfo = content.CreateScrollRectPanel(name+"_panel", true);
    }
    // public void OnComponentList(OSCMessage message)
    // {
    //     compoenntPanels = new Dictionary<string, Transform>();
    //     ClearList();
    //     var prefabs = UIPrefabProvider.Get(this, content);

    //     string typetag = message.typeTag;
    //     ulong id = message.GetULong(0);
    //     ulong displayingID = id;

    //     // if (typetag[1] == 'u')
    //     // {
    //     // 	var id 

    //     // }
    //     for (int i = 1; i < typetag.Length - 1; i++)
    //     {
    //         if (message.typeTag[i + 1] == 's')
    //         {
    //             string thisName = message.GetString(i);
    //             var thispanel = prefabs.GetPanel(thisName);
    //             compoenntPanels.Add(thisName, thispanel.transform);
    //             Button detailRequestButton = thispanel.GetComponentInChildren<Button>();
    //             //	detailRequestButton.onClick.RemoveAllListeners();
    //             detailRequestButton.onClick.AddListener(() => { RequestComponentDetails(id, thisName); });

    //         }
    //         else
    //         {
    //             // Debug.Log("ignoring flags ");
    //         }
    //     }

    // }
    //public List<

    protected virtual void ClearList()
    {

        for (int i = 0; i < createdComponentPanels.Count; i++)
            GameObject.Destroy(createdComponentPanels[i]);
        createdComponentPanels.Clear();
    }
    void OnHierarchyItemClicked(ulong id)
    {

        RequestComponentsFromObjectID(id);

    }
    public PanelInfo panelInfo;
    protected override void Start()
    {
        base.Start();
        if (hierarchyClient == null)
        {
            hierarchyClient = GameObject.FindObjectOfType(typeof(WSHierarchyClient)) as WSHierarchyClient;
        }
        if (hierarchyClient != null)
        {
            hierarchyClient.onItemClicked -= OnHierarchyItemClicked;
            hierarchyClient.onItemClicked += OnHierarchyItemClicked;
        }
        // content.SetPreferreedHeight(600);
        if (content == null) content = transform as RectTransform;
        // content.GetComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.Unconstrained;
        //panelInfo = new PanelInfo(content);
        panelInfo = content.CreateScrollRectPanel("Components", true);

        //     Debug.Log("info "+info.mainRect.name);
        //     panelInfo.AddFrame();
        //     // panelInfo.add
        // //    panelInfo.mainRect = content as RectTransform;
        //     content = UIBuilder.BuildScrollPanelWithFrames(panelInfo, "Component").content;
        //panelInfo.main
        //panelInfo.top.label = panelInfo.top.rect.GetComponentInChildren<Text>();
        // panelInfo.SetLabel("Components");
        // Debug.Log("created content " + content.name, content);
        //content.SetLabel("Componentes", true);
    }

    public void OnNodeClicked(TransformNodeInfo node)
    {
        RequestComponentsFromObjectID(node.id);
    }

    public void RequestComponentsFromObjectID(ulong id)
    {
        OSCMessage message = new OSCMessage(Const.objectComponentsAddress);
        message.Append(id);
        DebugClient(message.ToReadableString());
        Send(message);
    }

}