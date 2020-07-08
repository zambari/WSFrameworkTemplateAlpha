using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WSServer : MonoBehaviour
{
    public int port = 4649;
    protected WebSocketServer server;
    static List<WSServer> serverHosts;
    public CommStats statsSumary = new CommStats(); //temporary
    protected Queue<MessageEventArgs> messageQueue = new Queue<MessageEventArgs>();
    GameObject thisGameObject;
    public static int frameCount;
    System.Text.StringBuilder agregateLogBuilder = new System.Text.StringBuilder();

    bool waiterrunning;
    float debugPrintTimeInFuture;
    public BoolEvent OnAnyServiceConnected = new BoolEvent();
    public int activeServiesCount = 0;
    public Text portAndStatusText;
    IEnumerator DebugWaiter()
    {
        waiterrunning = true;
        while (Time.time < debugPrintTimeInFuture)
            yield return null;
        DebugServer(agregateLogBuilder.ToString());
        agregateLogBuilder = null;
        waiterrunning = false;

    }
    void DebugInAWhile(string s)
    {
        if (agregateLogBuilder == null) agregateLogBuilder = new System.Text.StringBuilder();
        agregateLogBuilder.Append(s);
        if (!waiterrunning)
        {
            debugPrintTimeInFuture = Time.time + 1;
            StartCoroutine(DebugWaiter());
        }
    }
    void Reset()
    {
        var currentserver = GameObject.FindObjectOfType<WSServer>();
        if (currentserver != null && currentserver != this)
        {
            Debug.Log("Please only one server on scene (for now at least) click to see current ", currentserver.gameObject);
        }
        name = "FrameworkServer";
    }
    IEnumerator TextUpdater()
    {

        while (true)
        {
            yield return new WaitForSeconds(1);
            ShowStatus();
        }
    }
    void Awake()
    {
        thisGameObject = gameObject;
        if (portAndStatusText != null) StartCoroutine(TextUpdater());
         
    }
    protected void OnMessageHandlerNonSync(object sender, MessageEventArgs e)
    {
        statsSumary.AddBytesRecieved(e.Data.Length);
        messageQueue.Enqueue(e);
    }

    // public void StartServer(string serviceName, int _port)
    // {
    // 	if (serverHosts == null)
    // 		serverHosts = new List<WSServer>();
    // 	serverHosts.Add(this);
    // 	port = _port;
    // 	DebugInAWhile("Started " + name + " server @" + _port + " " + serviceName + " ");
    // }

    public void OnClientConnectedNotification(WSServiceBase service)
    {
        activeServiesCount++;
        DebugServer(activeServiesCount + "+ connection recievred from " + service.serviceName);
        if (activeServiesCount == 1) OnAnyServiceConnected.Invoke(true);
        ShowStatus();
    }
    void ShowStatus()
    {
        portAndStatusText.SetText(port + " : rx:" + statsSumary.rxBytesTotal + " tx:" + statsSumary.txBytesTotal);
    }
    public void OnClientDisconnectedNotification(WSServiceBase service)
    {
        activeServiesCount--;
        // DebugServer(activeServiesCount + "- disconne recievred from " + service.serviceName);
        if (activeServiesCount == 0) OnAnyServiceConnected.Invoke(false);
    }
    public List<WSServiceBase> services = new List<WSServiceBase>();
    public void AddService(WSServiceBase service)
    {
        if (server == null)
        {
            server = new WebSocketServer(port);
            server.Start();
            DebugInAWhile("Server opened port " + port);
        }
        server.AddWebSocketService<WSServiceBehaviour>(service.serviceName, service.Initializer);
        services.Add(service);
        DebugInAWhile("  " + service.serviceName + ":started  ");
    }

    public static WSServer Get()
    {
        if (serverHosts.Count > 0) return serverHosts[0];
        return ((new GameObject("WSServer")).AddComponent<WSServer>());
    }
    protected void DebugServer(string s, GameObject g = null)
    {
        Debug.Log(s.MakeColor(new Color(0.3f, 0.3f, 0.7f)), thisGameObject);
    }

    // #if UNITY_EDITOR
    [ExposeMethodInEditor]
    void AddAllAvailableScanserives()
    {

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (Type type in assembly.GetTypes())
                if (type.IsSubclassOf(typeof(WSServiceBase)))
                    if (gameObject.GetComponent(type) == null)
                    {
                        Debug.Log("type found " + type + " adding components");
                        gameObject.AddComponent(type);
                    }
    }

    void Update()
    {
        frameCount = Time.frameCount;
    }
    // #endif
}