using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
// v.0.02 by zambari
// v.0.03 2019_3 
namespace zUI
{

    public abstract class SmartEditorBase : Editor
    {

        protected SerializedProperty textAlignment;
        protected SerializedProperty fontSize;
        protected SerializedProperty sprite;
        protected SerializedProperty useTranslation;
        protected SerializedProperty font;

        protected SerializedProperty label;
        protected SerializedProperty labelWhenToggleOn;
        //SerializedProperty changeLabelOnToggle;
        protected SerializedProperty hideText;
        protected SerializedProperty hideButton;
        protected SerializedProperty hideImage;
        protected SerializedProperty ignoreLayout;
        protected SerializedProperty width;
        protected SerializedProperty height;
        protected SerializedProperty flexibleWidth;
        protected SerializedProperty flexibleHeight;
        protected SerializedProperty namingConvention;
        protected SerializedProperty colorTab;
        protected SerializedProperty textColor;

        protected SerializedProperty imageColor;

        protected SerializedProperty imageColorMode;
        protected SerializedProperty textColorMode;
        protected SerializedProperty smartColor;
        protected SerializedProperty hasLayoutElement;
        protected SerializedProperty autoAdjustForText;
        protected SerializedProperty autoAdjustMargin;
        protected SerializedProperty hideChldren;
        protected SerializedProperty childCount;
        protected SerializedProperty text;
        protected SerializedProperty image;
        protected SerializedProperty applyColorsToText;
        protected SerializedProperty applyColorsToImage;
        // protected SerializedProperty getTransformInChilrenCount;

        protected SerializedProperty displayNumberOfHiddenChildren;

        // protected SerializedProperty autoToggleName;
        protected SerializedProperty isHidingLayoutElement;
        protected string[] views;
        protected string[] showHide = { "Hide", "Show" };
        public override void OnInspectorGUI()
        {

            Rect rect1 = EditorGUILayout.GetControlRect();
            rect1.width += 8;
            DrawBG(rect1);
            serializedObject.Update();
            GUILayout.Space(15);
            DisplayComponentShowHide();

            SmartUIBase.view = GUILayout.Toolbar(SmartUIBase.view, views);
            switch (SmartUIBase.view)
            {
                case (int) SmartUIBase.SmartViews.Text:
                    DisplayTextInspector();
                    break;
                case (int) SmartUIBase.SmartViews.Actions:
                    GetActionProperties();
                    DisplayActionInspector();
                    break;
                case (int) SmartUIBase.SmartViews.Look:
                    DisplayLookInspector();

                    break;
                case (int) SmartUIBase.SmartViews.Naming:
                    DisplayConfigInspector();
                    break;
                    // case (int) SmartButton.ButtonViews.Look:
                    //     DisplayLookInspector ();
                    //     break;
                    /*    case (int)ButtonViews.config:
                            DisplayConfigInspector();
                            break;
                        case (int)ButtonViews.attribs:
                            DisplayAttribInspector();
                            break;*/
            }
            //   GUILayout.Space(15);

            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(15);
        }
        // GUIStyle pressedButton;
        public static Texture2D bgTexture
        {
            get
            {
                // comment this line to read textures every time (for checing out new textures)
                if (_bgTexture == null)
                    _bgTexture = Resources.Load("stripebg") as Texture2D;
                return _bgTexture;
            }
        }

        static Texture2D _bgTexture;

        public static void DrawBG(Rect rect)
        {
#if UNITY_2019_3_OR_NEWER
            rect.y = rect.y - 24;
            rect.x = rect.x - 18;
#else

            rect.y = rect.y - 18;
            rect.x = rect.x - 15;
#endif
            Texture2D texture = bgTexture;
            if (texture != null)
            {
                for (int x = 0; x < rect.width; x += texture.width)
                    for (int y = 0; y < rect.height; y += texture.height)
                        GUI.DrawTextureWithTexCoords(new Rect(rect.x + x, rect.y + y, texture.width, texture.height), texture, new Rect(0f, 0f, 1f, 1f));
            }
        }
        protected virtual void GetAddditionalFields()

        {}
        protected void GetActionProperties()
        {
            GetAddditionalFields();

            //    if (autoToggleName == null) autoToggleName = serializedObject.FindProperty("autoToggleName");

            //autoToggleName.CheckProperty(so,"autoToggleName");
            //toggleNamingConvention.CheckProperty(so,"toggleNamingConvention");
        }
        protected SerializedObject so;

