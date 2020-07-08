using System.Collections;
using System.Collections.Generic;
// using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WSFrameworkConst;
//zbr 2020

public abstract class WSClientBase : MonoBehaviour
{
    [Space(10)]
    [ReadOnly]
    public bool isConnected = false;
    protected bool IsConnected { get { return ws != null && ws.IsConnected; } }

    public CommStats statsSumary = new CommStats(); //temporary
    protected Queue<MessageEventArgs> messageQueue = new Queue<MessageEventArgs>();
    public string serviceName = "/test";
    protected Queue<bool> conntectionEvents = new Queue<bool>();
    protected WebSocketSharp.WebSocket ws;
    [SerializeField]
    protected int waitBetweenReconnectAttempts = 8;
    Coroutine connectWatchdog;
    protected abstract void OnMessageDequeue(MessageEventArgs message);
    Coroutine measure;
    GameObject myGameObject;
    protected WSTargetAddress target
    {
        get
        {
            if (_target == null) _target = GetComponentInParent<WSTargetAddress>();
            if (_target == null) _target = GameObject.FindObjectOfType<WSTargetAddress>();
            return _target;
        }
    }

    float nextStatsUpdate;
    float lastSnapthot;
    int measureinterval = 3;

    [SerializeField] WSTargetAddress _target;
    protected string GetServiceName()
    {
        string possiblename = this.GetType().ToString().ToLower();
        if (possiblename.StartsWith("ws"))
            possiblename = possiblename.Substring(2);
        if (possiblename.EndsWith("service"))
            possiblename = possiblename.Substring(0, possiblename.Length - "Service".Length);
        if (possiblename.EndsWith("client"))
            possiblename = possiblename.Substring(0, possiblename.Length - "client".Length);
        possiblename = possiblename.ToLower();
        return "/" + possiblename; ;
    }
    protected virtual void Reset()
    {
        serviceName = this.GetPossibleServiceName();
        if (target == null)
        {
            var go = new GameObject("WebsocketClientHelper");
            _target = go.AddComponent<WSTargetAddress>();
        }

    }
    public string websocketAddress
    {
        get
        {
            if (target == null)
            {
                DebugClient("Target Address for websocket client not found, please add WSTargetAddress component");
                return "unknown";
            }
            return "ws://" + target.ipAddress + ":" + target.port + serviceName;
        }
    }

    protected void DebugClient(string s, GameObject g = null)
    { //WSServer.frameCount +
        Debug.Log("c " + serviceName.MakeColor(Const.clientUsingServiceNameColor).Small() + " " + s.MakeColor(Const.clientUsingServiceNameMssage).Small(), myGameObject);
    }
    protected void DebugClientNonColor(string s, GameObject g = null)
    {
        //	Debug.Log(WSServer.frameCount + serviceName, myGameObject);
    }

    // public void SendString(string s)
    // {
    // 	statsSumary.AddBytesSent(s.Length);
    // 	if (ws != null && ws.IsConnected)
    // 		ws.SendString(s);
    // 	else
    // 	{
    // 		DebugClient("not connected");
    // 	}
    // }
    protected void SendBytes(byte[] bytes)
    {
        if (ws != null && ws.IsConnected)
        {
            ws.SendAsync(bytes, null);
            statsSumary.AddBytesSent(bytes.Length);
        }
        else
        {
            DebugClient("not connected");
        }
    }

    protected IEnumerator ConnectionWachdog()
    {
        int attempt = 0;
        while (true)
        {
            isConnected = ws.IsConnected;
            if (!ws.IsConnected)
            {
                ws.ConnectAsync();
                attempt++;
                if (attempt > 1)
                    DebugClient("connection attempt " + attempt);
            }
            yield return new WaitForSeconds(waitBetweenReconnectAttempts);
        }
    }
    public void StopConnect()
    {
        if (connectWatchdog != null) StopCoroutine(connectWatchdog);
    }
    void OnConnectedBase()
    {
        isConnected = true;
        if (statsSumary.printOnRecieve)
         DebugClient("Client connected to " + websocketAddress);

        if (measure != null) StopCoroutine(measure);
        measure = StartCoroutine(statsSumary.DataRateMeasurement());
        OnConnected();
    }
    protected virtual void OnConnected()
    {

    }
    void OnDisconnectedBase()
    {
        isConnected = false;
        if (statsSumary.printOnRecieve)
        DebugClientNonColor("Client Disconnected".MakeColor(Const.disconnectionMessage));
        if (measure != null) StopCoroutine(measure);
        OnDisconnected();
    }
    protected virtual void OnDisconnected()
    {

    }

