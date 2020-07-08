using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zOSCLoggable
{
	[System.Serializable]
	public class LogInfo
	{
		public bool writeToConsole = true;
		public string name = "unnamed";
		public Color color = Color.green * .7f;

	}
	//s[Header("LogInfo")]
	public LogInfo loggingInfo;

	public void Log(string s)
	{
		if (string.IsNullOrEmpty(s)) return;
		if (!loggingInfo.writeToConsole) return;
		// Debug.Log((loggingInfo.name + ":" + s).MakeColor(loggingInfo.color));
	}
	public void Log(string s, Component component)
	{
		if (string.IsNullOrEmpty(s)) return;
		if (!loggingInfo.writeToConsole) return;
		Debug.Log(s.MakeColor(loggingInfo.color), component);
	}
}