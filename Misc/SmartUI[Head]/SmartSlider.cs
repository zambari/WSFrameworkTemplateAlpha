// #undef USE_IMAGES
// #undef USE_TEXTCONTROL
#define USE_IMAGES
#define USE_TEXTCONTROL
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Z;

#pragma warning disable 414

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace zUI
{
    [RequireComponent(typeof(Slider))]
#if USE_IMAGES
    // [RequireComponent(typeof(Image))]
#endif
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class SmartSlider : SmartUIBase
    { //, ILayoutElement, ILayoutIgnorer

        public Slider.SliderEvent OnValueChanged;

        public override bool Interactable { get { return _Interactable; } set { slider.interactable = value; _Interactable = value; } }

        [SerializeField]
        Slider _slider;
        Slider slider { get { if (_slider == null) _slider = GetComponentInChildren<Slider>(); return _slider; } }

        public Slider.SliderEvent onValueChanged
        {
            get
            {
                return slider.onValueChanged;
            }
        } //wrapper

        public SliderValueDisplay valueDisplay;
        public bool hasValueDisplay;
    
        protected override void SetComponentVisibility()
        {
            slider.hideFlags = (hideButton ? HideFlags.HideInInspector : HideFlags.None);
            if (valueDisplay == null)
            {
                valueDisplay = GetComponentInChildren<SliderValueDisplay>();
                hasValueDisplay = valueDisplay != null;
            }
            else { hasValueDisplay = true; }
            //   toggle.transition = Selectable.Transition.None;

            if (valueDisplay != null)
            {
                valueDisplay.gameObject.hideFlags = (hideText ? HideFlags.HideInHierarchy : HideFlags.None);
            }
        }
        protected override void ResetComponentVisibility()
        {
            if (slider != null)
            {
                slider.hideFlags = HideFlags.None;
                slider.gameObject.hideFlags = HideFlags.None;

            }
            //#if USE_IMAGES
            //#endif
            if (valueDisplay != null)
            {
                valueDisplay.gameObject.hideFlags = HideFlags.None;
            }
        }

        public void CreateTexts()
        {
            rect.AddChild().AddOrGetComponent<Text>();
            text.text = name;
            _label = name;
            var text2 = rect.AddChild().AddOrGetComponent<Text>();
            text.name = "TextLabel";
            valueDisplay = text2.AddOrGetComponent<SliderValueDisplay>();
            text2.name = "TextValue";
            text2.color = Color.white;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;

            text2.alignment = TextAnchor.MiddleRight;
            var recttex1 = text.GetComponent<RectTransform>();
            var recttex2 = text2.GetComponent<RectTransform>();
            recttex1.anchorMin = new Vector2(0, 0);
            recttex1.anchorMax = new Vector2(1, 1);
            recttex1.offsetMin = new Vector2(4, -3);
            recttex1.offsetMax = new Vector2(4, 3);
            recttex2.offsetMin = recttex1.offsetMax;
            recttex2.offsetMax = recttex1.offsetMax;
            recttex2.anchorMin = recttex1.anchorMin;
            recttex2.anchorMax = recttex1.anchorMax;
            Debug.Log("Created texts", gameObject);
            OnValidate();
        }
        protected override void Reset()
        {

            var texts = GetComponentsInChildren<Text>();
            if (texts.Length == 0)
            {
                CreateTexts();
            }
            label = name;
            base.Reset();
            namingConvention = Z.NameUtils.NamingConvention.oval;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => namingConvention = Z.NameUtils.NamingConvention.oval;
#endif
        }
        public override void OnValidate()
        {
            base.OnValidate();
            if (slider != null)
                OnValueChanged = slider.onValueChanged;
        }
        protected override void Awake()
        {
            base.Awake();

            //slider.onValueChanged.AddListener(OnValueChangedCallbak);

            //OnValueChanged=slider.onValueChanged;
        }
    }

}