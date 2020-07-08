using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;
using WSFrameworkConst;
// v.01. twaks

/// <summary>
/// You might consider inhrinting from the OSC variant instead
/// </summary>
public abstract class WSServiceBase : MonoBehaviour
{
    public WSServer server { get { if (_server == null) GetServer(); return _server; } }

    [SerializeField] protected WSServer _server;
    [ReadOnly]
    public int connectedClients;

    [SerializeField] protected string _serviceName = "/test";

    public CommStats stats = new CommStats();
    public string serviceName { get { return _serviceName; } set { _serviceName = value; } } // "/ping";
    [Space]
    public bool autoStart = true;
    [Space]
    protected List<WSServiceBehaviour> clientHanlders = new List<WSServiceBehaviour>();
    protected abstract void OnMessageDequeue(WSServiceBehaviour beh, WebSocketSharp.MessageEventArgs message);
    protected void OnErrror(WSServiceBehaviour behaviour, WebSocketSharp.ErrorEventArgs e)
    {
        DebugService("WSServiceBehaviour Got error " + e.Message);
    }
    protected void OnClose(WSServiceBehaviour behaviour, WebSocketSharp.CloseEventArgs e)
    {
        // Debug.Log("not removing handler !");
        // if (clientHanlders.Contains(behaviour))
        // 	clientHanlders.Remove(behaviour);
        // connectedClients = clientHanlders.Count;
        if (string.IsNullOrEmpty(e.Reason))
            DebugService("WSServiceBehaviour closed");
        else
            DebugService("WSServiceBehaviour closed with reason " + e.Reason);
    }
    GameObject myGameObject;

    // protected void BroacdcastString(string s)
    // {
    // 	lock(clientHanlders)
    // 	{
    // 		foreach (var c in clientHanlders)
    // 		{
    // 			c.SendString(s);
    // 			stats.AddBytesSent(s.Length);
    // 		}
    // 		if (clientHanlders.Count == 0)
    // 			DebugService("no clients connected");
    // 		else
    // 			DebugService("Broadcast '" + s + "' (" + s.Length + " bytes) to " + clientHanlders.Count + " clients");
    // 	}
    // }

    protected void BroacdcastBytes(byte[] s)
    {
        lock (clientHanlders)
        {
            foreach (var c in clientHanlders)
            {
                c.SendAsync(s, null);
                stats.AddBytesSent(s.Length);
            }
            if (clientHanlders.Count == 0)
                DebugService("no clients connected");
        }
    }

    protected virtual void OnOpen(WSServiceBehaviour serviceBehaviour)
    {
        //	clientHanlders.Add(serviceBehaviour);
        // DebugService("WSServiceBehaviour Open ");
    }

    protected virtual void Reset()
    {
        GetServer();

        serviceName = this.GetPossibleServiceName();
    }
    void GetServer()
    {
        if (_server == null) _server = GetComponent<WSServer>();
        if (_server == null) _server = GameObject.FindObjectOfType<WSServer>();
    }

    protected virtual void Start()
    {
        myGameObject = gameObject;
        if (autoStart)
            if (server != null)
            {
                server.AddService(this);
            }
    }

    public void Initializer(WSServiceBehaviour beh)
    {
        // Debug.Log("service run initialize");
        lock (clientHanlders)
        {
            beh.stats = stats;
            clientHanlders.Add(beh);
            connectedClients = clientHanlders.Count;
        }
    }

    protected void DebugService(string s)
    {
        string preppedstring = ("S " + serviceName.MakeColor(Const.serviceNameColor).Small() + " " + s.MakeColor(Const.serviceMessageColor));
        //zBench.DebugOnceInAWhile(preppedstring, gameObject);
        Debug.Log(preppedstring, gameObject);

        // Debug.Log(preppedstring, myGameObject);
    }
    List<WSServiceBehaviour> inactiveBehs;
    protected void UpdateQueues()
    {
        foreach (var thisClient in clientHanlders)
        {

            while (thisClient.messageQueue.Count > 0)
            {
                MessageEventArgs msg;
                lock (thisClient.messageQueue)
                {
                    msg = thisClient.messageQueue.Dequeue();
                }
                OnMessageDequeue(thisClient, msg);
            }
            if (thisClient.closedQueue.Count > 0)
            {
                CloseEventArgs close;
                lock (thisClient.closedQueue)
                {
                    close = thisClient.closedQueue.Dequeue();
                }
                if (close == null)
                {
                    OnOpen(thisClient);
                    if (server != null)
                        server.OnClientConnectedNotification(this);
                }
                else
                {
                    if (inactiveBehs == null) inactiveBehs = new List<WSServiceBehaviour>();
                    inactiveBehs.Add(thisClient);
                    OnClose(thisClient, close);

                }
            }
            if (thisClient.errorQueue.Count > 0)
            {
                ErrorEventArgs err;
                lock (thisClient.errorQueue)
                {
                    err = thisClient.errorQueue.Dequeue();
                }
                OnErrror(thisClient, err);
            }
        }
        if (inactiveBehs != null)
        {
            foreach (var deactivated in inactiveBehs)
            {
                if (clientHanlders.Contains(deactivated))
                    clientHanlders.Remove(deactivated);
                connectedClients = clientHanlders.Count;
                inactiveBehs = null;
            }
            if (server != null)
                server.OnClientDisconnectedNotification(this);
        }
    }

    void Update()
    {
        UpdateQueues();

    }

}