using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace Z.Reflection
{
	[System.Serializable]
	public class MemberInstanceLink : MemberDescription
	{
		public ulong instanceReference;
		MethodInfo methodInfoGet;
		MethodInfo methodInfoSet;
		FieldInfo fieldInfo;
		MethodInfo methodInfoOnValidate;
		object[] valueArray = new object[1]; // to avoid array creation wich each call
		public object GetObject(object obj)
		{
			switch (accessType)
			{
				case AccessType.fieldOnly:
					if (fieldInfo == null)
					{
						fieldInfo = obj.GetType().GetField(baseName);
						if (fieldInfo == null)
							fieldInfo = obj.GetType().GetField("_" + baseName);
					}
					if (fieldInfo == null)
					{
						Debug.Log(getName + " field info not found");
					}
					else
						return fieldInfo.GetValue(obj);
					break;
				case AccessType.get:
				case AccessType.get_set:
					if (methodInfoGet == null)
						methodInfoGet = obj.GetType().GetMethod(getName);
					Component component = (obj as Component);
					return methodInfoGet.Invoke(component, null);
			}
			return null;
		}
		public void SetObject(object obj, object value)
		{
			switch (accessType)
			{
				case AccessType.fieldOnly:
					if (fieldInfo == null)
						fieldInfo = obj.GetType().GetField(setName);
					if (fieldInfo == null)
					{
						Debug.Log(" field info not found");
					}
					else
					{
						fieldInfo.SetValue(obj, value);
						if (hasOnValidateNew)
						{
							if (methodInfoOnValidate == null)
								methodInfoOnValidate = obj.GetType().GetMethod("OnValidate");
							methodInfoOnValidate.Invoke(obj, null);
						}
					}
					break;
				case AccessType.set:
				case AccessType.get_set:
					if (methodInfoSet == null)
					{
						methodInfoSet = obj.GetType().GetMethod(setName);
						Debug.Log("createdmehtodinfo? " + (methodInfoSet != null));
					}
					Component component = (obj as Component);
					valueArray[0] = value;
					methodInfoSet.Invoke(component, valueArray);
					Debug.Log("set on " + component.name, component);
					break;
				default:
					Debug.Log("unknown accestype " + accessType);
					break;
			}
		}
		public float GetFloat(object obj)
		{
			lastValue = System.Convert.ToSingle(GetObject(obj));
			return lastValue;
		}
		public void SetFloat(object obj, float f)
		{
			SetObject(obj, (object)f);
		}
		public MemberInstanceLink(MemberDescription description, ulong id) // copying constructor
		{
			this.baseName = description.baseName;
			setName = description.setName;
			getName = description.getName;
			fieldType = description.fieldType;
			lastValue = description.lastValue;
			lastStringValue = description.lastStringValue;
			show = description.show;
			hasField = description.hasField;
			accessType = description.accessType;
			valueRange = description.valueRange;
			instanceReference = id;
		}
	}
}