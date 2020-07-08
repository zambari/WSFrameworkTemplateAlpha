using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;
using WSFrameworkConst;

public class WSServerForwarder : WSServer
{
    // [SerializeField] string _targetIP = "127.0.0.1";
    // public string targetIP { get { return _targetIP; } set { _targetIP = value; } }
    // public int targetPort { get { return _targetport; } set { _targetport = value; } }
    // public int _targetport = 4649;

    [System.Serializable]
    public class TargetConfig
    {
        public string url = "ws://127.0.0.1:4649";
        public bool enable = true;
        public bool muteSends = false;
        public bool muteRecieves = false;
        public bool pickForRecieving = false;
    }

    [System.Serializable]
    public class MultiServerConfig
    {
        public List<TargetConfig> targetAddresses = new List<TargetConfig> { new TargetConfig() };
    }
    public MultiServerConfig config;

    public void SetTargets(MultiServerConfig targetAddresses)
    {
        config = targetAddresses;
    }
    IEnumerator StatsRoutine()
    {
        var wait = new WaitForSeconds(.4f);
        if (statsObject == null) yield break;
        while (true)
        {
            if (statsObject.activeInHierarchy)
                GetStats();
            yield return wait;
        }
    }
    protected void Start()
    {
        // base.Start();
        StartCoroutine(StatsRoutine());
    }
    public void AddFakeService(WSServiceForwarder service)
    {
        if (server == null)
        {
            server = new WebSocketServer(port);
            server.Start();
            DebugServer("Server opened port " + port);
        }
        server.AddWebSocketService<WSFakeServiceBehavior>(service.serviceName, service.InitializerFake);
        //services.Add(service);
        DebugServer("  " + service.serviceName + ":started  ");
    }
    Text[] texts;
    Toggle[] toggles1;
    Toggle[] toggles2;
    public GameObject statsObject;
    void CounChanged()
    {
        var parent = statsObject.transform.parent;
        for (int i = 0; i < parent.childCount - 1; i++)
        {
            if (parent.GetChild(i) != statsObject.transform)
            {
                GameObject.Destroy(parent.GetChild(i).transform);
            }
        }
        statsObject.gameObject.SetActive(false);
        texts = new Text[config.targetAddresses.Count];
        toggles1 = new Toggle[config.targetAddresses.Count];
        toggles2 = new Toggle[config.targetAddresses.Count];
        for (int i = 0; i < config.targetAddresses.Count; i++)
        {
            var newStats = Instantiate(statsObject, statsObject.transform);
            newStats.SetActive(true);
            texts[i] = newStats.GetComponentInChildren<Text>();
            var tgs = newStats.GetComponentsInChildren<Toggle>();;
            if (tgs.Length == 2)
            {
                toggles1[i] = tgs[0];
                toggles2[i] = tgs[1];
            }
        }
        rxCount = new int[config.targetAddresses.Count];
        txCount = new int[config.targetAddresses.Count];

    }
    public int[] connected;
    public int[] rxCount;
    public int[] txCount;
    public int pickeda = -1;
    public int pickedb = -1;

    public void GetStats()
    {
        var adr = config.targetAddresses;
        if (adr.Count != texts.Length)
            CounChanged();
        for (int i = 0; i < adr.Count; i++)
        {
            var thisad = adr[i];
            string thisTExt = rxCount[i] + "\n" + txCount[i] + "\n" + connected[i];
            toggles1[i].isOn = thisad.muteSends;
            toggles2[i].isOn = thisad.muteRecieves;
            toggles1[i].onValueChanged.AddListener((x) => thisad.muteSends = x);
            toggles2[i].onValueChanged.AddListener((x) => thisad.muteRecieves = x);
        }
    }
    // #if UNITY_EDITOR
    [ExposeMethodInEditor]
    void AddDummyServices()
    {
        var serviceforwwraders = GetComponents<WSServiceForwarder>();
        for (int i = serviceforwwraders.Length - 1; i >= 0; i--) DestroyImmediate(serviceforwwraders[i]);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (Type type in assembly.GetTypes())
                if (type.IsSubclassOf(typeof(WSServiceBase)))
                {
                    WSServiceForwarder ws = gameObject.AddComponent<WSServiceForwarder>();
                    ws.serviceName = Const.GetPossibleServiceName(type.ToString());
                    Debug.Log(" " + type + " " + ws.serviceName);
                }
    }

    // #endif
}