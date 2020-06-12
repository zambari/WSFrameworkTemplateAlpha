using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using zUI;
public class WSComponentPopulator : MonoBehaviour
{
	public RectTransform content;
	Dictionary<string, Transform> compoenntPanels;
	public WSObjectReferencingClient wsClient;
	// ulong displayingID;
	public Text labeltext;
	 void AddComponentField( UIPrefabHelper prefabs, MemberInstanceLink memberDescription)
	{
		
		switch(memberDescription.fieldType)
		{
			case MemberDescription.FieldType.FloatField:
			{
				var slider=prefabs.GetSlider(memberDescription.baseName);
				ValueRemote value=new ValueRemote(memberDescription);
				value.BindSlider(slider);

			}
			break;
			case MemberDescription.FieldType.BoolField:
				var toggle=prefabs.GetToggle(memberDescription.baseName);

			break;
			case MemberDescription.FieldType.StringField:
			var inputfield=prefabs.GetInputField(memberDescription.baseName);

			break;
			case MemberDescription.FieldType.IntField:

			break;
		}

	}
	public void OnComponentDetails(ComponentDescriptorWithHandles info)
	{
		Transform dest = null;
		if (compoenntPanels.TryGetValue(info.typeName, out dest))
		{
			UIPrefabHelper prefabs = UIPrefabProvider.Get(this, dest);
			for(int i=0;i<info.memberInstances.Count;i++)
			{
				AddComponentField(prefabs,info.memberInstances[i]);
			}
		}
		else
		{
			Debug.Log("infalid request?");
		}
	}
	public void OnComponentList(GameObjectInfo info)
	{
		compoenntPanels = new Dictionary<string, Transform>();
		ClearList();
		labeltext.SetText(info.name);
		var prefabs = UIPrefabProvider.Get(this, content);
		// }
		for (int i = 0; i < info.componentNames.Length; i++)
		{
			string thiscompnentname = info.componentNames[i];
			var thispanel = prefabs.GetPanel(thiscompnentname);
			compoenntPanels.Add(thiscompnentname, thispanel.transform);
			Button detailRequestButton = thispanel.GetComponentInChildren<Button>();
			//	detailRequestButton.onClick.RemoveAllListeners();
			detailRequestButton.onClick.AddListener(() => { wsClient.RequestComponentDetails(info.id, thiscompnentname); });   // uwaga bug, dalej jest tu podczepiony toggle od folda i colliduje

		}

	}
	public void OnComponentList(OSCMessage message)
	{
		compoenntPanels = new Dictionary<string, Transform>();
		ClearList();
		var prefabs = UIPrefabProvider.Get(this, content);

		string typetag = message.typeTag;
		ulong id = message.GetULong(0);
		ulong displayingID = id;

		// if (typetag[1] == 'u')
		// {
		// 	var id 

		// }
		for (int i = 1; i < typetag.Length - 1; i++)
		{
			if (message.typeTag[i + 1] == 's')
			{
				string thisName = message.GetString(i);
				var thispanel = prefabs.GetPanel(thisName);
				compoenntPanels.Add(thisName, thispanel.transform);
				Button detailRequestButton = thispanel.GetComponentInChildren<Button>();
				//	detailRequestButton.onClick.RemoveAllListeners();
				detailRequestButton.onClick.AddListener(() => { wsClient.RequestComponentDetails(id, thisName); });

			}
			else
			{
				// Debug.Log("ignoring flags ");
			}
		}

	}

	[ExposeMethodInEditor]
	protected virtual void ClearList()
	{
		if (content == null) return;
		for (int i = content.childCount - 1; i >= 0; i--)
		{
			GameObject g = content.GetChild(i).gameObject;

#if UNITY_EDITOR
			EditorApplication.delayCall += () => DestroyImmediate(g);
#else
			Destroy(g);
#endif

		}

	}
}