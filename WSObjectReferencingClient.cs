using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class WSObjectReferencingClient : WSOSCClient
{
	public ulong targetID = 7327144221104664690;
	public long targetID5 = 73271442264690;
	protected override void OnOSCMessage(OSCMessage message)
	{
		string address = message.Address;
		if (!address.StartsWith(WSObjectIDService.objectIDKeywordAddress + WSObjectIDService.objectComponentsAddress))
		{
			Debug.Log("not a component response");
			return;
		}
		ulong id = message.GetULong(0);
		for (int i = 1; i < message.typeTag.Length / 2; i++)
		{
			string componenName = message.GetString(i * 2);
			int flags = message.GetInt(i * 2 + 1);
			Debug.Log(" component " + componenName + " flags " + flags);
		}
	}
	public WSObjectIDService.TRSReportLevel tRSReportLevel;


	[ExposeMethodInEditor]
	void Request()
	{
		OSCMessage message = new OSCMessage(WSObjectIDService.objectIDKeywordAddress+WSObjectIDService.objectComponentsAddress);
		message.Append(targetID);
	//	message.Append(targetID5);
		SendAsync(message);
	}
	[ExposeMethodInEditor]
	void RequestComponents()
	{
		OSCMessage message = new OSCMessage(WSObjectIDService.objectIDKeywordAddress+WSObjectIDService.objectComponentsAddress);
		message.Append(targetID);
	//	message.Append(targetID5);
		SendAsync(message);
	}
	[ExposeMethodInEditor]
	void RequestTransform()
	{
		OSCMessage message = new OSCMessage(WSObjectIDService.objectIDKeywordAddress+WSObjectIDService.GetAddressFor(tRSReportLevel));
		message.Append(targetID);
	//	message.Append(targetID5);
		SendAsync(message);
	}
	[ExposeMethodInEditor]
	void AddressTarget()
	{
		var o = ObjectID.FindTransform(targetID);
		Debug.Log("o ==null " + o == null);
	}

}