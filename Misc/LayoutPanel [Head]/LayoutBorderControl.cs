using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
using Z.LayoutPanel;
[ExecuteInEditMode]
public class LayoutBorderControl : MonoBehaviour, IRequestInitEarly
{
    static LayoutBorderControl instance;
    static List<IBorderControlListener> borderDraggers;
    public int borderSize
    {
        get { return setup.borderSize; }
        set
        {
            setup.borderSize = value;
            OnValidate();
        }
    }
    public bool inerseqr;
    float lastSet = -1;
    public bool topEnabled
    {
        get { return setup.borderSetup.topBorder; }
        set
        {
            setup.borderSetup.topBorder = value;
            OnValidate();
        }
    }
    public float borderAlphaHovered { get { return setup.borderSetup.hoveredAlpha; } set { setup.borderSetup.hoveredAlpha = value; OnValidate(); } }
    public float borderAlphaNormal { get { return setup.borderSetup.normalAlpha; } set { setup.borderSetup.normalAlpha = value; OnValidate(); } }

    public float borderR { get { return setup.borderSetup.baseColor.r; } set { setup.borderSetup.baseColor = setup.borderSetup.baseColor.SetR(value); OnValidate(); } }

    public float borderG { get { return setup.borderSetup.baseColor.g; } set { setup.borderSetup.baseColor = setup.borderSetup.baseColor.SetG(value); OnValidate(); } }

    public float borderB { get { return setup.borderSetup.baseColor.b; } set { setup.borderSetup.baseColor = setup.borderSetup.baseColor.SetB(value); OnValidate(); } }
    public float spacingFloat
    {
        get
        {
            return setup.borderSize;
        }
        set
        {
            setup.borderSize = (int)(value);
            OnValidate();
        }

    }
    public float horizonttaldistance
    {
        get
        {
            return setup.panelSpacingOffserHoriz;
        }
        set
        {
            setup.panelSpacingOffserHoriz = (int)(value);
            OnValidate();
        }

    }
    public float distanceBorders
    {
        get
        {
            return setup.borderSetup.offsetSide;
        }
        set
        {
            setup.borderSetup.offsetSide = (int)value;
            OnValidate();
        }

    }
    public float borderSizeFloat
    {
        get
        {
            if (lastSet == -1)
                return setup.borderSetup.borderSize / 30f;

            return lastSet;
        }
        set
        {
            lastSet = value;
            if (inerseqr) value = value.InversedSquare();
            setup.borderSetup.borderSize = (int)(value * 30);
            OnValidate();
        }
    }
    public static void BroadcastSetup(LayoutSetup thisSetup)
    {
        if (borderDraggers != null)
            for (int i = borderDraggers.Count - 1; i >= 0; i--)
            {
                if (borderDraggers[i] == null) borderDraggers.RemoveAt(i);
                borderDraggers[i].UpdateLayoutSetupObject(thisSetup);
            }
        else
        {
            Debug.LogWarningFormat("borderDraggersnull");
        }
    }
    public static void Subscribe(IBorderControlListener source)
    {
        if (borderDraggers == null)
            borderDraggers = new List<IBorderControlListener>();
        if (!borderDraggers.Contains(source)) borderDraggers.Add(source);
        source.UpdateLayoutSetupObject(setup);
    }
    public static void UnsSubscribe(IBorderControlListener source)
    {
        if (borderDraggers == null) borderDraggers = new List<IBorderControlListener>();
        if (borderDraggers.Contains(source))
            borderDraggers.Remove(source);

    }
    public static bool isReady { get { return instance != null; } }
    public static LayoutSetup setup
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LayoutBorderControl>();
                if (instance == null)
                {
                    return null;
                }
            }
            return instance._setup;
        }
    }

    [SerializeField] LayoutSetup _setup; // = new BorderColors();
    void SetupSignleton()
    {
        if (instance != null && instance != this)
        {
            if (!zBench.PrefabModeIsActive(gameObject)) return;
#if UNITY_EDITOR
            // if (GetComponents.Count<)
            // Debug.Log($"Componetn count {GetComponents<Component>().Length} ", gameObject);
            _setup = instance._setup;
            UnityEditor.Undo.DestroyObjectImmediate(instance);

#endif
            return;
        }
        else
            instance = this;
    }
    private void OnEnable()
    {
        SetupSignleton();

    }
    private void Awake()
    {
        SetupSignleton();
        if (setup == null)
        {
            var setups = Resources.FindObjectsOfTypeAll(typeof(LayoutSetup)) as LayoutSetup[];
            if (setups.Length > 0) _setup = setups[0];
        }

    }
    public void Init(MonoBehaviour awakenSource)
    {
        SetupSignleton();
    }

    private void OnValidate()
    {
        if (zBench.PrefabModeIsActive(gameObject)) return;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += BroadcastEvents;
#else
		BroadcastEvents();
#endif

    }

    [ExposeMethodInEditor]
    void BroadcastEvents()

    {
        BroadcastSetup(setup);

    }
}
public interface IBorderControlListener
{
    void UpdateLayoutSetupObject(LayoutSetup setup);
}
// [System.Serializable]
// public class BorderColors
// {
// 	public Color baseColor = Color.gray;

