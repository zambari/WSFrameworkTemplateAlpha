using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Z.Reflection
{
	[System.Serializable]
	public class ComponentSelectorDrawer
	{
		public string selectedComponentType;
	}
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(ComponentSelectorDrawer))]
	public class ComponentSelectorDrawerEditor : PropertyDrawer
	{
		string[] componentNames;
		bool expandedcopmonents = true;
		Component lastTarget;
		void GetOtherComponents(SerializedProperty property)
		{

			var thisTarget = property.serializedObject.targetObject as Component;
			if (thisTarget != lastTarget || componentNames == null)
			{
				componentNames = (thisTarget as Component).GetComponentTypes();
			}
			lastTarget = thisTarget;
		}
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//position.y+=10;
			Rect line = new Rect(position);
			line.height = lineheigt;
			line.y = position.y + lineheigt / 2; // - lineheigt;
			//	GUI.Label(line, label);
			var selectedComponentType = property.FindPropertyRelative("selectedComponentType");
			//	line.y += lineheigt;

			string currentComponentType = selectedComponentType.stringValue;
			GetOtherComponents(property);
			// string clabel = "Pick Component : " + (expandedcopmonents? "": currentComponentType);
			if (string.IsNullOrEmpty(currentComponentType)) currentComponentType = "[None]";
			string clabel = "Pick Component : " + currentComponentType;
			expandedcopmonents = EditorGUI.Foldout(line, expandedcopmonents, clabel);
			if (expandedcopmonents)
			{
				for (int i = 0; i < componentNames.Length; i++)
				{
					line.y += lineheigt;
					string thisComponentType = componentNames[i];
					bool thisSelected = thisComponentType == currentComponentType;

					bool newSel = GUI.Toggle(line, thisSelected, thisComponentType);
					if (newSel && !thisSelected)
					{
						selectedComponentType.stringValue = thisComponentType;
					//expandedcopmonents = false;
						property.serializedObject.ApplyModifiedProperties();
					}
				}
			}
			//	GUILayout.Space(50);
			// if (!string.IsNullOrEmpty(currentComponentType))
			// 	GetTypeInfo(currentComponentType);

			// serializedObject.ApplyModifiedProperties();

			// DrawDefaultInspector();
		}
		int lineheigt = 20;
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!expandedcopmonents) return 2 * lineheigt;
			GetOtherComponents(property);
			return (componentNames.Length + 2) * lineheigt;
		}

	}

}
#endif