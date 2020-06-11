using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;
public class WSObjectIDService : WSOSCService
{

	// protected override void  Reset() {
	// 	base. Reset();
	// 	_serviceName
	// }
	public enum TRSReportLevel { position, localPosition, positionAndRotation, localScale, localPositionAndRotation, localPositionRotationAndScale }
	public static string GetAddressFor(TRSReportLevel level)
	{
		switch (level)
		{
			case TRSReportLevel.position:
				return objectPosGlobal;
			case TRSReportLevel.localPosition:
				return objectPosLocal;
			case TRSReportLevel.positionAndRotation:
				return objectPosRot;
			case TRSReportLevel.localPositionAndRotation:
				return objectPosLocal;
			case TRSReportLevel.localScale:
				return localScale;
			default:
				return "unknwon trs";
		}
	}
	static readonly public string objectIDKeywordAddress = "/id";
	static readonly public string objectComponentsAddress = "/components";
	static readonly public string objectPosRotScale = "/trs";
	static readonly public string objectPosRot = "/posRot";
	static readonly public string localScale = "/localScale";
	static readonly public string objectPosLocal = "/posLocal";
	static readonly public string objectPosGlobal = "/posGlobal";
	static readonly public string invalid = "/invalid";
	protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
	{
		DebugService(message.BinaryData.ByteArrayToString(0, 0, '-'));
		if (!message.AssertTypeTag(0, 'u'))
		{
		DebugService("		first param is not ulong, aborintg ");
			return;
		}
		string addres = message.Address;
		int objectcidindex = addres.IndexOf(objectIDKeywordAddress);
		if (objectcidindex < 0)
		{
		DebugService("		no objectidd ");
			return;
		}
		string restOfCommandd = addres.Substring(objectcidindex + 3);
		Debug.Log("		rest of comand " + restOfCommandd);

		System.UInt64 oid = message.GetULong(0);
		Transform thisobj = ObjectID.FindTransform(oid);
		if (thisobj == null)
		{
			ReportNotFound(beh, oid);
			return;
		}
	

		if (restOfCommandd == objectComponentsAddress)
		{
			ReportComponents(beh, thisobj, oid);
			return;
		}

		if (restOfCommandd == objectPosGlobal)
			ReportTRS(beh, thisobj, oid, false);
		else
		if (restOfCommandd == objectPosLocal)
			ReportTRS(beh, thisobj, oid, true);
		else
		if (restOfCommandd == objectPosRot)
			ReportTRS(beh, thisobj, oid, true, true);
		else
		if (restOfCommandd == objectPosRotScale)
			ReportTRS(beh, thisobj, oid, true, true, true);

		// if (addres.Contains(objectIDKeywordAddress))
		// {

		// }
		// int nexSlash = addres.IndexOf('/', 1);
		// if (nexSlash > 0)
		// {

		// }
	}
	void ReportTRS(WSServiceBehaviour beh, Transform thisobj, System.UInt64 oid, bool useLocal = true, bool useRotation = false, bool useScale = false)
	{
		OSCMessage message = new OSCMessage(objectIDKeywordAddress + (useScale?objectPosRotScale: (useRotation?objectPosRot : useLocal? objectPosLocal : objectPosGlobal)));
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
	void ReportComponents(WSServiceBehaviour beh, Transform thisobj, System.UInt64 oid)
	{
		var allcomponents = thisobj.GetComponents<Component>();
		OSCMessage message = new OSCMessage(objectIDKeywordAddress + objectComponentsAddress);
		message.Append(oid);
		message.Append(thisobj.name);
		for (int i = 0; i < allcomponents.Length; i++)
		{
			var thiscomponent = allcomponents[i];
			message.Append(thiscomponent.GetType().ToString());
			int flags = 0;
			var mono = thiscomponent as MonoBehaviour;
			if (mono != null)
				flags = mono.enabled?1 : 2;
			message.Append(flags);
		}
		beh.Send(message);
	}
	void ReportNotFound(WSServiceBehaviour beh, System.UInt64 oid)
	{
		OSCMessage message = new OSCMessage(objectIDKeywordAddress + invalid);
		message.Append(oid);
		beh.Send(message);
	}
}