        protected void GetTextProperties()
        {
            if (label == null) label = serializedObject.FindProperty("_label");
            if (labelWhenToggleOn == null) labelWhenToggleOn = serializedObject.FindProperty("labelWhenToggleOn");
            //  if (changeLabelOnToggle == null) changeLabelOnToggle = serializedObject.FindProperty("changeLabelOnToggle");
            if (autoAdjustForText == null) autoAdjustForText = serializedObject.FindProperty("autoAdjustForText");

            if (autoAdjustMargin == null) autoAdjustMargin = serializedObject.FindProperty("autoAdjustMargin");

            if (textAlignment == null) textAlignment = serializedObject.FindProperty("textAlignment");
            if (font == null) font = serializedObject.FindProperty("font");
            if (fontSize == null) fontSize = serializedObject.FindProperty("_fontSize");
            if (hideText == null) hideText = serializedObject.FindProperty("hideText");
            if (hideButton == null) hideButton = serializedObject.FindProperty("hideButton");
            if (hideImage == null) hideImage = serializedObject.FindProperty("hideImage");
            if (text == null) text = serializedObject.FindProperty("_text");
            if (image == null) image = serializedObject.FindProperty("_image");

            if (useTranslation == null) useTranslation = serializedObject.FindProperty("useTranslation");
        }

        protected void GetLookProperties()
        {
            if (width == null) width = serializedObject.FindProperty("_width");
            if (height == null) height = serializedObject.FindProperty("_height");
            if (sprite == null) sprite = serializedObject.FindProperty("_sprite");
            if (flexibleWidth == null) flexibleWidth = serializedObject.FindProperty("_flexibleWidth");
            if (flexibleHeight == null) flexibleHeight = serializedObject.FindProperty("_flexibleHeight");
            if (ignoreLayout == null) ignoreLayout = serializedObject.FindProperty("_ignoreLayout");
            if (colorTab == null) colorTab = serializedObject.FindProperty("colorTab");
            if (textColor == null) textColor = serializedObject.FindProperty("textColor");
            if (imageColor == null) imageColor = serializedObject.FindProperty("imageColor");
            if (imageColorMode == null) imageColorMode = serializedObject.FindProperty("imageColorMode");
            if (textColorMode == null) textColorMode = serializedObject.FindProperty("textColorMode");
            //  if (smartColor == null) smartColor = serializedObject.FindProperty("smartColor");
        }

        protected virtual void OnEnable()
        {
            so = serializedObject;
            views = System.Enum.GetNames(typeof(SmartUIBase.SmartViews));
            GetLookProperties();
            GetTextProperties();
            GetActionProperties();
            if (hideChldren == null) hideChldren = serializedObject.FindProperty("hideChildrenRead");
            if (applyColorsToText == null) applyColorsToText = serializedObject.FindProperty("applyColorsToText");
            if (applyColorsToImage == null) applyColorsToImage = serializedObject.FindProperty("applyColorsToImage");

            if (childCount == null) childCount = serializedObject.FindProperty("childCount");
            if (displayNumberOfHiddenChildren == null) displayNumberOfHiddenChildren = serializedObject.FindProperty("displayNumberOfHiddenChildren");
            if (hasLayoutElement == null) hasLayoutElement = serializedObject.FindProperty("hasLayoutElement");
            if (isHidingLayoutElement == null) isHidingLayoutElement = serializedObject.FindProperty("isHidingLayoutElement");
            // pressedButton=new GUIStyle(GUI.skin.GetStyle("Button"));
        }

        protected virtual void DisplayActionInspector()
        {

        }

        protected virtual void DisplayConfigInspector()
        {
            GUILayout.Space(30);

            if (namingConvention == null) namingConvention = serializedObject.FindProperty("namingConvention");
            EditorGUILayout.PropertyField(namingConvention);
            GUILayout.Space(30);

        }
        protected virtual void DisplayTextInspector()
        {

            if (text.objectReferenceValue == null)
            {
                GUILayout.Label("Text object not found");
                return;
            }
            if (!hideText.boolValue)
            {
                GUILayout.Label("Text object is visible");
                EditorGUILayout.PropertyField(text);
                if (GUILayout.Button("Select"))
                {
                    Text t = (serializedObject.targetObject as MonoBehaviour).GetComponentInChildren<Text>();
                    if (t != null)
                        Selection.activeObject = t;
                }
                return;
            }

            if (serializedObject.targetObjects.Length <= 1 || SmartButton.editTogether)
            {
                EditorGUILayout.PropertyField(label);
            }
            else
            {
                for (int i = 0; i < serializedObject.targetObjects.Length; i++)
                {
                    SmartButton thisButton = (SmartButton) serializedObject.targetObjects[i];
                    string label = thisButton.label;
                    string newLabel = GUILayout.TextArea(label);
                    if (label != newLabel) thisButton.label = newLabel;
                }
            }
            if (serializedObject.targetObjects.Length > 1)
                SmartButton.editTogether = GUILayout.Toggle(SmartButton.editTogether, "Edit multiple buttons together");
            EditorGUILayout.Space();

            // EditorGUILayout.PropertyField (autoAdjustForText);
            //  if (autoAdjustForText.boolValue)
            //      EditorGUILayout.PropertyField (autoAdjustMargin);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(textAlignment);
            EditorGUILayout.PropertyField(font);
            if (fontSize != null)
                EditorGUILayout.PropertyField(fontSize);
            EditorGUILayout.Space();
            if (textColor != null)
            {
                EditorGUILayout.PropertyField(applyColorsToText);
                EditorGUILayout.PropertyField(textColor);
            }

        }

