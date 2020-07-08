using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
// v.0.02 by zambari
namespace zUI {
    [CustomEditor (typeof (SmartToggle))]
    [CanEditMultipleObjects]
    public class SmartToggleEditor : SmartEditorBase {

        protected override void DisplayMainComponentShowHide (GUILayoutOption width) {
            GUILayout.Label ("Toggle", width);

        }
        protected override void DisplayMainComponentShowHideToggle (GUILayoutOption width) {
            int newHideButton = GUILayout.Toolbar ((hideButton.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width);
            hideButton.boolValue = (newHideButton == 0);
        }

        protected SerializedProperty OnToggle;
        protected SerializedProperty isOn;
        protected SerializedProperty OnToggleInverted;
        protected SerializedProperty OnTrue;
        protected SerializedProperty OnFalse;
        protected SerializedProperty gameObjectToToggle;
        protected SerializedProperty gameObjectToToggleInverted;

        
        protected override void GetAddditionalFields () {

            if (OnToggle == null) OnToggle = serializedObject.FindProperty ("OnToggle");
            if (isOn == null) isOn = serializedObject.FindProperty ("_isOn");
            if (OnToggleInverted == null) OnToggleInverted = serializedObject.FindProperty ("OnToggleInverted");
            if (OnFalse == null) OnFalse = serializedObject.FindProperty ("OnFalse");
            if (OnTrue == null) OnTrue = serializedObject.FindProperty ("OnTrue");
            if (gameObjectToToggle == null) gameObjectToToggle = serializedObject.FindProperty ("gameObjectToToggle");
            if (gameObjectToToggleInverted == null) gameObjectToToggleInverted = serializedObject.FindProperty ("gameObjectToToggleInverted");

        }

        protected override void DisplayActionInspector () {

            GUILayout.Space (10);
            EditorGUILayout.PropertyField (OnToggle);
            EditorGUILayout.PropertyField (OnToggleInverted);

            GUILayout.Space (10);
            EditorGUILayout.PropertyField (OnTrue);
            EditorGUILayout.PropertyField (OnFalse);
            //   if (isToggle.boolValue) 
            GUILayout.Space (10);
            EditorGUILayout.PropertyField (isOn);

            // //  EditorGUILayout.PropertyField(changeLabelOnToggle);
            // //  EditorGUILayout.PropertyField(autoToggleName);
            // // EditorGUILayout.PropertyField (toggleNamingConvention);
            // // if (toggleNamingConvention.enumValueIndex != 0)
            // { //
            //     GUILayout.BeginHorizontal ();
            //     GUILayout.BeginVertical ();
            //     GUILayout.Space (5);
            //     EditorGUILayout.PropertyField (label);

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
            //    GUILayout.EndVertical ();
            //   GUILayout.EndHorizontal ();

        // EditorGUILayout.PropertyField (onOffValues);
        // if (onOffValues.boolValue) {
        // EditorGUILayout.PropertyField (onClick);
        // EditorGUILayout.PropertyField (onClickOff);
        // } else
        //  EditorGUILayout.PropertyField (onToggle);
        //
        //GUILayout.Space (10);

        EditorGUILayout.PropertyField (gameObjectToToggle);
        EditorGUILayout.PropertyField (gameObjectToToggleInverted);
    }

}
}