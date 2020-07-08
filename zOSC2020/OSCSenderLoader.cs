using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
public class OSCSenderLoader : ListPopulator
{

	// Use this for initialization
	public zOSCSender sender;
	[ExposeMethodInEditor]
	void SaveTargets()
	{
		Targets targets = new Targets();
		targets.targets = sender.senderModule.targets;
		zPath.WriteJson(targets, zPath.AppRootPath("targets.json"), false);

		// public static T ReadJson<T> (this string path) {
		//     var tex = path.ReadAllText ();
		//     return JsonUtility.FromJson<T> (tex);
		// }

		// public static void WriteJson<T>(this T obj, string path, bool silent = true)
	}

	[ExposeMethodInEditor]
	void LoadTargets()
	{
		if (zPath.Exists(zPath.AppRootPath("targets.json")))
		{
			Targets targets = zPath.ReadJson<Targets>(zPath.AppRootPath("targets.json"));
			sender.senderModule.targets = targets.targets;
			Debug.Log("loaded " + targets.targets.Count + " targes");;

			SetListSize(targets.targets.Count);
			for (int i = 0; i < targets.targets.Count; i++)
			{
				var thistarget = targets.targets[i];
				var thisitem = items[i];
				items[i].SetLabel(thistarget.targetAddr + ":" + thistarget.targetPort);
				var thistogle = thisitem.GetComponentInChildren<Toggle>();
				thistogle.isOn = thistarget.use;
				int k = i;
				thistogle.onValueChanged.AddListener((x) =>
				{
					sender.SetTargetEnabled(k, x);

				});
			}
		}
		else
		{
			SaveTargets();
		}

	}
	public bool loadOnAwake;

	void Awake()
	{
		if (loadOnAwake)
			LoadTargets();
	}

}