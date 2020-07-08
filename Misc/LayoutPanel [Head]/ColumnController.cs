using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Z.LayoutPanel
{
	[ExecuteInEditMode]
	public class ColumnController : MonoBehaviour, IBorderControlListener//, IHasContent
	{
		public enum GroupPick { none, mainHoriz, column, panel }
		public GroupPick group;
		bool hasVertical;
		bool hasHorizontal;
		//VerticalLayoutGroup layoutGroupV { get { if (_layoutGroup == null) _layoutGroup = GetComponent<VerticalLayoutGroup>(); return _layoutGroup; } }

		[SerializeField]
		VerticalLayoutGroup _verticalLayoutGroup;
		//	VerticalLayoutGroup layoutGroupV { get { if (_layoutGroup == null) _layoutGroup = GetComponent<VerticalLayoutGroup>(); return _layoutGroup; } }

		[SerializeField]
		HorizontalLayoutGroup _horizontalLayoutGroup;
		[SerializeField]
		LayoutSetup _setup;
		public LayoutElement targetLayoutElement;
		public Transform content
		{
			get
			{
				if (group == GroupPick.column)
					if (_content == null)
					{
						if (targetLayoutElement != null)
							content = targetLayoutElement.transform;
						//var ver = GetComponent<VerticalLayoutGroup>();
						//if (ver != null) content = ver.transform as RectTransform;
					}
				return _content;
			}
			set { _content = value; }
		}
		public Transform _content;
		private void Reset()
		{
			if (targetLayoutElement == null) targetLayoutElement = GetComponentInChildren<LayoutElement>();
		}
		private void OnValidate()
		{
			if (targetLayoutElement == null) targetLayoutElement = GetComponentInChildren<LayoutElement>();
			//if ()
			if (content == null) return;
		}
		LayoutSetup setup
		{
			get
			{
				if (_setup == null)
				{
					//if (Lay)
					_setup = LayoutBorderControl.setup;
				}
				return _setup;
			}
		}
		private void OnEnable()
		{
			if (!zBench.PrefabModeIsActive(gameObject))
			{
				_verticalLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
				_horizontalLayoutGroup = GetComponentInChildren<HorizontalLayoutGroup>();
				LayoutBorderControl.Subscribe(this);
			}
		}
		private void OnDisable()
		{
			if (!zBench.PrefabModeIsActive(gameObject))
			{
				LayoutBorderControl.UnsSubscribe(this);
			}

		}

		[FormerlySerializedAs("applySettingsToVerticalLayout")]
		public bool applySettingsToVerticalLayout = true;
		public bool applySettingsToHorizontalLayout = true;
		public void UpdateLayoutSetupObject(LayoutSetup setup)
		{
			LayoutGroupSettings thisGroupSettings = null;
			if (group == GroupPick.none) return;
			if (zBench.PrefabModeIsActive(gameObject)) return;
			if (group == GroupPick.mainHoriz)
				thisGroupSettings = setup.mainhorizontalSettings;
			if (group == GroupPick.panel)
				thisGroupSettings = setup.panelGroupSettings;
			if (group == GroupPick.column)
				thisGroupSettings = setup.columnGroupSettings;
			// Debug.Log
			if (applySettingsToVerticalLayout && _verticalLayoutGroup != null)
			{
				thisGroupSettings.ApplyTo(_verticalLayoutGroup, setup);
			}

			if (applySettingsToHorizontalLayout && _horizontalLayoutGroup != null)
			{
				thisGroupSettings.ApplyTo(_horizontalLayoutGroup, setup);
			}
		}
	}
}