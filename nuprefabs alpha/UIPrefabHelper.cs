using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{
	public class UIPrefabHelper
	{

		// Use this for initialization
		public UIPrefabProvider provider;
		public Transform root;
		public MonoBehaviour source;

	}
	public static class UIPRefabExtensions
	{
		public static GameObject GetBasicObject(this UIPrefabHelper helper, PrefabSet.BasicObjectTypes type, string label, string variant = null)
		{
			if (helper.provider == null)
			{
				Debug.Log("this helper does not ave a provider");

			}
			return helper.provider.GetPrefab(type.ToString(), label, helper.root, variant);
		}
		public static Button GetButton(this UIPrefabHelper helper, string label, string variant = null)
		{
			return helper.GetBasicObject(PrefabSet.BasicObjectTypes.Button, label, variant).GetComponent<Button>();
		}
		public static Transform GetPanel(this UIPrefabHelper helper, string label, bool startFolded = false, string variant = null)
		{
			var panel = helper.GetBasicObject(PrefabSet.BasicObjectTypes.Subpanel, label, variant).transform;
			var fold = panel.GetComponentInChildren<SimpleFoldController>();
			if (fold != null)
			{
				fold.startFolded = startFolded;
			}
			return panel;
		}
		public static Toggle GetToggle(this UIPrefabHelper helper, string label, string variant = null)
		{
			return helper.GetBasicObject(PrefabSet.BasicObjectTypes.Toggle, label, variant).GetComponent<Toggle>();
		}
		public static Slider GetSlider(this UIPrefabHelper helper, string label, string variant = null)
		{
			return helper.GetBasicObject(PrefabSet.BasicObjectTypes.Slider, label, variant).GetComponent<Slider>();
		}
		public static InputField GetInputField(this UIPrefabHelper helper, string label, string variant = null)
		{
			return helper.GetBasicObject(PrefabSet.BasicObjectTypes.InputField, label, variant).GetComponent<InputField>();
		}
		public static Text GetLabel(this UIPrefabHelper helper, string label, string variant = null)
		{
			return helper.GetBasicObject(PrefabSet.BasicObjectTypes.Label, label, variant).GetComponent<Text>();
		}
	}
}