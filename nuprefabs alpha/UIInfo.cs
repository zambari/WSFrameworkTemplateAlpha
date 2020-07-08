using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;

[System.Serializable]
public class TopInfo
{
    public Image image;
    public Text label;
    public RectTransform rect;
    public Button foldButton;
    public Text foldLabel;
    public Toggle toggle;
    public TopInfo(RectTransform rect)
    {
        this.rect = rect;

    }
    public TopInfo(Transform rect)
    {
        this.rect = rect as RectTransform;

    }
    Button _button;
    public Button button { get { if (_button == null && rect != null) _button = rect.gameObject.AddOrGetComponent<Button>(); return _button; } set { _button = value; } }
    Color _color;
    public void SetColor(Color c)
    {
        _color = c;
        image.SetColor(c);
    }
    public Color color
    {
        get { if (image != null) _color = image.color; return _color; }
        set { _color = value; image.SetColor(value); }
    }
}
public static class PanelInfoExtensions
{
    public static bool IsEmptyOrNull (this PanelInfo info)
    {
        if (info==null || info.mainRect==null) return true;
        return false;
    }
}
[System.Serializable]
public class PanelInfo
{

    public RectTransform mainRect;
    public RectTransform _content;
    public RectTransform _frame;
    public RectTransform frame { get { if (_frame == null) return mainRect; return _frame; } set { _frame = value; } }
    public static PanelInfo FromRect(RectTransform rect)
    {
        PanelInfo info = new PanelInfo(rect);
        return info;
    }
    public Color color
    {
        get
        {
            // if (_color == default(Color) && top != null) _color = top.rect.GetComponentInChildren<Image>().color; return _color;
            if (top != null) return top.color;
            return Color.magenta;
        }
        set { top.color = value; }
    }

    public static PanelInfo FromRect(Transform rect)
    {
        PanelInfo info = new PanelInfo(rect);
        return info;
    }
    public PanelInfo(Transform src)
    {
        mainRect = src as RectTransform;
    }
    public PanelInfo(RectTransform src)
    {
        mainRect = src;
    }
    public static PanelInfo ConvertToPanel(RectTransform target, bool addFrames) //t
    {
        var info = new PanelInfo(target.AddPanel("adfufg"));
        info.mainRect.AddLayoutElementFromExisting();
        if (addFrames)
        {
            info.AddFrame();
            info.AddTopToFrame();
        }
        else
        {
            info.AddTopToMain();
        }

        return info;
    }
 public static PanelInfo ConvertToPanelNonChild(RectTransform target, bool addFrames) //t
    {
        var info = new PanelInfo(target);
    //    info.mainRect.AddLayoutElementFromExisting();
        if (addFrames)
        {
            info.AddFrame();
            info.AddTopToFrame();
        }
        else
        {
            info.AddTopToMain();
        }

        return info;
    }
    public TopInfo top;
    public RectTransform content
    {
        get { if (_content == null) return mainRect; return _content; }
        set { _content = value; }
    }

    public void SetLabel(string s, bool setname = true)
    {
        if (top != null) top.label.SetText(s);
        else
        {
            Text text = mainRect.GetComponentInChildren<Text>();
            text.SetText(s);
        }
        if (setname && mainRect != null) mainRect.name = s;
    }
    public string name
    {
        get { return mainRect.name; }
        set { mainRect.name = value; SetLabel(value); }
    }
}

[System.Serializable]
public class SliderInfo
{
    public RectTransform background;
    public Image fill;
    public Image Handle;
    public RectTransform fillArea;
    public RectTransform slideArea;
    public Slider slider;
    public Z.SliderValueDisplay sliderValueDisplay;
    public Text sliderLabel;
    public RectTransform mainRect;
    public SliderInfo(RectTransform main)
    {
        mainRect = main;
    }
    public void PadRight(float amt)
    {
        fillArea.PadRight(amt, true);
        background.PadRight(amt, true);
        slideArea.PadRight(amt, true);
    }
}