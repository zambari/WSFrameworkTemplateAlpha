using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;
using WSFrameworkConst;
// v.01. twaks

public abstract class WSServiceBase : MonoBehaviour
{
	// public CommStats statsSumary = new CommStats(); //temporary
	[ReadOnly]
	public int connectedClientsCount;
	public WSServer server { get { if (_server == null) GetServer(); return _server; } }

	[SerializeField] WSServer _server;
	[SerializeField] protected string _serviceName = "/test";
	public CommStats stats = new CommStats();
	public string serviceName { get { return _serviceName; } set { _serviceName = value; } } // "/ping";
	public bool autoStart = true;
	Coroutine aligner;
	protected bool timeSpreadQueueItems;

	List<WSServiceBehaviour> clientHanlders = new List<WSServiceBehaviour>();
	protected abstract void OnMessageDequeue(WSServiceBehaviour beh, WebSocketSharp.MessageEventArgs message);
	protected virtual void OnErrror(WSServiceBehaviour behaviour, WebSocketSharp.ErrorEventArgs e)
	{
		DebugService("WSServiceBehaviour Got error " + e.Message);
	}
	protected virtual void OnClose(WSServiceBehaviour behaviour, WebSocketSharp.CloseEventArgs e)
	{
		if (clientHanlders.Contains(behaviour))
			clientHanlders.Remove(behaviour);
		connectedClientsCount = clientHanlders.Count;
		DebugService("WSServiceBehaviour closed with reason " + e.Reason + "  connectedClientsCount= " + connectedClientsCount);
	}
	GameObject myGameObject;

	protected void BroacdcastString(string s)
	{
		foreach (var c in clientHanlders)
		{
			c.SendString(s);
			stats.AddBytesSent(s.Length);
		}
		if (clientHanlders.Count == 0)
			DebugService("no clients connected");
		else
			DebugService("Broadcast '" + s + "' (" + s.Length + " bytes) to " + clientHanlders.Count + " clients");
	}
	protected void BroacdcastBytes(byte[] s)
	{
		foreach (var c in clientHanlders)
		{
			c.SendAsync(s, null);
			stats.AddBytesSent(s.Length);
		}
		if (clientHanlders.Count == 0)
			DebugService("no clients connected");
		//	else
		//		DebugService("Broadcast " + s.Length + "bytes to " + clientHanlders.Count + " clients");
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
		lock(clientHanlders)
		{
			beh.stats = stats;
			clientHanlders.Add(beh);
			connectedClientsCount = clientHanlders.Count;
		}
	}

	protected void DebugService(string s)
	{
		Debug.Log("S "+serviceName.MakeColor(Const.serviceNameColor) .Small()+ " " + s.MakeColor(Const.serviceMessageColor), myGameObject);
	}

	protected virtual void OnEnable()
	{
		aligner = StartCoroutine(Aligner());
	}

	protected virtual void OnDisable()
	{
		StopCoroutine(aligner);
	}

	[HideInInspector]
	IEnumerator Aligner()
	{
		while (true)
		{
			//lock(behaviours)
			{
				foreach (var thisClient in clientHanlders)
				{
					while (thisClient.messageQueue.Count > 0)
					{
						//	DebugService("we have " + thisClient.messageQueue.Count + " messages");
						MessageEventArgs msg;
						lock(thisClient.messageQueue)
						{
							msg = thisClient.messageQueue.Dequeue();
						}
						OnMessageDequeue(thisClient, msg);
						if (timeSpreadQueueItems)
							yield return null;
					}
					while (thisClient.closedQueue.Count > 0)
					{
						CloseEventArgs close;
						lock(thisClient.closedQueue)
						{
							close = thisClient.closedQueue.Dequeue();
						}
						if (close == null)
						{
							OnOpen(thisClient);
						}
						else
						{
							OnClose(thisClient, close);
						}
					}
					while (thisClient.errorQueue.Count > 0)
					{
						ErrorEventArgs err;
						lock(thisClient.errorQueue)
						{
							err = thisClient.errorQueue.Dequeue();
						}
						OnErrror(thisClient, err);
					}

					yield return null;
				}
				yield return null;
			}
		}

	}

}