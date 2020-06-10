using System.Collections;
using System.Collections.Generic;
// using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
//zbr 2020

// public abstract class WSClientBase : MonoBehaviour
// {
// //   public CommStats statsSumary = new CommStats();
//     public string serviceName = "/test";

//    public CommStats statsSumary = new CommStats(); //temporary
// 	protected abstract void OnMessageDequeue(WebSocketSharp.MessageEventArgs s);
// 	protected Queue<MessageEventArgs> messageQueue = new Queue<MessageEventArgs>();

// 	Coroutine aligner;
// 	protected virtual void OnEnable()
// 	{
// 		aligner = StartCoroutine(Aligner());
// 	}

// 	[HideInInspector]
// 	protected bool timeSpreadQueueItems;
// 	IEnumerator Aligner()
// 	{
// 		while (true)
// 		{
// 			while (messageQueue.Count > 0)
// 			{
// 				OnMessageDequeue(messageQueue.Dequeue());
// 				if (timeSpreadQueueItems)
// 				{
// 					yield return null;
// 					yield return null;
// 				}
// 			}
// 			yield return null;
// 		}
// 	}

// 	protected void OnMessageHandlerNonSync(object sender, MessageEventArgs e)
// 	{
// 		statsSumary.AddBytesRecieved(e.Data.Length);
// 		messageQueue.Enqueue(e);
// 	}
// 	protected virtual void OnDisable()
// 	{
// 		StopCoroutine(aligner);
// 	}
// 	protected virtual void Update()
// 	{
// 		while (messageQueue.Count > 0)
// 			OnMessageDequeue(messageQueue.Dequeue());
// 	}

// }

public abstract class WSClientBase : MonoBehaviour
{
	public CommStats statsSumary = new CommStats(); //temporary
	protected Queue<MessageEventArgs> messageQueue = new Queue<MessageEventArgs>();
	[ReadOnly]
	public bool isConnected = false;
	public string serviceName = "/test";
	protected abstract void OnMessageDequeue(MessageEventArgs message);

	protected Queue<bool> conntectionEvents = new Queue<bool>();

	protected WebSocketSharp.WebSocket ws;
	public bool autoConnect = true;

	[SerializeField]
	protected int waitBetweenReconnectAttempts = 8;

	Coroutine connectWatchdog;
	protected WSTargetAddress target
	{
		get
		{
			if (_target == null) _target = GetComponentInParent<WSTargetAddress>();
			if (_target == null) _target = GameObject.FindObjectOfType<WSTargetAddress>();
			return _target;
		}
	}

	[SerializeField] WSTargetAddress _target;
	Coroutine measure;
	GameObject myGameObject;
	protected bool IsConnected { get { return ws != null && ws.IsConnected; } }
	protected virtual void Reset()
	{
		string possiblename = this.GetType().ToString();
		if (possiblename.StartsWith("WS"))
			possiblename = possiblename.Substring(2);
	//	if (possiblename.EndsWith("Client"))
		//	possiblename = possiblename.Substring(0, possiblename.Length - "Client".Length);

		serviceName = "/" + possiblename;
		if (_target == null) _target = GetComponentInParent<WSTargetAddress>();
		if (_target == null) _target = GameObject.FindObjectOfType<WSTargetAddress>();
		if (_target == null)
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
			}
			return "ws://" + target.ipAddress + ":" + target.port + serviceName;
		}
	}

	protected void DebugClient(string s, GameObject g = null)
	{
		Debug.Log((WSServer.frameCount + (this.GetType().ToString() + " " + s).MakeColor(0, .6f, .9f)).Small(), myGameObject);
	}

	public void Send(string s)
	{
		statsSumary.AddBytesSent(s.Length);
		if (ws != null && ws.IsConnected)
			ws.SendString(s);
		else
		{
			DebugClient("not connected");
		}
	}
	public void Send(byte[] bytes)
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
	public void SendAsync(byte[] bytes)
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

	// protected void DebugClient(string s, GameObject g = null)
	// {
	// 	//  Debug.Log("<color=" + ColorUtility.ToHtmlStringRGB(new Color(0.3f, 0.7f, 0.1f)) + ">" + s + "</color>", gameObject);
	// 	Debug.Log(s.MakeGreen());
	// }

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
	protected virtual void OnConnected()
	{
		isConnected = true;
		DebugClient("Client connected to " + websocketAddress);
		if (measure != null) StopCoroutine(measure);

		measure = StartCoroutine(statsSumary.DataRateMeasurement());
	}
	protected virtual void OnDisconnected()
	{
		isConnected = false;
		DebugClient("Client Disconnected");
		if (measure != null) StopCoroutine(measure);
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
		DebugClient("Connecting to " + websocketAddress);
		ws = new WebSocketSharp.WebSocket(websocketAddress);
		ws.OnOpen += OnOpenHandlerNonSynced;
		ws.OnClose += OnCloseHandlerNonSynced;
		ws.OnMessage += OnMessageHandlerNonSync;
		connectWatchdog = StartCoroutine(ConnectionWachdog());
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

		DebugClient("client connected");
	}

	private void OnCloseHandlerNonSynced(object sender, CloseEventArgs e)
	{
		conntectionEvents.Enqueue(false);
		DebugClient("client:WebSocket closed with reason: " + e.Reason);
	}

	protected virtual IEnumerator Start()
	{
		myGameObject = gameObject;
		yield return null;
		// #if UNITY_EDITOR
		if (autoConnect)
		{
			Connect();
		}
		// #endif
	}
	float nextStatsUpdate;
	float lastSnapthot;
	int measureinterval = 3;
	protected virtual void Update()
	{
		while (messageQueue.Count > 0)
		{
			OnMessageDequeue(messageQueue.Dequeue());
		}

		while (conntectionEvents.Count > 0)
		{
			var thisEvent = conntectionEvents.Dequeue();
			if (thisEvent)
				OnConnected();
			else
				OnDisconnected();

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