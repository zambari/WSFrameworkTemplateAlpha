using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;

namespace zUI
{
	[CreateAssetMenu]
	public class PrefabSet : ScriptableObject
	{
		public string varianName = "None";
		public enum BasicObjectTypes { Label, Button, Slider, Toggle, InputField, Subpanel, Horizontal, DropDown, }
		private void Reset()
		{
			namedGameObjects = new List<NamedGameObject>();
			var types = System.Enum.GetNames(typeof(BasicObjectTypes));
			for (int i = 0; i < types.Length; i++)
			{
				namedGameObjects.Add(new NamedGameObject() { name = types[i] });
			}
		}
		public List<NamedGameObject> namedGameObjects = new List<NamedGameObject>();
		public GameObject Get(string name)
		{
			for (int i = 0; i < namedGameObjects.Count; i++)
				if (namedGameObjects[i].name == name)
				{
					return namedGameObjects[i].value;
				}
			return null;
		}

		[ExposeMethodInEditor]
		void TEsT()
		{
			//	Reset();	
			var types = System.Enum.GetNames(typeof(BasicObjectTypes));
			for (int i = 0; i < types.Length; i++)
			{
				namedGameObjects[i].name = types[i];
			}
		}
	}
}