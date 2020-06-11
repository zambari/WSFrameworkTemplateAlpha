using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
namespace Z.Reflection
{
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
//[CustomEditor(typeof(ReflectionScannerTool))]
public class ReflectionScannerToolEditor : Editor
{
	string[] componentNames;

	int selectedComponent;
	bool expandedcopmonents = true;
	SerializedProperty componentTypeName;
	bool expandedfields = true;
	void OnEnable()
	{
		componentNames = (target as Component).GetComponentTypes();
		componentTypeName = serializedObject.FindProperty("componentTypeName");
	}
	public override void OnInspectorGUI()
	{
		// GetProperties();
		serializedObject.Update();
		//	GUILayout.Toolbar(0, componentNames);
		string currentComponentType = componentTypeName.stringValue;
		expandedcopmonents = EditorGUILayout.Foldout(expandedcopmonents, "Pick Component" + (expandedcopmonents? "": currentComponentType));
		if (expandedcopmonents)
		{

			for (int i = 0; i < componentNames.Length; i++)
			{
				string thisComponentType = componentNames[i];
				bool thisSelected = thisComponentType == currentComponentType;

				bool newSel = GUILayout.Toggle(thisSelected, thisComponentType);
				if (newSel && !thisSelected)
				{
					componentTypeName.stringValue = thisComponentType;
					expandedcopmonents = false;
				}
			}
		}
		GUILayout.Space(50);
		if (!string.IsNullOrEmpty(currentComponentType))
			GetTypeInfo(currentComponentType);

		serializedObject.ApplyModifiedProperties();

		DrawDefaultInspector();

	}
	void GetTypeInfo(string currentComponentType)
	{
		Type t = TypeUtility.GetTypeByName(currentComponentType); // Type.GetType(currentComponentType);
		if (t == null)
		{
			GUILayout.Label("type not found : " + currentComponentType);
			return;
		}

	}
	void HandleMember(MemberInfo member)
	{
	//	GUILayout.Label(member.Name + " " + member.MemberType);
	}

}

#endif

}
