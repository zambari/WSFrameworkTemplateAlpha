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
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class SmartLabel : SmartUIBase
    { //, ILayoutElement, ILayoutIgnorer

      
        public GameObject gameObjectToToggle;
        public GameObject gameObjectToToggleInverted;
        public bool _isOn;
        public bool isOn
        {
            get { return _isOn; }

            set
            {
                _isOn = value;
                if (gameObjectToToggle != null)
                    gameObjectToToggle.ShowOrHide(_isOn);
                if (gameObjectToToggleInverted != null)
                    gameObjectToToggleInverted.ShowOrHide(!_isOn);
                HandleName();
            }

        }

      




        void Start()
        {

        }


        protected override void ResetComponentVisibility()
        {
            // if (te != null)
            //     toggle.hideFlags = HideFlags.None;

        }
        protected override void SetComponentVisibility()
        {
            // if (toggle != null)
                // toggle.hideFlags = (hideButton ? HideFlags.HideInInspector : HideFlags.None);
        }
        public override void OnValidate()
        {
            base.OnValidate();
            // OnToggle = toggle.onValueChanged;
        
        }
        protected override void Reset()
        {
            base.Reset();
         
            namingConvention = Z.NameUtils.NamingConvention.hexBrackets;
        }

    }

}