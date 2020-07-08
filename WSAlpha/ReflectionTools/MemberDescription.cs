﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace Z.Reflection
{

	/// <summary>
	/// This is meant to be a way to store cached reflection results. should not refer to an instance but be reusable per type
	/// </summary>

	[System.Serializable]
	public class MemberDescription
	{
		public string baseName;
		public string setName;
		public string getName;
		public FieldType fieldType;
		public float lastValue;
		public bool lastBoolValue { get { return lastValue > 0; } set { lastValue = value?1 : 0; } }
		public string lastStringValue;
		public bool show = true;
		//	public string backingFieldName;

		// public enum AccessDirection { notSet, setOnly, getSet, autoUpdate }
		[SerializeField] bool _hasGet;
		[SerializeField] bool _hasSet;
		public bool hasField;
		public bool hasOnValidateNew;
		public bool hasGet { get { return _hasGet; } set { _hasGet = value; } }
		public bool hasSet { get { return _hasSet; } set { _hasSet = value; } }
		public Vector2 valueRange;

		public enum FieldType { Unknown, FloatField, IntField, BoolField, StringField, ClassField, TransformField, GameObjectField, Void }
		public AccessType accessType { get { return _accessType; } set { _accessType = value; accessTypeString = value.ToString(); } }
		public AccessType _accessType;
		public enum AccessType { notSet, ignore, fieldOnly, get_set, get, set, exposeInEditor }
		public string accessTypeString;
		public bool ReadValue(object o)
		{
			if (fieldType == FieldType.FloatField)
			{
				var previousVal = lastValue;
				lastValue = GetFloatValue(o);
				return (lastValue != previousVal);
			}
			if (fieldType == FieldType.IntField)
			{
				var previousVal = lastValue;
				lastValue = GetIntValue(o);
				return (lastValue != previousVal);
			}
			if (fieldType == FieldType.StringField)
			{
				var previousVal = lastStringValue;
				lastStringValue = GetStringValue(o);
				return (lastStringValue != previousVal);
			}
			if (fieldType == FieldType.BoolField)
			{
				var previousVal = lastBoolValue;
				lastBoolValue = GetBoolValue(o);
				return (lastBoolValue != previousVal);
			}
			return false;
		}

		public object GetValue(object obj)
		{
			if (accessType == AccessType.ignore)
			{
				return null;
			}
			if (accessType == AccessType.fieldOnly)
			{
				FieldInfo field = obj.GetType().GetField(baseName, ValueProxy.myBindingFlags);
				if (field == null)
				{
					FieldInfo f2 = obj.GetType().GetField("_" + baseName, ValueProxy.myBindingFlags);
					if (f2 == null)
					{
						Debug.Log("this object does not have a field called " + baseName + " nor _" + baseName);
						accessType = AccessType.ignore;
					}
					else field = f2;
				}
				if (field == null) return -1;
				return field.GetValue(obj);
			}
			if (accessType == AccessType.get || accessType == AccessType.get_set)
			{
				if (string.IsNullOrEmpty(getName))
				{
					Debug.Log("no get name");
				}
				MethodInfo methodInfo = obj.GetType().GetMethod(getName, ValueProxy.myBindingFlags);

				if (methodInfo == null)
				{
					zBench.ThrottledLog((" method ==null " + getName).Small());
					return null;
				}
				return methodInfo.Invoke(obj, null);
			}
			return null;
		}
		public bool GetBoolValue(object o)
		{
			return System.Convert.ToBoolean(GetValue(o));
		}
		public float GetFloatValue(object o)
		{
			return System.Convert.ToSingle(GetValue(o));
		}
		public string GetStringValue(object o)
		{
			return System.Convert.ToString(GetValue(o));
		}
		public float GetIntValue(object o)
		{
			return System.Convert.ToInt32(GetValue(o));
		}
		public bool CheckSet(MethodInfo methodInfo)
		{
			var paramInfo = methodInfo.GetParameters();
			// SETTER
			if (paramInfo.Length == 1) // has one parameter  
			{
				if (methodInfo.ReturnType == typeof(void)) // returns nothing
				{
					var thistypedesc = paramInfo[0].ParameterType.GetTypeDescription();
					hasSet = (thistypedesc != MemberDescription.FieldType.Unknown);
					if (hasSet)
					{
						fieldType = thistypedesc;
						if (accessType == MemberDescription.AccessType.notSet)
							accessType = MemberDescription.AccessType.set;
					}
					return true;
				}
			}
			// GETTER
			return false;
		}

		public bool CheckField(FieldInfo field)
		{
			// SETTER
			// FontUpdateTracker
#if UNITY_2019_3_OR_NEWER
			if (field.GetCustomAttribute(typeof(HideInInspector)) != null) return false;
#else
			//? 
#endif
			if (field.Name.Contains("_BackingField")) return false;
			var ty = field.FieldType.GetTypeDescription();
			if (ty == MemberDescription.FieldType.Unknown)
			{
				return false;
			}
			fieldType = ty;
			hasField = true;
			if (accessType == AccessType.notSet) accessType = AccessType.fieldOnly;
			if (string.IsNullOrEmpty(baseName)) baseName = field.Name;
			valueRange = field.GetRangeFromField();
			return true;
		}
		public bool CheckGet(MethodInfo methodInfo)
		{
			var paramInfo = methodInfo.GetParameters();
			// SETTER
			if (paramInfo.Length == 0) // can be get only
			{
				var rettype = methodInfo.ReturnType.GetTypeDescription();
				// info.hasGet = (info.fieldType != MemberDescription.FieldType.Unknown);
				hasGet = (fieldType != MemberDescription.FieldType.Unknown);

				if (hasGet)
				{
					fieldType = rettype;
					getName = methodInfo.Name;
					if (accessType == MemberDescription.AccessType.notSet)
						accessType = MemberDescription.AccessType.get;
					if (accessType == MemberDescription.AccessType.set)
						accessType = MemberDescription.AccessType.get_set;
				}
				return hasGet;
			}
			return false;
		}
		public static MemberDescription Describe(ComponentDescriptor parent, List<MemberInfo> allinfos, int index)
		{
			var thisMember = allinfos[index];
			MemberDescription newInfo = new MemberDescription();
			newInfo.setName = thisMember.Name; //assumign set
			newInfo.baseName = newInfo.setName.TryPlainMethod();
			if (thisMember.MemberType == MemberTypes.Property)
			{
				Debug.Log("properties not supported " + thisMember.Name);
				return null;
			}
			if (thisMember.MemberType == MemberTypes.Method)
			{
				var thisMethod = (thisMember as MethodInfo);
#if UNITY_2018_3_OR_NEWER
				if (thisMethod.GetCustomAttribute(typeof(ExposeMethodInEditorAttribute)) != null)
				{
					newInfo.baseName = thisMember.Name;
					newInfo.accessType = AccessType.exposeInEditor;
					newInfo.fieldType = FieldType.Void;
					return newInfo;
				}
#else
				Debug.Log("sorry need to port tthis");
#endif

				if (!newInfo.CheckSet(thisMethod))
				{
					return null;
				}

				newInfo.accessType = AccessType.set;
				string getMethodName = thisMember.Name.TryGetMethod();
				if (getMethodName != null)
				{
					var getinfo = allinfos.FindByName(getMethodName);
					if (getinfo != null)
					{
						if (newInfo.CheckGet(getinfo as MethodInfo))
						{
							allinfos.Remove(getinfo);
							return newInfo;
						}
					}
					newInfo.accessType = AccessType.get_set;
					return newInfo;
				}
			}

			if (thisMember.MemberType == MemberTypes.Field)
			{
				var field = thisMember as FieldInfo;
				if (newInfo.CheckField(field))
				{
					foreach (var otherdesc in parent.members)
					{
						if (otherdesc != null && otherdesc.baseName == field.Name || (field.Name.Length > 1 && otherdesc.baseName == field.Name.Substring(1)))
						{
							otherdesc.hasField = true;
							otherdesc.valueRange = newInfo.valueRange;
							return null;
						}
					}
				}
				else
					return null;
				if (newInfo.accessType == AccessType.fieldOnly)
				{
					// throw new NotImplementedException("nie pamietam co to mialo robic");
				}
				return newInfo;
			}
			return null;
		}
	}
}