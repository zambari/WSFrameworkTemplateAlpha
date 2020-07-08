using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;

[System.Serializable]
public class WSSSErverSettnigs
{
	public string serverName = "unnamed";
	public int serverPort = 4644;

}
public class WSServerSettingsLoader : MonoBehaviour
{
	public WSSSErverSettnigs settnigs;
	string path { get { return zPath.AppRootPath("serverSettings.json"); } }
	// Use this for initialization
	[ExposeMethodInEditor]
	void SaveEXample()
	{
		zPath.WriteJson(settnigs, path, false);
	}
	public bool autoLoad=true;
		void Awake()
	
	{
		if (!autoLoad) return;

		var n = zPath.ReadJson<WSSSErverSettnigs>(path);
		if (n == null)
		{
			Debug.Log("loading failed");
			return;
		}
		Debug.Log("loaded");
		settnigs = n;
		var server = GetComponent<WSServer>();
		server.port = n.serverPort;
	}

}