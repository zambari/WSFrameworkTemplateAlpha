using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;

[ExecuteInEditMode]
public class UIPrefabProvider : MonoBehaviour
{
	// Use this for initialization

	public static UIPrefabProvider instance;
	public List<PrefabSet> prefabSets;
	public static UIPrefabHelper Get(MonoBehaviour src, Transform content = null)
	{
		if (content == null)
		{
			content = src.transform;
		}
		if (instance == null)
		{
			Debug.Log("no prefabprovide instance");
		}
		var newHelper = new UIPrefabHelper() { source = src, root = content, provider = instance };
		return newHelper;

	}
	public static UIPrefabHelper GetPrefabs(MonoBehaviour src, Transform content = null)
	{
		return Get(src, content);

	}
	PrefabSet mainSet;
	void Reset()
	{
		prefabSets = new List<PrefabSet>();
		var found = Resources.FindObjectsOfTypeAll<PrefabSet>() as PrefabSet[];
		prefabSets = new List<PrefabSet>(found);
		Debug.Log(" found " + found.Length);

	}
	public GameObject GetPrefab(string typeName, string label, Transform target, string variantName = null)
	{
		if (mainSet == null && prefabSets.Count > 0) mainSet = prefabSets[0];
		if (mainSet == null)
		{
			Debug.Log("no main set");
			return null;
		}
		var go = mainSet.Get(typeName);
		if (go != null)
		{
			go = Instantiate(go, target);
			go.transform.localScale = Vector3.one;
			Text t = go.GetComponentInChildren<Text>();
			if (t != null) t.text = label;
			if (!string.IsNullOrEmpty(variantName)) variantName = " (" + variantName + ")";
			go.name = ">" + typeName + ":" + label + variantName;
		}
		return go;
	}
	void SetInstance()
	{
		if (instance != null && instance != this)
		{
			Debug.Log("TWO INSTANCES of UIREFABS", gameObject);
			return;
		}
		instance = this;
	}
	void Awake()
	{
		SetInstance();

	}
	private void OnEnable()
	{
		SetInstance();
	}

}