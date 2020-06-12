using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using zUniqueness;
using UnityEditor;
using System.Security.Cryptography;
using System.Text;

[CustomEditor(typeof(ObjectID))]
public class ObjectIDEditor : Editor
{
    GUIStyle fontStyle;
    SerializedProperty ida;
    SerializedProperty idb;
    bool showObjectList;

    static Texture2D _bgTexture;
    public static void DrawBG()
    {
        Rect rect = EditorGUILayout.GetControlRect();
        DrawBG(rect);
    }
    public static void DrawBG(Rect rect)
    {
        rect.y = rect.y - 18;
        rect.x = rect.x - 15;
        Texture2D texture = bgTexture;
        if (texture != null)
        {
            for (int x = 0; x < rect.width; x += texture.width)
                for (int y = 0; y < rect.height; y += texture.height)
                    GUI.DrawTextureWithTexCoords(new Rect(rect.x + x, rect.y + y, texture.width, texture.height), texture, new Rect(0f, 0f, 1f, 1f));
        }
    }
    public static Texture2D bgTexture
    {
        get
        {
            if (_bgTexture == null)
                _bgTexture = Resources.Load("stripebg") as Texture2D;
            return _bgTexture;
        }
    }

    public override void OnInspectorGUI() //2
    {
        DrawBG();

        if (fontStyle == null)
        {
            fontStyle = new GUIStyle(EditorStyles.miniLabel);
            fontStyle.fontSize = 9;
        }
        if (ida == null)
        {
            ida = serializedObject.FindProperty("idStringPartA");
            idb = serializedObject.FindProperty("idStringPartB");
        }
        Rect t = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniLabel);
        t.x = t.width - 90;
        t.y -= 14;
        GUI.Label(t, ida.stringValue, fontStyle);
        t.y += 15;
        GUI.Label(t, idb.stringValue, fontStyle);
        showObjectList = EditorGUILayout.Foldout(showObjectList, "Show known objects");
        if (showObjectList)
        {
            var prop = serializedObject.FindProperty("identifier");
            var targetObj = (target as ObjectID);
            GUILayout.Label("My id:");
            //            ulong val = Convert.ToUInt64(prop.longValue);


            GUILayout.TextArea("v " + (targetObj.identifier).ToString());

            GUILayout.Label("All list");
            if (ObjectID.identifierList == null) ObjectID.identifierList = new List<ulong>();
            int count = ObjectID.identifierList.Count;
            int limit = 150;
            if (count > limit) count = limit;
            for (int i = 0; i < count; i++)
            {
                var obj = ObjectID.objectDict[ObjectID.identifierList[i]];
                GUILayout.Label(ObjectID.identifierList[i].ToString() + (obj == null ? " null" : obj.name) + " " + ObjectID.identifierList[i]);
            }
            if (ObjectID.identifierList.Count > limit)
            {
                GUILayout.Label("()...)And "+(ObjectID.identifierList.Count - limit)+" more");
            }

        }
        //    DrawDefaultInspector();
    }
}