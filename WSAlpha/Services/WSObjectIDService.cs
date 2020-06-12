using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;
using Z;
using Z.Reflection;

public class WSObjectIDService : WSOSCService
{

	protected virtual void OnObjectIDTargettingMessage(WSServiceBehaviour beh, Transform targettedTransform, ulong id, OSCMessage message, string currentOSCAddress)
	{

		if (currentOSCAddress.StartsWith(Const.objectComponentsDetailsAddress))
		{
			DebugService("currentOSCAddress " + currentOSCAddress + "    " + currentOSCAddress.OSCFollowingSemgents());
			string componentname = currentOSCAddress.OSCFollowingSemgents().OSCFollowingSemgents().Substring(1);
			System.Type t = Z.Reflection.TypeUtility.GetTypeByName(componentname);
			if (t == null)
			{
				DebugService("type " + componentname + " was not found ");
				return;
			}

			Component component = targettedTransform.GetComponent(componentname);
			if (component == null)
			{
				DebugService("compnent not found " + componentname);
			}
			else
				ReportComponentDetails(beh, targettedTransform, component, id);
		}

		if (currentOSCAddress == Const.objectComponentsAddress)
		{
			ReportComponents(beh, targettedTransform, id);
			return;
		}
		if (currentOSCAddress == Const.objectPosGlobal)
			ReportTRS(beh, targettedTransform, id, false);
		else
		if (currentOSCAddress == Const.objectPosLocal)
			ReportTRS(beh, targettedTransform, id, true);
		else
		if (currentOSCAddress == Const.objectPosRot)
			ReportTRS(beh, targettedTransform, id, true, true);
		else
		if (currentOSCAddress == Const.objectPosRotScale)
			ReportTRS(beh, targettedTransform, id, true, true, true);
	}
	protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
	{
		DebugService(message.BinaryData.ByteArrayToString(0, 0, '-'));
		if (!message.AssertTypeTag(0, 'u'))
		{
			DebugService("		first param is not ulong, aborintg ");
			return;
		}
		string fingerprint = message.GetULong(0).ToFingerprintString();
		string addres = message.Address;
		int objectcidindex = addres.IndexOf(Const.objectIDKeywordAddress);
		if (objectcidindex < 0)
		{
			DebugService("		no objectidd ");
			return;
		}
		string restOfCommandd = addres.OSCFollowingSemgents();
		Debug.Log("		rest of comand " + restOfCommandd + "  vs " + addres.OSCFollowingSemgents());

		System.UInt64 oid = message.GetULong(0);
		Transform thisobj = ObjectID.FindTransform(oid);
		if (thisobj == null)
		{
			Debug.Log("not found " + oid.ToFingerprintString());
			ReportNotFound(beh, oid);
			return;
		}

		OnObjectIDTargettingMessage(beh, thisobj, oid, message, restOfCommandd);

	}
	void ReportTRS(WSServiceBehaviour beh, Transform thisobj, System.UInt64 oid, bool useLocal = true, bool useRotation = false, bool useScale = false)
	{
		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + (useScale? Const.objectPosRotScale: (useRotation? Const.objectPosRot : useLocal? Const.objectPosLocal : Const.objectPosGlobal)));
		Vector3 pos = useLocal?thisobj.localPosition : thisobj.position;

		message.Append(pos.x);
		message.Append(pos.y);
		message.Append(pos.z);
		if (useRotation)
		{
			Quaternion rot = useLocal? thisobj.localRotation : thisobj.rotation;
			message.Append(rot.x);
			message.Append(rot.y);
			message.Append(rot.z);
			message.Append(rot.w);

		}
		if (useScale)
		{
			Vector3 localScale = thisobj.localScale;
			message.Append(localScale.x);
			message.Append(localScale.y);
			message.Append(localScale.z);
		}
		beh.Send(message);
	}

	void ReportComponentDetails(WSServiceBehaviour beh, Transform thisobj, Component c, System.UInt64 oid)
	{
		DebugService("reporting component detail");
		if (c == null)
		{
			Debug.Log("invalid compnnet refernee");
		}
		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + Const.objectComponentsDetailsAddress);
			ComponentDescriptor descriptor = ComponentDescriptor.GetDescriptor(c.GetType());
		descriptor.ReadValues(c);
		var desc=descriptor.GetVisibleAsJson(oid,c);
		message.Append(desc);
		// DebugService(" sending json  " + desc);
		beh.Send(message);
	}
	void ReportComponents(WSServiceBehaviour beh, Transform thisobj, System.UInt64 oid)
	{
		DebugService("reporting components");
		var allcomponents = thisobj.GetComponents<Component>();
		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + Const.objectComponentsAddress);
		GameObjectInfo info = new GameObjectInfo(thisobj); //=new GameObjectInfo();
		message.Append(JsonUtility.ToJson(info));
		// DebugService("sent " + JsonUtility.ToJson(info));
		beh.Send(message);
	}
	void ReportNotFound(WSServiceBehaviour beh, System.UInt64 oid)
	{
		OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + Const.oscMessageAddess);
		message.Append(oid);
		beh.Send(message);
	}
}