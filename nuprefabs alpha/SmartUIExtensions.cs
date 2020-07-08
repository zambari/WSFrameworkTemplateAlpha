using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;

public static class SmartUIExtensions
{
    // public static void SetText(this Text text, object s)
    // {
    //     if (text != null) text.SetText(s.ToString());

    // }
    // /// <summary>
    // /// Null safe version of text=text
    // /// </summary>
    // public static void SetText(this Text text, string s)
    // {
    //     if (text != null) text.text = s;
    // }
    // /// <summary>
    // /// looks for smartui base, sets text if found
    // /// </summary>

    public static void SetLabel(this Component source, string s, bool setName = false)
    {
        var smartui = source.GetComponent<SmartUIBase>();
        if (smartui != null) smartui.label = s;
        else
        {
            var texte = source.GetComponentInChildren<Text>();
            if (texte != null) texte.SetText(s);
        }
        if (setName)
        {
            source.name = s;
        }
    }
}