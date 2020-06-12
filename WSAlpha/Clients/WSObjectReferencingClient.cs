﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;
using Z.Reflection;

public class WSObjectReferencingClient : WSOSCClient
{
	public ulong targetID = 7327144221104664690;
	public TRSReportLevel tRSReportLevel;
	public WSComponentPopulator componentHandler;
	protected override void OnOSCMessage(OSCMessage message)
	{
		string address = message.Address;
		DebugClient("got message " + address + " " + message.typeTag);
		if (message.typeTag.Length > 0) {}
		//		ulong oid = message.GetULong(0);
		//	string id = oid.ToFingerprintString();

		if (address.StartsWith(Const.objectIDKeywordAddress + Const.invalid))
		{
			DebugClient("id not found ");
		}

		if (address.StartsWith(Const.objectIDKeywordAddress + Const.objectComponentsAddress))
		{
			GameObjectInfo info = JsonUtility.FromJson<GameObjectInfo>(message.GetString(0));
			if (info == null)
			{
				Debug.Log(" nofinf");
			}
			else
			{
				componentHandler.OnComponentList(info);
			}
			return;
		}

		if (address.StartsWith(Const.objectIDKeywordAddress + Const.objectComponentsDetailsAddress))
		{
			DebugClient("got message detaols " + message.GetString(0));
			var descriptor = JsonUtility.FromJson<ComponentDescriptorWithHandles>(message.GetString(0));
			if (descriptor != null)
				componentHandler.OnComponentDetails(descriptor);
		}
		// for (int i = 1; i < message.typeTag.Length / 2; i++)
		// {
		// 	string componenName = message.GetString(i * 2);
		// 	int flags = message.GetInt(i * 2 + 1);
		// 	// Debug.Log(" component " + componenName + " flags " + flags);
		// }
	}
	public void RequestComponentDetails(ulong id, string componentName)
	{

		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + Const.objectComponentsDetailsAddress + "/" + componentName);
		message.Append(id);
		SendAsync(message);
	}

	public void OnNodeClicked(TransformNodeInfo node)
	{
		RequestByObjectID(node.objectId);
	}

	[ExposeMethodInEditor]
	void Request()
	{
		RequestByObjectID(targetID);

	}
	void RequestByObjectID(ulong id)
	{
		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + Const.objectComponentsAddress);
		message.Append(id);
		SendAsync(message);
	}

	[ExposeMethodInEditor]
	void RequestComponents()
	{
		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + Const.objectComponentsAddress);
		message.Append(targetID);
		//	message.Append(targetID5);
		SendAsync(message);
	}

	[ExposeMethodInEditor]
	void RequestTransform()
	{
		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + Const.GetAddressFor(tRSReportLevel));
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