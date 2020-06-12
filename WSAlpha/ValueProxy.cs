using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace Z.Reflection
{
	[System.Serializable]
	public class ValueProxy : MemberDescription
	{
		public string name;
		public System.Int32 memberId;
		public ulong objectIdTarget; // hold objectid of the referenc object
		public object objectReference; // holds the object iselft
		public static Dictionary<int, ValueProxy> proxyDict;
		public static List<ValueProxy> activeProxies = new List<ValueProxy>();
		private MethodInfo methodInfoGet;
		private MethodInfo methodInfoSet;
		private FieldInfo fieldInfo;
		private MethodInfo methodInfoOnValidate;
		private object lastObjectValue;
		private object[] valueArray = new object[1]; // to avoid array creation wich each call

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
		public void SetFloat(float f)
		{
			SetObject(objectReference, (object) f);
		}

		public int GetInt(object obj)
		{
			lastValue = System.Convert.ToInt32(GetObject(obj));
			return (int) lastValue;
		}

		public void SetInt(int f)
		{
			SetObject(objectReference, (object) f);
		}
		public bool GetBool(object obj)
		{
			lastBoolValue = System.Convert.ToBoolean(GetObject(obj));
			return lastBoolValue;
		}
		public void SetBool(int f)
		{
			SetObject(objectReference, (object) f);
		}
		public string GetString(object obj)
		{
			lastStringValue = System.Convert.ToString(GetObject(obj));
			return lastStringValue;
		}
		public void SetString(string f)
		{
			SetObject(objectReference, (object) f);
		}
		void CopyFromMember(MemberDescription description)
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

		}
		public ValueProxy(MemberDescription description, ulong id) // copying constructor
		{
			CopyFromMember(description);
			objectIdTarget = id;
			memberId = incremental;
		}

		public ValueProxy(MemberDescription description, object obj) // copying constructor
		{
			CopyFromMember(description);
			objectReference = obj;
			var component = (obj as Component);
			if (component != null)
			{
				objectIdTarget = component.GetObjectID();
			}
		}
		public bool HasValueChanged()
		{
			return ReadValue(objectReference);
			// switch (fieldType)
			// {
			// 	case FieldType.FloatField:

			// 		break;
			// 	case FieldType.IntField:

			// 		break;
			// 	case FieldType.BoolField:

			// 		break;
			// 	case FieldType.StringField:

			// 		break;
			// }

			// object val = GetObject(objectReference);
			// if (lastObjectValue != val)
			// {
			// 	Debug.Log(" value hasn changed " + name);
			// 	lastObjectValue = val;
			// 	return true;
			// }
			// else
			// {
			// 	Debug.Log(" value hasn't changed " + name);
			// 	return false;
			// }
		}

		// public ValueProxy(MemberInstanceLink link, object obj)
		// {
		// 	linkreference = link;
		// 	objectReference = obj;
		// }
		// public void SetFloat(float f)
		// {
		// 	Debug.Log("seting float " + f);
		// 	linkreference.SetFloat(objectReference, f);
		// }
		// public float GetFloat()
		// {
		// 	return linkreference.GetFloat(objectReference);
		// }
		static System.Int32 _incremental = 5;

		/// <summary>
		/// always returns value +1 from the previous requested (globally)
		/// </summary>
		/// <value></value>
		public static System.Int32 incremental { get { return _incremental++; } set { _incremental = value; } }
		public static void RegisterProxy(ValueProxy proxy)
		{
			// int valueid=;
			Debug.Log("registering proxy with valueid " + proxy.memberId.ToColorfulString());
			if (proxyDict == null) proxyDict = new Dictionary<int, ValueProxy>();
			proxyDict.Add(proxy.memberId, proxy);
			if (!activeProxies.Contains(proxy)) activeProxies.Add(proxy);
			Debug.Log("registered proxy for " + proxy.memberId.ToColorfulString() + " proxy " + proxy.name + "  " + "object reference is null =  " + (proxy.objectReference == null));
		}
		public static ValueProxy GetProxyFromDict(int memberid)
		{

			if (proxyDict == null)
				proxyDict = new Dictionary<int, ValueProxy>();
			ValueProxy value = null;
			if (proxyDict.TryGetValue(memberid, out value))
			{

			}
			else
			{

			}
			return value;
			// Debug.Log("registered proxy for "+valueid.ToColorfulString());
		}
	}
}