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
    [RequireComponent(typeof(Scrollbar))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class SmartScrollbar : SmartUIBase
    { //, ILayoutElement, ILayoutIgnorer

        // public override bool Interactable { get { return _Interactable; } set { scrollbar.interactable = value; _Interactable = value; } }

        [SerializeField]
        Scrollbar scrollbar { get { if (_scrollbar == null) _scrollbar = GetComponentInChildren<Scrollbar>(); return _scrollbar; } }
        Scrollbar _scrollbar;

        protected override void ResetComponentVisibility()
        {
            if (scrollbar != null)
                scrollbar.hideFlags = HideFlags.None;
        }

        protected override void SetComponentVisibility()
        {
            scrollbar.hideFlags = (hideButton ? HideFlags.HideInInspector : HideFlags.None);
        }

        protected override void Reset()
        {

            base.Reset();
            namingConvention = Z.NameUtils.NamingConvention.doubleRound;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => namingConvention = Z.NameUtils.NamingConvention.doubleRound;
#endif

        }
    }

}