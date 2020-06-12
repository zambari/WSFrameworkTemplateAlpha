using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WSFrameworkConst
{
	public enum TRSReportLevel { position, localPosition, positionAndRotation, localScale, localPositionAndRotation, localPositionRotationAndScale }

	public static class Const
	{
		static readonly public string invalid = "/invalid";
		static readonly public string oscMessageAddess = "/message";
		static readonly public string objectIDKeywordAddress = "/id";
		static readonly public string objectComponentsAddress = "/components";
		static readonly public string objectComponentsDetailsAddress = "/componentInfo";
		static readonly public string valueOSC = "/value";
		static readonly public string objectPosRotScale = "/trs";
		static readonly public string objectPosRot = "/posRot";
		static readonly public string localScale = "/localScale";
		static readonly public string objectPosLocal = "/posLocal";
		static readonly public string objectPosGlobal = "/posGlobal";
		public static readonly Color serviceNameColor = new Color(.2f, .6f, .8f);
		public static readonly Color serviceMessageColor = new Color(.5f, .2f, .9f);
		public static readonly Color clientUsingServiceNameColor = new Color(.2f, .5f, .1f);
		public static readonly Color clientUsingServiceNameMssage = new Color(.3f, .8f, .5f);
		public static readonly Color connectionMessage = new Color(0, .9f, .2f);
		public static readonly Color disconnectionMessage = new Color(.6f, .1f, .1f);
		public static string GetAddressFor(this TRSReportLevel level)
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
	public static string GetPossibleServiceName(this object obj)
	{
		string possiblename = obj.GetType().ToString();
		if (possiblename.StartsWith("WS"))
			possiblename = possiblename.Substring(2);
		if (possiblename.EndsWith("Service"))
			possiblename = possiblename.Substring(0, possiblename.Length - "Service".Length);
		if (possiblename.EndsWith("Client"))
			possiblename = possiblename.Substring(0, possiblename.Length - "Client".Length);
		possiblename = possiblename.ToLower();
		return "/" + possiblename;;
	}
	}
}