using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
// v.0.02 by zambari
namespace zUI {
    [CustomEditor (typeof (SmartSlider))]
    [CanEditMultipleObjects]
    public class SmartSliderEditor : SmartEditorBase {
        protected override void DisplayMainComponentShowHide (GUILayoutOption width) {
            GUILayout.Label ("Slider", width);

        }
        SerializedProperty onValueChanged;
        protected override void GetAddditionalFields () {
            onValueChanged = serializedObject.FindProperty ("OnValueChanged");

        }
        protected override void DisplayMainComponentShowHideToggle (GUILayoutOption width) {
            int newHideButton = GUILayout.Toolbar ((hideButton.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width);
            hideButton.boolValue = (newHideButton == 0);
        }
        SerializedProperty valueDisplay;
        protected override void DisplayTextInspector () {
            if (!hideText.boolValue) {
                if (valueDisplay == null) valueDisplay = serializedObject.FindProperty ("valueDisplay");
                EditorGUILayout.PropertyField (valueDisplay);
            }
            base.DisplayTextInspector ();
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

            //   if (isToggle.boolValue) 
            {
                GUILayout.Space (20);

                EditorGUILayout.PropertyField (onValueChanged);
                GUILayout.Space (20);
                // EditorGUILayout.PropertyField (isOn);
                // EditorGUILayout.PropertyField (onToggle);
                // EditorGUILayout.PropertyField (onClickOff);

                // //  EditorGUILayout.PropertyField(changeLabelOnToggle);
                // //  EditorGUILayout.PropertyField(autoToggleName);
                // // EditorGUILayout.PropertyField (toggleNamingConvention);
                // // if (toggleNamingConvention.enumValueIndex != 0)
                // { //
                //     GUILayout.BeginHorizontal ();
                //     GUILayout.BeginVertical ();
                //     GUILayout.Space (5);
                //     EditorGUILayout.PropertyField (label);

                //     // EditorGUILayout.PropertyField (labelWhenToggleOn);
                //     // GUILayout.EndVertical ();
                //     // GUILayout.BeginVertical ();
                //     // GUILayout.Space (20);

                //     // if (GUILayout.Button ("Copy normal Label")) {
                //     //     labelWhenToggleOn.stringValue = label.stringValue;
                //     // }

                //     // if (GUILayout.Button ("Swap labels label")) {
                //     //     string l = labelWhenToggleOn.stringValue;
                //     //     labelWhenToggleOn.stringValue = label.stringValue;
                //     //     label.stringValue = l;
                //     // }
                //     /* if (gameObjectToToggle.objectReferenceValue != null)
                //          if (GUILayout.Button("Get Toggle object"))
                //          {
                //              GameObject game = (GameObject)gameObjectToToggle.objectReferenceValue;

                //          }*/
                //     //  GUILayout.FlexibleSpace();
                //     GUILayout.EndVertical ();
                //     GUILayout.EndHorizontal ();

                // }

                // // EditorGUILayout.PropertyField (onOffValues);
                // // if (onOffValues.boolValue) {
                // EditorGUILayout.PropertyField (onClick);
                // EditorGUILayout.PropertyField (onClickOff);
                // // } else
                // EditorGUILayout.PropertyField (onToggle);

                // GUILayout.Space (10);

            } // else

        }
    }
}