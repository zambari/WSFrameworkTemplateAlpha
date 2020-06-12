using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
// v.0.02 by zambari
namespace zUI {
    [CustomEditor (typeof (SmartScrollbar))]
    [CanEditMultipleObjects]
    public class SmartScrollbarEditor : SmartEditorBase {
        protected override void DisplayMainComponentShowHide (GUILayoutOption width) {
            GUILayout.Label ("Scrollbar", width);

        }
        protected override void DisplayMainComponentShowHideToggle (GUILayoutOption width) {
            int newHideButton = GUILayout.Toolbar ((hideButton.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width);
            hideButton.boolValue = (newHideButton == 0);
        }

        protected override void DisplayLookInspector () {
            base.DisplayLookInspector ();

            if (GUILayout.Button ("Remove sprites")) {
                var images = (target as SmartUIBase).GetComponentsInChildren<Image> ();
                int changed = 0;
                foreach (var i in images) {

                    if (i.sprite != null) {

                        Undo.RegisterCompleteObjectUndo (i, "sprite");
                        changed++;
                        i.sprite = null;
                    }
                }
                if (changed == 0) {
                    Debug.Log ("No sprites found, nothign changed");
                } else {
                    Debug.Log ("Removed " + changed + " sprites");
                }
            }
        }
        protected override void DisplayActionInspector () {

            GUILayout.Space (20);
            GUILayout.Label ("sorry to dissapoint, but only child hiding is useful in smartscrollbar");

            GUILayout.Space (20);
        }
    }
}