using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WSFrameworkConst;

[RequireComponent(typeof(WSServerForwarder))]
public class WSServiceForwarder : WSServiceBase
{
    WSServerForwarder forwarder { get { if (_forwarder == null) _forwarder = GetComponent<WSServerForwarder>(); return _forwarder; } }
    WSServerForwarder _forwarder;
    public CommStats statsSumary = new CommStats(); //temporary,
    protected List<WebSocketSharp.WebSocket> wss = new List<WebSocketSharp.WebSocket>();
    protected override void Reset()
    {
        _server = GetComponent<WSServer>();
    }
    protected override void OnMessageDequeue(WSServiceBehaviour beh, WebSocketSharp.MessageEventArgs message)
    {
        Debug.Log("shoudnot be callsed");
    }

    private void OnOpenHandlerNonSynced(object sender, System.EventArgs e)
    {
        //  conntectionEvents.Enqueue(true);

        DebugFakeClient("client connected");
        //		DebugClientNonColor("Client is connected ".MakeColor(Const.connectionMessage));
    }

    private void OnCloseHandlerNonSynced(object sender, CloseEventArgs e)
    {
        // conntectionEvents.Enqueue(false);
        DebugFakeClient(("client:WebSocket closed with reason: " + e.Reason).MakeRed());
    }

    protected void OnMessageDequeueServerSid(WSServiceBehaviour beh, MessageEventArgs message)
    {
        DebugFakeClient("fake service recieving " + message.Data);
        foreach (var w in wss)
            w.SendAsync(message.RawData, null);
    }

    protected void DebugFakeClient(string s, GameObject g = null)
    { //WSServer.frameCount +
        Debug.Log("FAKEC: " + serviceName.MakeColor(new Color(0.4f, 0f, 0.7f)).Small() + " " + s.MakeColor(Const.clientUsingServiceNameMssage).Small());
    }

    public virtual void Disconnect()
    {

        foreach (var ws in wss)
        {
            ws.Close();
        }
        wss.Clear();
        // ws = new WebSocketSharp.WebSocket(websocketAddress);

    }

    // public void AddService(WSServiceBase service)
    // 	{
    // 		if (server == null)
    // 		{
    // 			server = new WebSocketServer(port);
    // 			server.Start();
    // 			DebugInAWhile("Server opened port " + port);
    // 		}
    // 		server.AddWebSocketService<WSServiceBehaviour>(service.serviceName, service.Initializer);
    // 		services.Add(service);
    // 		DebugInAWhile("  " + service.serviceName + ":started  ");
    // 	}
    void StartEmulatedService()
    {
        forwarder.AddFakeService(this);
    }
    protected List<WSFakeServiceBehavior> fakeClientHanlders = new List<WSFakeServiceBehavior>();

    void ConnecteEmulatedClient()
    {

        wss.Clear();
        int count = forwarder.config.targetAddresses.Count;

        for (int i = 0; i < count; i++)
        {
            var thisConfg = forwarder.config.targetAddresses[i];
            string thisaddr = thisConfg.url + serviceName;
            // if (!thisConfg.enable) continue;
            // string thisaddr = "ws://" + target.ipAddress + ":" + target.port + serviceName;
            // string thisaddr = "ws://127.0.0.1:4644" + serviceName;
            Debug.Log("connrecting to " + thisaddr);
            if (thisConfg.enable)
            {
                var thisws = new WebSocketSharp.WebSocket(thisaddr);
                int k = i;
                if (i == 0) // only the first one we'll listen from?
                {
                    thisws.OnOpen += OnOpenHandlerNonSynced;
                    thisws.OnOpen += (x, y) => (server as WSServerForwarder).connected[k]++;
                    thisws.OnClose += OnCloseHandlerNonSynced;
                    thisws.OnClose += (x, y) => (server as WSServerForwarder).connected[k]--;
                    thisws.OnMessage += (x, y) => OnServerMessage(x, y, k);
                    thisws.OnMessage += (x, y) => (server as WSServerForwarder).rxCount[k]++; //OnServerMessage;
                }
                thisws.Connect();
                wss.Add(thisws);
            }
        }

    }
    protected void OnServerMessage(object sender, MessageEventArgs e, int k)
    {
        Debug.Log("message server " + serviceName);
        statsSumary.AddBytesRecieved(e.Data.Length);
        (server as WSServerForwarder).rxCount[k]++;
        Debug.Log("servermsg " + k);
        if (k == (server as WSServerForwarder).pickeda || (server as WSServerForwarder).pickeda == -1)
        {
            if (forwarder.config.targetAddresses[k].enable && !(forwarder.config.targetAddresses[k].muteSends))
                foreach (var f in fakeClientHanlders)
                {
                    f.SendAsync(e.RawData, null);
                }
        }
        else
        {
            Debug.Log("ignoring not picked " + k);
        }
    }

    void OnClientMessage(MessageEventArgs e, WSFakeServiceBehavior behh)
    {
        Debug.Log("unqueued message from client  " + serviceName);
        for (int i = 0; i < wss.Count; i++)
        {
            if (i == (server as WSServerForwarder).pickedb || (server as WSServerForwarder).pickedb == -1)
            {
                if (forwarder.config.targetAddresses[i].enable && !(forwarder.config.targetAddresses[i].muteRecieves))
                    wss[i].SendAsync(e.RawData, null);
            }
            (server as WSServerForwarder).txCount[i]++;
        }

    }
    public void InitializerFake(WSFakeServiceBehavior beh)
    {
        Debug.Log("service run initialize , will assign cleinhandler " + serviceName);
        beh.onMessage += (e) => OnClientMessage(e, beh);
        // lock (clientHanlders)
        // {
        beh.stats = stats;
        beh.onOpen += () =>
        {
            Debug.Log("serviceOpen");

        };
        beh.onClose = (x) =>
        {
            Debug.Log("behclose");
            connectedClients = fakeClientHanlders.Count;
        };
        fakeClientHanlders.Add(beh);
        connectedClients = fakeClientHanlders.Count;
        ///   }
    }

    void OnDestroy()
    {
        foreach (var w in wss)
            w.Close();
        //   foreach(var x in fakeClientHanlders) x.Disconnect();
    }
    protected override void Start()
    {

        StartEmulatedService();
        ConnecteEmulatedClient();

    }

    void OnConnectedBaseClient()
    {
        if (statsSumary.printOnRecieve)
            DebugFakeClient("fake Client connected to ");

    }

    void OnDisconnectedBaseClient()
    {
        DebugFakeClient("Client Disconnected".MakeColor(Const.disconnectionMessage));
    }
    protected void OnMessageDequeueClient(MessageEventArgs message)
    {
        DebugFakeClient(" fake client mesage arriving " + message);
    }

}