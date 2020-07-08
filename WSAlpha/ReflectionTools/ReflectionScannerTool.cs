using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Z.Reflection
{
	public class ReflectionScannerTool : MonoBehaviour
	{
		public ComponentSelectorDrawer componentSelector = new ComponentSelectorDrawer();

		public ComponentDescriptor componentDescriptor;
		//public C
		//	public string componentTypeName;
		public bool refresh = false;
		string lastType;

		Component FindInstance()
		{
			if (!string.IsNullOrEmpty(componentDescriptor.typeName))
			{
				var allcmop = GetComponents<Component>();
				foreach (var c in allcmop)
				{
					if (c.GetType().ToString() == componentDescriptor.typeName)
					{
						// Debug.Log("found something of typ " + componentDescriptor.typeName);
						return c;
					}
				}
			}
			return null;
		}

		[ExposeMethodInEditor]
		void OnValidate()
		{
			if (refresh || componentDescriptor == null || lastType != componentSelector.selectedComponentType)
			{
				lastType = componentSelector.selectedComponentType;
				if (!string.IsNullOrEmpty(lastType))
					componentDescriptor = ComponentDescriptor.GetDescriptor(componentSelector.selectedComponentType);
			}
		}

		[ExposeMethodInEditor]
		void SaveSetupToDisk()
		{
			//			string s1 = componentDescriptor.GetVisibleAsJson();
			//			string s2 = JsonUtility.ToJson(componentDescriptor, true);
			componentDescriptor.SaveToDisk();
		}

		[ExposeMethodInEditor]
		public void RefreshValues()
		{

			componentDescriptor.ReadValues(FindInstance());
			Debug.Log("refreshed");
		}

	}

}