// 	[Range(0, 15)]
// 	public int borderSizeH = 3;
// 	[Range(0, 15)]
// 	public int borderSizeV = 3;
// 	public bool extendToFillCornersHorizontal;
// 	public bool extendToFillCornersVertical;
// 	public bool bordersPlacedInside;
// 	public int topHeight = 15;
// 	public void OnValidate()
// 	{
// 		baseColor.a = 1;

// 	}
// 	public Color normalColor
// 	{
// 		get
// 		{
// 			Color color = baseColor;
// 			color.a = normalAlpha;
// 			return color;
// 		}
// 	}
// 	public Color hoveredColor
// 	{
// 		get
// 		{
// 			Color color = baseColor;
// 			color.a = hoveredAlpha;
// 			return color;
// 		}
// 	}
// 	public void ApplyRectSettings(RectTransform target, Side side)
// 	{
// 		Vector3 newAnchoredPosition = Vector3.zero;
// 		target.anchorMin = side.GetAnchorMin();
// 		target.anchorMax = side.GetAnchorMax();
// 		target.pivot = side.GetPivot(LayoutBorderControl.setup.bordersPlacedInside);
// 		target.anchoredPosition = Vector2.zero;
// 		target.sizeDelta = BorderDeltaSize(side);
// 		// if (columnMode)
// 		// {
// 		//     if (_side == Side.Top)
// 		//         newAnchoredPosition += new Vector3(0, LayoutSettings.borderSizeColumnOffset);
// 		//     if (_side == Side.Bottom)
// 		//         newAnchoredPosition += new Vector3(0, -LayoutSettings.borderSizeColumnOffset);
// 		//     rect.anchoredPosition = newAnchoredPosition;

// 		// }
// 	}

// 	public Vector2 BorderDeltaSize(Side side)
// 	{

// 		var newSize = Vector2.zero;
// 		if (side.isHorizontal())
// 		{
// 			newSize = new Vector2(borderSizeH, 0);
// 		}
// 		else
// 		{
// 			newSize = new Vector2(0, borderSizeV);
// 		}

// 		if (side.isHorizontal())
// 		{
// 			if (extendToFillCornersHorizontal)
// 				newSize += new Vector2(0, 2 * borderSizeH);
// 		}
// 		else
// 		{
// 			if (extendToFillCornersVertical)
// 				newSize += new Vector2(2 * borderSizeV, 0);
// 		}
// 		return newSize;
// 	}

// 	[Range(0, 1)]
// 	public float normalAlpha = .5f;
// 	[Range(0, 1)]
// 	public float hoveredAlpha = .8f;
// 	public Color dropTargetColor = new Color(0.2f, 1, 0.2f, 0.8f);
// 	public Color dropTargetColorWhenSplit = new Color(.7f, 0.2f, 0.7f, 0.8f);
// }