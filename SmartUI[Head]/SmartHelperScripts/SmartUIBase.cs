#define MOVE_TO_TOP // warning, blows up if added to prefab
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
// v.03 duzo nowego
namespace zUI
{
    [DisallowMultipleComponent]
    public abstract class SmartUIBase:
#if MONOBG
        MonoBehaviourWithBg
#else 
    MonoBehaviour
#endif
    {
        // public enum NaemNamingConvention { hook, hexBrackets, oval, squaredLabel, smallCurly, triLabel, curlyLabel, BTNSquaredLabel, BTNTriLabel, BTNCurlyLabel, none }
        public NameUtils.NamingConvention namingConvention;
        public static int view;
        public static bool editTogether = true;

        [SerializeField] protected bool _Interactable = true;
        public virtual bool Interactable { get; set; }
        // protected NameHelper nameHelper;
        public TextAnchor textAlignment;
        public enum SmartViews { Text, Actions, Look, Naming /* ChildHide*/ }
        public bool hideText = true;
        public bool hideImage = true;
        public bool hideButton = true;
        public int getTransformInChilrenCount;
        public LayoutElement layoutElement;
        [SerializeField] protected bool _isToggle = true;
        public Color textColor = Color.white;
        public Color imageColor = Color.white;
        protected bool wasHideText;
#if USE_TEXTCONTROL
        public int _fontSize = 14;
#endif

        public Text text { get { if (_text == null) _text = GetComponentInChildren<Text>(); return _text; } }
        public bool hideChldren; //used by editor
        [SerializeField] Text _text;
        public bool autoAdjustForText;
        public int autoAdjustMargin = 40;
        public Font font;

        protected Image image { get { if (_image == null) _image = GetComponentInChildren<Image>(); return _image; } }

        [SerializeField] Image _image;

        //public SmartColor textColor;
        //public SmartColor imageColor;
        protected RectTransform rect { get { if (_rect == null) _rect = GetComponent<RectTransform>(); return _rect; } }
        RectTransform _rect;

        [TextArea]

        [SerializeField] protected string _label;

#if USE_TEXTCONTROL
        public int fontSize
        {
            get
            {
                if (text != null) _fontSize = text.fontSize;
                return _fontSize;
            }
            set { _fontSize = value; if (text != null) text.fontSize = _fontSize; }
        }
#endif
        public string label
        {
            get
            {
                //    if (text != null)
                //        return text.text;
                return _label;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                _label = value;
                // Debug.Log ("new labels is " + value);
                if (text != null && !zBench.PrefabModeIsActive(gameObject)) // && text.text != _label)
                {

                    text.text = _label;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(text);
                    // Debug.Log("made text dirt");

#endif

                }

                HandleName();
            }
        }
        public bool applyColorsToText;
        public bool applyColorsToImage;
        public bool displayNumberOfHiddenChildren;
        public Sprite _sprite;
        protected virtual void OnEnable()
        {
            SetComponentVisibility();
        }
        void ResetBaseComponentVisibility()
        {
            if (text != null)

                text.gameObject.hideFlags = HideFlags.None;
            if (image != null)
            {
                image.hideFlags = HideFlags.None;
                image.gameObject.hideFlags = HideFlags.None;
            }
        }
        void SetBaseComponentVisibility()
        {
            if (text != null)
                text.gameObject.hideFlags = (hideText ? HideFlags.HideInHierarchy : HideFlags.None);
            if (image != null)
                image.hideFlags = (hideImage ? HideFlags.HideInInspector : HideFlags.None);
            if (!wasHideText && hideText)
            {
                wasHideText = true;
                GetTextValues();
            }
        }
        protected abstract void ResetComponentVisibility();
        protected void Cleanup()
        {
            var canvasredn = GetComponent<CanvasRenderer>();
            if (canvasredn != null) canvasredn.hideFlags = HideFlags.None;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).hideFlags = HideFlags.None;
            }
            name = name.RemoveAllTags();
        }

        void OnDestroy()
        {
            ResetComponentVisibility();
            Cleanup();
        }
        protected abstract void SetComponentVisibility();
        public Sprite sprite
        {
            get { return _sprite; }
            set { _sprite = value; if (image != null) image.sprite = _sprite; }
        }

        protected virtual void Reset()
        {

            var canvasredn = GetComponent<CanvasRenderer>();
            if (canvasredn != null) canvasredn.hideFlags = HideFlags.HideInInspector;
            if (image != null) imageColor = image.color;

            if (text == null)
            {
                var n = name.RemoveAllTags();
                if (!string.IsNullOrEmpty(n))
                    _label = n;
            }
            else
            {
                textColor = text.color;
            }
            GetTextValues();
            //  if (imageColor == null) imageColor = new SmartColor();

            // #if UNITY_EDITOR && MOVE_TO_TOP
            //             for (int i = 0; i < 4; i++)
            //             {
            //                 UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
            //             }
            // #endif
            SetChildrenHideState(true);
            if (text != null)
                label = text.text;
            OnValidate();

        }
        public void SetHidingLayoutElement(bool hide)
        {
            if (layoutElement != null)
            {
                layoutElement.hideFlags = hide ? HideFlags.HideInInspector : HideFlags.None;
            }
            isHidingLayoutElement = hide;

        }
        public bool isHidingLayoutElement;
        public bool hasLayoutElement;
        public virtual void OnValidate()
        {
            if (layoutElement == null) layoutElement = GetComponent<LayoutElement>();
            hasLayoutElement = layoutElement != null;
            if (zBench.PrefabModeIsActive(gameObject))
            {
                return;
            }
            SetBaseComponentVisibility();
            SetComponentVisibility();
            var comps = GetComponents<SmartUIBase>();
#if UNITY_EDITOR
            if (comps.Length > 1)
            {
                Debug.Log($" {name} mutiple  Cmops {comps.Length} ", gameObject);
                for (int i = comps.Length - 1; i > 0; i--)
                    UnityEditor.EditorApplication.delayCall += () => UnityEditor.Undo.DestroyObjectImmediate(comps[i]);

            }
#endif
            wasHideText = hideText;
            if (applyColorsToText)
            {
                if (text != null) text.color = textColor;
            }

            if (applyColorsToImage)
            {
                if (image != null) image.color = imageColor;
            }

            label = _label;
            if (text != null && font != null)
            {
                text.font = font;

                text.alignment = textAlignment;
#if USE_TEXTCONTROL
                text.color = textColor;
                text.fontSize = _fontSize;
#endif
            }
#if USE_IMAGES
            if (image != null)
            {
                image.color = imageColor;
                image.sprite = _sprite;
            }
#endif
            // ColorBlock newColorBlock = button.colors;
#if USE_IMAGES
            newColorBlock.normalColor = imageColor;
#endif

            LayoutRebuilder.MarkLayoutForRebuild(rect);
            HandleToggleAutoName();

        }
        protected virtual void HandleToggleAutoName()
        {
            //      if (toggleNamingConvention!=ToggleNamingConvention.none && gameObjectToToggle != null)
            //    {
            // if (gameObjectToToggle != null)
            // {
            //     _label = GetToggledName(gameObjectToToggle.name, true);
            //     //        labelWhenToggleOn =GetToggledName(gameObjectToToggle.name,false);
            // }
            //     } 
            if (text != null && !string.IsNullOrEmpty(text.text))
                //   text.text=_isOn?_label:labelWhenToggleOn; 
                label = (text.text);
        }

        protected void HandleName()
        {

            string posttag = namingConvention.ClosingBracket();
            if (hideChildren)
            {

                if (displayNumberOfHiddenChildren)
                {
                    string hideem = "";
                    if (getTransformInChilrenCount < 9)
                        hideem += NameUtils.GetChar(NameUtils.UnicodeNumberType.smalCircled, getTransformInChilrenCount);
                    else
                        hideem += "(" + getTransformInChilrenCount + ")";
                    posttag = hideem + posttag;
                }
            }
            name = label.SetPreTag(namingConvention.OpeningBracket()).SetTag(posttag);

        }
        protected void AdjustForTextSize()
        {
            if (autoAdjustForText)
            {
                minWidth = text.preferredWidth + autoAdjustMargin;
            }
        }
        protected virtual void Awake()
        {
            OnValidate();
        }

        protected void GetTextValues()
        {
            if (text != null)
            {
                _label = text.text;
                font = text.font;
                textAlignment = text.alignment;
                // if (textColor == null) textColor = new SmartColor();
#if USE_TEXTCONTROL
                textColor = text.color;

                _fontSize = text.fontSize;
#endif
            }
        }
        protected bool hideChildren;
        public bool hideChildrenRead;
        public void SetChildrenHideState(bool shouldHide)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).hideFlags = shouldHide ? HideFlags.HideInHierarchy : HideFlags.None;
            }
            hideChildren = shouldHide;
            hideChildrenRead = shouldHide;
            // Debug.Log ("has hidden? " + val, gameObject);
        }

        [SerializeField]
        bool _ignoreLayout;
        [SerializeField]
        float _width = 160;
        [SerializeField]
        float _height = 30;
        [SerializeField]
        bool _flexibleWidth = true;
        [SerializeField]
        bool _flexibleHeight = false;
        public bool ignoreLayout { get { return _ignoreLayout; } set { _ignoreLayout = value; } }

        public float minWidth { get { return _width; } set { _width = value; rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _width); } }
        public float minHeight { get { return _height; } set { _height = value; rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _height); } }
        public float flexibleHeight { get { return _flexibleHeight ? 1 : 0; } set { _flexibleHeight = value != 0; } }
        public float flexibleWidth { get { return _flexibleWidth ? 1 : 0; } set { _flexibleWidth = value != 0; } }
        public int layoutPriority { get; set; }
        public float preferredWidth { get; set; }
        public float preferredHeight { get; set; }
        public void CalculateLayoutInputHorizontal() {}
        public void CalculateLayoutInputVertical() {}

    }
}