using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
// v.0.02 by zambari
namespace zUI {
    [CustomEditor (typeof (SmartButton))]
    [CanEditMultipleObjects]
    public class SmartButtonEditor : SmartEditorBase {
        protected override void DisplayMainComponentShowHide (GUILayoutOption width) {
            GUILayout.Label ("Button", width);

        }
        protected override void DisplayMainComponentShowHideToggle (GUILayoutOption width) {
            int newHideButton = GUILayout.Toolbar ((hideButton.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width);
            hideButton.boolValue = (newHideButton == 0);
        }
        protected SerializedProperty toggleNamingConvention;

        protected SerializedProperty onClick;
        protected SerializedProperty onClickOff;
        protected SerializedProperty isOn;
        protected SerializedProperty isToggle;
        protected SerializedProperty onToggle;
        protected SerializedProperty onToggleInverted;
        protected SerializedProperty onOffValues;
        protected SerializedProperty gameObjectToToggle;
        protected SerializedProperty gameObjectToToggleInverted;

        protected override void GetAddditionalFields () {

            if (onClick == null) onClick = serializedObject.FindProperty ("OnClick");
            if (isOn == null) isOn = serializedObject.FindProperty ("_isOn");
            if (isToggle == null) isToggle = serializedObject.FindProperty ("_isToggle");
            if (onClickOff == null) onClickOff = serializedObject.FindProperty ("OnClickOff");
            if (onToggle == null) onToggle = serializedObject.FindProperty ("OnToggle");
            if (onToggleInverted == null) onToggleInverted = serializedObject.FindProperty ("OnToggleInverted");
            if (onOffValues == null) onOffValues = serializedObject.FindProperty ("seperateOnOffActions");
            if (gameObjectToToggle == null) gameObjectToToggle = serializedObject.FindProperty ("gameObjectToToggle");
            if (gameObjectToToggleInverted == null) gameObjectToToggleInverted = serializedObject.FindProperty ("gameObjectToToggleInverted");
        }
        protected override void DisplayActionInspector () {

            if (toggleNamingConvention == null) toggleNamingConvention = serializedObject.FindProperty ("toggleNamingConvention");
            // 

            {
                GUILayout.Space (10);
                EditorGUILayout.PropertyField (onClick);
                EditorGUILayout.PropertyField (isToggle);
                GUILayout.Space (10);
                if (isToggle.boolValue) {
                    EditorGUILayout.PropertyField (onClickOff);
                    EditorGUILayout.PropertyField (onToggle);
                    EditorGUILayout.PropertyField (onToggleInverted);
                    EditorGUILayout.PropertyField (isOn);

                }
                //  EditorGUILayout.PropertyField(changeLabelOnToggle);
                //  EditorGUILayout.PropertyField(autoToggleName);
                // EditorGUILayout.PropertyField (toggleNamingConvention);
                // if (toggleNamingConvention.enumValueIndex != 0)
                // { //
                // GUILayout.BeginHorizontal ();
                // GUILayout.BeginVertical ();
                GUILayout.Space (5);

                // EditorGUILayout.PropertyField (labelWhenToggleOn);
                // GUILayout.EndVertical ();
                // GUILayout.BeginVertical ();
                // GUILayout.Space (20);

                // if (GUILayout.Button ("Copy normal Label")) {
                //     labelWhenToggleOn.stringValue = label.stringValue;
                // }

                // if (GUILayout.Button ("Swap labels label")) {
                //     string l = labelWhenToggleOn.stringValue;
                //     labelWhenToggleOn.stringValue = label.stringValue;
                //     label.stringValue = l;
                // }
                /* if (gameObjectToToggle.objectReferenceValue != null)
                     if (GUILayout.Button("Get Toggle object"))
                     {
                         GameObject game = (GameObject)gameObjectToToggle.objectReferenceValue;


                     }*/
                //  GUILayout.FlexibleSpace();
                // GUILayout.EndVertical ();
                // GUILayout.EndHorizontal ();

                // }

                // EditorGUILayout.PropertyField (onOffValues);
                // if (onOffValues.boolValue) {
                // EditorGUILayout.PropertyField (onClick);
                // EditorGUILayout.PropertyField (onClickOff);
                // // } else
                // EditorGUILayout.PropertyField (onToggle);

                GUILayout.Space (10);

                EditorGUILayout.PropertyField (gameObjectToToggle);

                EditorGUILayout.PropertyField (gameObjectToToggleInverted);
            } // else

        }
        // protected override void DisplayComponentShowHide ()
        //  {
        //     GUILayoutOption w = GUILayout.Width (100);
        //     int space = 30;
        //     int space2 = 5;
        //     GUILayout.BeginHorizontal ();
        //     GUILayout.Space (space);
        //     GUILayout.Label ("Image", w);
        //     GUILayout.Space (space2);
        //     GUILayout.Label ("Button", w);
        //     GUILayout.Space (space2);
        //     GUILayout.Label ("Text", w);
        //     GUILayout.EndHorizontal ();
        //     GUILayout.BeginHorizontal ();
        //     //GUILayout.Label("Image");
        //     int newHideImage = GUILayout.Toolbar ((hideImage.boolValue ? 0 : 1), showHide, w);
        //     GUILayout.Space (space2);
        //     //GUILayout.Label("Button");
        //     int newHideButton = GUILayout.Toolbar ((hideButton.boolValue ? 0 : 1), showHide, w);
        //     GUILayout.Space (space2);
        //     //GUILayout.Label("Text");
        //     int newHideText = GUILayout.Toolbar ((hideText.boolValue ? 0 : 1), showHide, w);
        //     //   if (newHideText == 1)

        //     GUILayout.FlexibleSpace ();
        //     GUILayout.EndHorizontal ();
        //     hideText.boolValue = (newHideText == 0);
        //     hideImage.boolValue = (newHideImage == 0);
        //     hideButton.boolValue = (newHideButton == 0);
        //     GUILayout.Space (16);
        // }
    }
}