using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WSServer : MonoBehaviour
{
	public int port = 4649;
	WebSocketServer server;
	static List<WSServer> serverHosts;

	public CommStats statsSumary = new CommStats(); //temporary
	protected Queue<MessageEventArgs> messageQueue = new Queue<MessageEventArgs>();
GameObject thisGameObject;
void Awake()
{
	thisGameObject=gameObject;
}
	protected void OnMessageHandlerNonSync(object sender, MessageEventArgs e)
	{
		statsSumary.AddBytesRecieved(e.Data.Length);
		messageQueue.Enqueue(e);
	}

	public void StartServer(string serviceName, int _port)
	{
		if (serverHosts == null)
			serverHosts = new List<WSServer>();
		serverHosts.Add(this);
		port = _port;
		DebugServer("Started " + name + " server @" + _port + " " + serviceName);
	}

	public void AddService(WSServiceBase service)
	{

		if (server == null)
		{
			server = new WebSocketServer(port);
			server.Start();
			DebugServer("server opened port " + port);
		}
		server.AddWebSocketService<WSServiceBehaviour>(service.serviceName, service.Initializer);
		DebugServer("started service " + service.serviceName);

	}

	public static WSServer Get()
	{
		if (serverHosts.Count > 0) return serverHosts[0];
		return ((new GameObject("WSServer")).AddComponent<WSServer>());
	}

	protected void DebugServer(string s, GameObject g = null)
	{
		//    Debug.Log("<color=" + ColorUtility.ToHtmlStringRGB(new Color(0.8f, 0.3f, 0.4f)) + ">" + s + "</color>", gameObject);
		Debug.Log(s.MakeColor(new Color(0.3f, 0.3f, 0.7f)), thisGameObject);
	}
	public static int frameCount;
	 void Update() {
		frameCount=Time.frameCount;
	}

}