    //     IEnumerator DataRateMeasurement()
    // {
    //     while (true)
    //     {
    //         startTime = Time.time;
    //         statsSumary.bytesPerSecond = statsSumary.bytesSinceTick / 3;
    //         statsSumary.bytesSinceTick = 0;

    //         statsSumary.rxBytesPerSecond = statsSumary.rxBytesSinceTick / 3;
    //         statsSumary.rxBytesSinceTick = 0;

    //         yield return new WaitForSeconds(3);
    //     }
    // }
    protected void OnMessageHandlerNonSync(object sender, MessageEventArgs e)
    {
        statsSumary.AddBytesRecieved(e.Data.Length);
        messageQueue.Enqueue(e);
    }
    public virtual void Connect()
    {

        ws = new WebSocketSharp.WebSocket(websocketAddress);
        ws.OnOpen += OnOpenHandlerNonSynced;
        ws.OnClose += OnCloseHandlerNonSynced;
        ws.OnMessage += OnMessageHandlerNonSync;
        ws.Connect();
        if (statsSumary.printOnSend)
        DebugClient("Connecting to " + websocketAddress + " " + ws.IsConnected);

    }
    public virtual void Disconnect()
    {

        if (ws!=null)
if (statsSumary.printOnSend)
        DebugClient("Disconnect request to " + websocketAddress + " " + ws.IsConnected);
        //ws.closeAsync(0, "requested closing");
        // ws = null;
        ws.Close();
        ws = null;
        // ws = new WebSocketSharp.WebSocket(websocketAddress);
        // ws.OnOpen += OnOpenHandlerNonSynced;
        // ws.OnClose += OnCloseHandlerNonSynced;
        // ws.OnMessage += OnMessageHandlerNonSync;
        // ws.Connect();

    }
    //    protected override void Update()
    // {
    //     base.Update();
    //     if (autoReuest)
    //     {
    //         requestcounter++;
    //         if (requestcounter > requestinterval)
    //         {
    //             requestcounter = 0;
    //             RequestFrame();
    //         }
    //     }
    // }

    private void OnOpenHandlerNonSynced(object sender, System.EventArgs e)
    {
        conntectionEvents.Enqueue(true);
        // DebugClient("client connected");
        //		DebugClientNonColor("Client is connected ".MakeColor(Const.connectionMessage));
    }

    private void OnCloseHandlerNonSynced(object sender, CloseEventArgs e)
    {
        conntectionEvents.Enqueue(false);
        DebugClientNonColor(("client:WebSocket closed with reason: " + e.Reason).MakeRed());
    }

    protected virtual void  Start()
    {
        myGameObject = gameObject;
        if (target != null)
        {
            target.OnConnectRequested += Connect;
            target.OnDisconnectRequested += Disconnect;
        }
        //	yield return null;
        //	yield return null;
        // #if UNITY_EDITOR
        if (target.clientsShouldAutoConnect && Time.time > 1)
        {
            Connect();
            //	connectWatchdog = StartCoroutine(ConnectionWachdog());
        }
        // #endif
    }
    void OnDestroy()
    {
        if (ws != null)
            ws.Close();
    }

    protected virtual void Update()
    {
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {

                OnMessageDequeue(messageQueue.Dequeue());
            }
        }
        lock (conntectionEvents)
        {
            while (conntectionEvents.Count > 0)
            {
                var thisEvent = conntectionEvents.Dequeue();
                if (thisEvent)
                    OnConnectedBase();
                else
                    OnDisconnectedBase();
            }
        }
        if (Time.time > nextStatsUpdate)
        {
            nextStatsUpdate = Time.time + measureinterval;
            // Debug.Log("mesring "+txBytesTotal );
            statsSumary.UpdateAverages(Time.time - lastSnapthot);
            lastSnapthot = Time.time;
        }

    }
}