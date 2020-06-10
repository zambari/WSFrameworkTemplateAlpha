using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class zOSCListener : MonoBehaviour, IRecieveOSC
{
	public zOSCListenerModule oscListener = new zOSCListenerModule();
	public string listenportfunny;
	public int listenportdecoded;
	protected List<IConsumeOSC> consumers = new List<IConsumeOSC>();
	public void Register(IConsumeOSC consumer)
	{
		consumers.Add(consumer);
	}
	protected virtual void OnValidate()
	{
		oscListener.OnValidate(this);
		listenportdecoded = 0;
		for (int i = 0; i < listenportfunny.Length; i++)
		{
			listenportdecoded = listenportdecoded <<1;
			listenportdecoded += (int) listenportfunny[i];
		}
		// listenportfunny = zOSCNameAlpha.PortToName(oscListener.listenPort);
	}

	void BroadcastPacketToConsumers(OSCMessage message)
	{
		foreach (var oscConsumer in consumers)
			oscConsumer.OnMessage(message);
		Debug.Log(name + " reciever got packet " + message.Address + " type " + message.typeTag + " (consumer count " + consumers.Count + ")");
	}
	public void Unregister(IConsumeOSC consumer)
	{
		consumers.Remove(consumer);
	}
	protected virtual void Start()
	{
		if (oscListener.autoStart)
		{
			if (oscListener.RestartLocalServer())
			{
				Debug.Log("additional osc lisener started at " + oscListener.listenPort);
			}
		}
		oscListener.useCustomReciever = true;
		oscListener.onPacket -= BroadcastPacketToConsumers;
		oscListener.onPacket += BroadcastPacketToConsumers;
	}

	// protected virtual void OnValidate()
	// {
	// 	if (name == "GameObject") name = "zOSCAdditionalListener";
	// 	oscListener.loggingInfo.name = name;
	// }
	void Update()
	{
		oscListener.Update();
	}
	void OnDestroy()
	{
		if (oscListener != null && oscListener.server != null)
			oscListener.server.Close();
	}

}