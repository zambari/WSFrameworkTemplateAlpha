using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace Z.Reflection
{
	public static class TypeUtility //fromsomeowhere
	{
		public static string[] GetComponentTypes(this Component source)
		{
			var list = new List<string>();
			var comps = source.GetComponents<Component>();
			foreach (var c in comps)
				if (c != source) //exclude self
					list.Add(c.GetType().ToString());
			return list.ToArray();
		}

		public static Type GetTypeByName(string name)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type type in assembly.GetTypes())
					if (type.Name == name)
						return type;
			return null;
		}
	}

	public static class HelperReflectionExtensions1
	{

		public static Vector2 GetRangeFromField(this FieldInfo f)
		{
			var attrib = f.GetCustomAttribute(typeof(RangeAttribute)) as RangeAttribute;
			if (attrib != null)
			{
				Vector2 range = new Vector2(attrib.min, attrib.max);
				Debug.Log("found range " + range);
				return range;
			}
			return Vector2.zero;
		}
		static List<string> componentMemberNames;
		public static void RemoveMembersPresentInComponent(this List<MemberInfo> members)
		{
			if (componentMemberNames == null)
			{
				componentMemberNames = new List<string>();
				var commonMembers = typeof(MonoBehaviour).GetMembers();
				foreach (var cm in commonMembers)
				{
					string thisname = cm.Name;
					if (thisname != "enabled")
					{
						componentMemberNames.Add(thisname);
					}
				}
			}
			for (int i = members.Count - 1; i >= 0; i--)
			{
				if (componentMemberNames.Contains(members[i].Name)) members.RemoveAt(i);
			}
		}
		public static MemberDescription.FieldType GetTypeDescription(this Type typeInfo)
		{
			if (typeInfo == typeof(System.Single))
				return MemberDescription.FieldType.FloatField;
			if (typeInfo == typeof(System.Int32))
				return MemberDescription.FieldType.IntField;
			if (typeInfo == typeof(bool))
				return MemberDescription.FieldType.BoolField;
			if (typeInfo == typeof(System.String))
				return MemberDescription.FieldType.StringField;
			if (typeInfo == typeof(UnityEngine.Transform))
				return MemberDescription.FieldType.TransformField;
			if (typeInfo == typeof(UnityEngine.GameObject))
				return MemberDescription.FieldType.GameObjectField;
			return MemberDescription.FieldType.Unknown;

		}

		public static MemberInfo FindByName(this List<MemberInfo> memberInfos, string name)
		{
			foreach (var m in memberInfos)
				if (m.Name == name) return m;
			return null;
		}

		public static string TryGetMethod(this string methodName)
		{
			if (methodName.StartsWith("set_"))
				return "get_" + methodName.Substring(4);
			if (methodName.StartsWith("Set"))
				return "Get" + methodName.Substring(3);
			return null;
		}
		public static string TryPlainMethod(this string methodName)
		{
			if (methodName.StartsWith("set_"))
				return methodName.Substring(4);
			if (methodName.StartsWith("Set"))
				return methodName.Substring(3);
			return null;
		}

	}
}