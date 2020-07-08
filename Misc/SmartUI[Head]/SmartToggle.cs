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

#pragma warning disable 414

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace zUI
{
    [RequireComponent(typeof(Toggle))]
#if USE_IMAGES
    // [RequireComponent(typeof(Image))]
#endif
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class SmartToggle : SmartUIBase
    { //, ILayoutElement, ILayoutIgnorer
        [SerializeField] Toggle _toggle;

        [Header("Edit the original object!")]
        public UnityEngine.UI.Toggle.ToggleEvent OnToggle;
        public BoolEvent OnToggleInverted;
        public UnityEvent OnTrue;
        public UnityEvent OnFalse;
        public GameObject gameObjectToToggle;
        public GameObject gameObjectToToggleInverted;
        public bool _isOn;
        public bool isOn
        {
            get { return _isOn; }

            set
            {
                _isOn = value;
                // if (OnToggle != null) OnToggle.Invoke(_isOn); //stackoverflow
                if (OnToggleInverted != null) OnToggleInverted.Invoke(!_isOn);
                if (gameObjectToToggle != null)
                    gameObjectToToggle.ShowOrHide(_isOn);
                if (gameObjectToToggleInverted != null)
                    gameObjectToToggleInverted.ShowOrHide(!_isOn);
                if (_isOn && OnTrue != null) OnTrue.Invoke();
                if (!_isOn && OnFalse != null) OnFalse.Invoke();
                HandleName();
            }

        }

        public override bool Interactable { get { return _Interactable; } set { toggle.interactable = value; _Interactable = value; } }

        Toggle toggle { get { if (_toggle == null) _toggle = GetComponent<Toggle>(); return _toggle; } }

        public Toggle.ToggleEvent onValueChanged
        {
            get
            {
                return toggle.onValueChanged;
            }
        } //wrapper
        void Start()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);

        }

        public void OnValueChanged(bool newOn)
        {
            isOn = newOn;
            if (isOn)
            {
                if (OnTrue != null) OnTrue.Invoke();
            }
            else
            {
                if (OnFalse != null) OnFalse.Invoke();
            }
            if (OnToggleInverted != null) OnToggleInverted.Invoke(!isOn);

        }
        protected override void ResetComponentVisibility()
        {
            if (toggle != null)
                toggle.hideFlags = HideFlags.None;

        }
        protected override void SetComponentVisibility()
        {
            if (toggle != null)
                toggle.hideFlags = (hideButton ? HideFlags.HideInInspector : HideFlags.None);
        }
        public override void OnValidate()
        {
            base.OnValidate();
            OnToggle = toggle.onValueChanged;

        }
        protected override void Reset()
        {
            base.Reset();
            if (text != null)
                label = text.text;
            namingConvention = Z.NameUtils.NamingConvention.hexBrackets;
        }

    }

}