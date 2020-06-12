﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace Z.Reflection
{

	[System.Serializable]
	public class ComponentDescriptorWithHandles : ComponentDescriptorBase
	{
		public ulong objectID;

public ComponentDescriptorWithHandles(ulong id)
{
	objectID=id;
}
		public List<MemberInstanceLink> memberInstances;
	}

	[System.Serializable]
	public class ComponentDescriptor : ComponentDescriptorBase
	{

		public List<MemberDescription> members;
		public Component referenceObject;
		static string componentPath = "ComponentInfos";

		// public string GetVisibleAsJson(GameObject referenceObject)
		// {
		// 	return GetVisibleAsJson(referenceObject.GetID());
		// }
		public string GetVisibleAsJson(ulong id,Component c)
		{
			var newDescriptor = new ComponentDescriptorWithHandles(id);
			ulong baseId = id.Compact();
			newDescriptor.typeName = typeName;
			newDescriptor.memberInstances = new List<MemberInstanceLink>();

			for (int i=0;i<members.Count;i++)
			{
				if (members[i].show)	
				{
					ulong thisid= baseId.MergeWithInt(ObjectID.incremental);// ValueProxy.MakeValueUnique(baseId);
					
					var thisInstanceMember= new MemberInstanceLink(members[i],thisid);
					var thisProxy=new ValueProxy(thisInstanceMember,c);
					thisProxy.name = members[i].baseName; // gameobject names
					ValueProxy.RegisterProxy(thisid,thisProxy);
					thisInstanceMember.hasOnValidateNew=hasOnValidate;
					newDescriptor.memberInstances.Add(thisInstanceMember);
				}
			}
			newDescriptor.hasOnValidate = hasOnValidate;
			
			return JsonUtility.ToJson(newDescriptor   //line breaks #iffed
#if UNITY_EDITOR
				, true //faster in builds
#endif
			);
		}
		private ComponentDescriptor() {}
		public static ComponentDescriptor GetDescriptor(string t)
		{

			return GetDescriptor(TypeUtility.GetTypeByName(t));
		}
		static Dictionary<Type, ComponentDescriptor> descriptionDict;
		public static ComponentDescriptor GetDescriptor(Type t)
		{
			if (descriptionDict == null) descriptionDict = new Dictionary<Type, ComponentDescriptor>();
			ComponentDescriptor descriptor = null;
			if (descriptionDict.TryGetValue(t, out descriptor))
			{
				Debug.Log("found component desciptor for typ "+t.ToString()+" in dictionary");
				return descriptor;
			}
			else
			{
				Debug.Log("not found in dict "+t);
			}
			var loadpath = GetSaveLoadPath(t.ToString());
			if (System.IO.File.Exists(loadpath))
			{
				Debug.Log("file exitsts " + loadpath);
				var file = System.IO.File.ReadAllText(loadpath);
				var loading = JsonUtility.FromJson<ComponentDescriptor>(file);
				if (loading != null)
				{
					Debug.Log("loaded");
					descriptionDict.Add(t, loading);
					return loading;
				}
			}
			else
			{
				Debug.Log("does not exis creating desc for "+t);
			}
			descriptor = new ComponentDescriptor();
			descriptor.Scan(t);
			descriptionDict.Add(t, descriptor);
			Debug.Log("scan for "+t+"shows " + descriptor.members.Count + " members");
			return descriptor;
		}
		public void ReadValues(Component src)
		{
			if (src == null)
				return;
			referenceObject = src;
			if (src.GetType().ToString() != typeName)
			{
				Debug.Log("type mismatch " + typeName + " vs " + src.GetType());
			}
			for (int i = 0; i < members.Count; i++)
				if (members[i].show)
					members[i].ReadValue(src);
		}
		public void PlaceInDict()
		{
			if (string.IsNullOrEmpty(typeName)) return;
			if (descriptionDict == null) descriptionDict = new Dictionary<Type, ComponentDescriptor>();
			if (descriptionDict.ContainsKey(type))
			{
				if (descriptionDict[type] == this)
					Debug.Log("no need to update");
				else
					Debug.Log("updated");
				descriptionDict[type] = this;
			}
			else
				descriptionDict.Add(type, this);

		}
		// public ComponentDescriptor(Type t)
		// {
		// 	Scan(t);
		// }
		// public ComponentDescriptor(string t)
		// {
		// 	Scan(t);
		// }
		public void Scan(string className)
		{
			if (string.IsNullOrEmpty(className)) return;
			typeName = className;
			Scan(TypeUtility.GetTypeByName(typeName));
		}
		public void Scan(Type t)
		{
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			typeName = t.ToString();
			members = new List<MemberDescription>();
			List<MemberInfo> allinfos = new List<MemberInfo>(t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
			allinfos.RemoveMembersPresentInComponent();
			for (int i = 0; i < allinfos.Count; i++)
			{
				var m = (allinfos[i] as MethodInfo);
				if (m != null && m.Name.Contains("OnValidate"))
				{
					Debug.Log("valid");
				}
				if (m != null && m.Name == "OnValidate" && m.GetParameters().Length == 0)
					hasOnValidate = true;
				else
				{
					var description = MemberDescription.Describe(this, allinfos, i);
					if (description != null)
						members.Add(description);
				}
			}
			sw.Stop();
			Debug.Log("scan finihed elasped " + sw.ElapsedMilliseconds);

		}
		static string GetSaveLoadPath(string typeName)
		{

			string path = Application.streamingAssetsPath;
			if (!System.IO.Directory.Exists(path))
				System.IO.Directory.CreateDirectory(path);
			path = System.IO.Path.Combine(path, componentPath);
			if (!System.IO.Directory.Exists(path))
				System.IO.Directory.CreateDirectory(path);
			path = System.IO.Path.Combine(path, typeName + ".json");
			return path;
		}
		public void SaveToDisk()
		{
			if (string.IsNullOrEmpty(typeName) || members == null || members.Count == 0)
			{
				Debug.Log("invalid state for saving");
				return;
			}
			string asJson = JsonUtility.ToJson(this, true);
			System.IO.File.WriteAllText(GetSaveLoadPath(typeName), asJson);
			Debug.Log("saved as " + GetSaveLoadPath(typeName));

		}
	}

	[System.Serializable]
	public class ComponentDescriptorBase
	{
		public string typeName;
		Type _type;
		public Type type
		{
			get
			{
				if (string.IsNullOrEmpty(typeName)) return default(Type);
				if (_type == null) _type = TypeUtility.GetTypeByName(typeName);
				return _type;
			}
		}
		public bool hasOnValidate;
	}
}