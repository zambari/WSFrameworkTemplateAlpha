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
// v.0.03 by zambari
#pragma warning disable 414
// v.0.04 dodane define zeby nie walczyc ze stylessynce

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace zUI
{

    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class SmartButton : SmartUIBase //, ILayoutElement, ILayoutIgnorer
    {

        public UnityEvent OnClick;
        public UnityEvent OnClickOff;
        public BoolEvent OnToggle;
        public BoolEvent OnToggleInverted;
        public GameObject gameObjectToToggle;
        public GameObject gameObjectToToggleInverted;
        public bool _isOn;
        public bool isOn
        {
            get { return _isOn; }

            set
            {
                _isOn = value;

                if (OnToggle != null) OnToggle.Invoke(_isOn);
                if (OnToggleInverted != null) OnToggleInverted.Invoke(!_isOn);
                if (gameObjectToToggle != null)
                    gameObjectToToggle.ShowOrHide(_isOn);
                if (gameObjectToToggleInverted != null)
                    gameObjectToToggleInverted.ShowOrHide(!_isOn);

                HandleName();
            }

        }
        Button button { get { if (_button == null) _button = GetComponentInChildren<Button>(); return _button; } }
        Button _button;
        public override bool Interactable { get { return _Interactable; } set { button.interactable = value; _Interactable = value; } }

        //public SmartColor imageColor;
        public Button.ButtonClickedEvent onClick
        {
            get
            {
                return button.onClick;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            OnValidate();
            button.onClick.AddListener(OnButtonClick);
        }

        public virtual void OnButtonClick()
        {
            _isToggle = true;
            if (_isToggle)
            {
                isOn = !isOn;
                if (isOn)
                {
                    if (OnClick != null) OnClick.Invoke();
                }
                else
                {
                    if (OnClickOff != null) OnClickOff.Invoke();
                }
                if (OnToggle != null) OnToggle.Invoke(isOn);

            }
            else
            {
                Debug.Log("notogle");
                if (OnClick != null) OnClick.Invoke();
            }
        }


        protected override void SetComponentVisibility()
        {
            button.hideFlags = (hideButton ? HideFlags.HideInInspector : HideFlags.None);
        }
        protected override void ResetComponentVisibility()
        {
        if (button != null)
            {
                button.hideFlags = HideFlags.None;
                button.gameObject.hideFlags = HideFlags.None;
            }
        }
    }
}