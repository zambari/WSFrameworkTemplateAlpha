using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDefaultFont : MonoBehaviour
{
    public Font _defaultFont;
    public static Font defaultFont;
    public static int defaultFontSize;
    public int _defaultFontSize=10;

    void Awake()
    {
        defaultFontSize = _defaultFontSize; 
        if (_defaultFont != null) defaultFont = _defaultFont;

    }

}
