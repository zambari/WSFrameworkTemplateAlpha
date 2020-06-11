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
#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(MemberDescription))]
	public class MemberDescriptionInspector : PropertyDrawer
	{
		string[] enumAccessType;
		string[] enumTypes;
		string[] onOff = new string[] { "0", "I" };
		SerializedProperty name;
		SerializedProperty hasGet;
		SerializedProperty hasSet;
		SerializedProperty valueRange;
		SerializedProperty fieldType;
		SerializedProperty accessType;
		SerializedProperty show;
		GUIStyle fadedLabel;
		bool wasRefreshed;
		static float refreshTime;
		void Refresh(SerializedProperty property)
		{
			var x = property.serializedObject.targetObject;
			if (x.GetType() == (typeof(ReflectionScannerTool)))
				(x as ReflectionScannerTool).RefreshValues();
			wasRefreshed = true;
			refreshTime = Time.time;
		}
		void CheckEnums()
		{
			if (enumAccessType == null) enumAccessType = System.Enum.GetNames(typeof(MemberDescription.AccessType));
			if (enumTypes == null) enumTypes = System.Enum.GetNames(typeof(MemberDescription.FieldType));
			for (int i = 0; i < enumAccessType.Length; i++)

			{
				int start = enumAccessType[i].IndexOf("Field");
				if (start > 0)
					enumAccessType[i] = enumAccessType[i].Substring(0, start);
			}
		}
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//	property.serializedObject.Update();
			//	if (name == null) 
			name = property.FindPropertyRelative("baseName");
			if (fadedLabel == null)
			{
				fadedLabel = new GUIStyle(EditorStyles.miniLabel);
				Color color = fadedLabel.normal.textColor;
				color.a = 0.5f;
				fadedLabel.normal.textColor = color;
			}
			if (!wasRefreshed || Time.time - refreshTime > 2) Refresh(property);

			//	if (show == null) 
			show = property.FindPropertyRelative("show");
			CheckEnums();

			Rect upperRight = new Rect(position);
			int w = 45;
			fieldType = property.FindPropertyRelative("fieldType");

			upperRight.x = position.width - w;
			upperRight.width = w;
			upperRight.height = 17;
			int ison = show.boolValue ? 1 : 0;
			ison = GUI.Toolbar(upperRight, ison, onOff);
			bool showVal = ison == 1;
			GUI.Label(position, showVal?("[" + name.stringValue + "]") : name.stringValue, showVal?EditorStyles.largeLabel : fadedLabel);
			show.boolValue = showVal;
			upperRight.x -= 35;
			if (showVal)
			{
				Component reference = null;

				Rect content = new Rect(position);
				content.height = 20;
				content.x += 40;
				GUI.Label(content, enumTypes[fieldType.enumValueIndex]); //label
				//content.x = position.x;
				content.height = 20;
				content.y += 20;
				accessType = property.FindPropertyRelative("accessType");

				GUI.Label(content, enumAccessType[accessType.enumValueIndex], fadedLabel);
				Rect column = new Rect(content);
				Rect rangeRect = new Rect(content);
				content.y = position.y;

				content.x += 80;
				if (fieldType.enumValueIndex == (int) MemberDescription.FieldType.FloatField || fieldType.enumValueIndex == (int) MemberDescription.FieldType.IntField)
					ShowValue(content, property.FindPropertyRelative("lastValue").floatValue.ToString());
				if (fieldType.enumValueIndex == (int) MemberDescription.FieldType.BoolField)
					ShowValue(content, (property.FindPropertyRelative("lastValue").floatValue == 1 ? "True" : "False)"));
				if (fieldType.enumValueIndex == (int) MemberDescription.FieldType.StringField)
					ShowValue(content, property.FindPropertyRelative("lastStringValue").stringValue);
				column.x += 40;
				if (fieldType.enumValueIndex == (int) MemberDescription.FieldType.FloatField)
				{
					var range = property.FindPropertyRelative("valueRange");
					var rangeval = range.vector2Value;
					if (rangeval != Vector2.zero)
					{

						rangeRect.x = position.x + position.width - 130;
						rangeRect.width = 130;
						GUI.Label(rangeRect, rangeval.x.ToString("RANGE: 0.00") + " " + rangeval.x.ToString("- 0.00"));
					}

				}
				if (reference != null)
				{

				}

			}
		}
		void ShowValue(Rect content, string s)
		{
			GUI.Label(content, "value: " + s);
		}
		// public override int Get
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var show = property.FindPropertyRelative("show");
			if (!show.boolValue) return 15;
			return 20 * 2.5f;
		}
	}
#endif
}