        protected virtual void DisplayLookInspector()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(displayNumberOfHiddenChildren);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(sprite);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(image);
            EditorGUILayout.Space();
            if (imageColor != null)
            {
                EditorGUILayout.PropertyField(applyColorsToImage);
                EditorGUILayout.PropertyField(imageColor);
            }
            // EditorGUILayout.LabelField ("Layout:");
            // EditorGUILayout.PropertyField (autoAdjustForText);
            // if (!autoAdjustForText.boolValue) {
            //     EditorGUILayout.PropertyField (width);
            // } else {
            //     EditorGUILayout.PropertyField (autoAdjustMargin);
            // }
            // EditorGUILayout.PropertyField (height);
            // EditorGUILayout.PropertyField (flexibleWidth);
            // EditorGUILayout.PropertyField (flexibleHeight);
            // EditorGUILayout.PropertyField (ignoreLayout);
            EditorGUILayout.Space();
            DisplayConfigInspector();
        }
        protected virtual void DisplayMainComponentShowHide(GUILayoutOption options)
        {

        }

        protected virtual void DisplayMainComponentShowHideToggle(GUILayoutOption options)
        {

        }

        protected virtual void DisplayComponentShowHide()
        {
            GUILayoutOption width = GUILayout.Width(80);
            int space = 30;
            int space2 = 5;
            GUILayout.BeginHorizontal();
            bool hasimage = (target as SmartUIBase).GetComponent<Image>() != null;
            if (hasimage)
            {
                GUILayout.Space(space);
                GUILayout.Label("Image", width);
            }
            if (text.objectReferenceValue != null)
            {
                GUILayout.Space(space2);
                GUILayout.Label("Text", width);
            }

            bool hasLayout = (target as SmartUIBase).GetComponent<LayoutElement>() != null;
            if (hasLayout)
            {
                GUILayout.Space(space2);
                GUILayout.Label("Layout", width);
            }

            // GUILayout.Space (space2);
            DisplayMainComponentShowHide(width);
            GUILayout.FlexibleSpace();
            //  GUILayout.Space (space2);
            // GUILayout.Space (space2);
            //   GUILayout.Space (space2);
            GUILayout.Label("Children", width);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            //GUILayout.Label("Image");
            if (hasimage)
            {
                int newHideImage = GUILayout.Toolbar((hideImage.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width); //, width
                GUILayout.Space(space2);
                hideImage.boolValue = (newHideImage == 0);
            }
            if (text.objectReferenceValue != null)
            {
                int newHideText = GUILayout.Toolbar((hideText.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width);
                hideText.boolValue = (newHideText == 0);
            }

            if (hasLayout)
            {
                GUILayout.Space(space2);
                int newHideLayout = GUILayout.Toolbar((isHidingLayoutElement.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width);
                if (newHideLayout != (isHidingLayoutElement.boolValue?0 : 1))
                {

                    isHidingLayoutElement.boolValue = newHideLayout == 0;
                    // Debug.Log ("val " + isHidingLayoutElement.boolValue);
                    foreach (var t in targets)
                    {
                        (t as SmartUIBase).SetHidingLayoutElement(isHidingLayoutElement.boolValue);
                    }

                }
            }
            DisplayMainComponentShowHideToggle(width);

            GUILayout.FlexibleSpace();
            int newShowChildren = GUILayout.Toolbar((hideChldren.boolValue ? 0 : 1), showHide, EditorStyles.miniButton, width);
            bool newVal = newShowChildren == 0;
            if (newVal != hideChldren.boolValue)
            {
                Debug.Log("value changed " + newShowChildren + " " + hideChldren.boolValue);
                foreach (var t in targets)
                {
                    (t as SmartUIBase).SetChildrenHideState(newVal);
                }
                RefreshHierarchy();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(16);
        }
        GameObject[] lastSelection;

        void RefreshHierarchy()
        {
            lastSelection = Selection.gameObjects;
            Selection.activeGameObject = null;
            EditorApplication.delayCall += () =>
            {
                Selection.objects = lastSelection;
            };
        }

    }
}