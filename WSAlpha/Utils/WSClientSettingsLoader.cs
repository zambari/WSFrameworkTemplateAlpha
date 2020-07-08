using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;

public class WSClientSettingsLoader : MonoBehaviour
{

	// Use this for initialization
	public WSServerForwarder.MultiServerConfig multiConfig;
	string path { get { return zPath.AppRootPath("emuClientSettings.json"); } }
	// Use this for initialization
	[ExposeMethodInEditor]
	void SaveEXample()
	{
		zPath.WriteJson(multiConfig, path, false);
	}
	public bool LoadONAWake;
	public bool applyOnAwake = true;

	void Awake()

	{
		if (LoadONAWake)
		{
			if (!zPath.Exists(path))
			{
				return;
			}
			var n = zPath.ReadJson<WSServerForwarder.MultiServerConfig>(path);
			if (n == null)
			{
				Debug.Log("loading failed");
				return;
			}
			Debug.Log("loaded");
			multiConfig = n;
		}
		if (applyOnAwake)
		{
			var server = GetComponent<WSServerForwarder>();
			if (multiConfig != null)
				server.SetTargets(multiConfig);
			Debug.Log(" sent to servers ");
		}
	}

}