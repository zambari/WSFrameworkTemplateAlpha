using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using zUI;

namespace Z.LayoutPanel
{

    [RequireComponent(typeof(LayoutElement))]
    [ExecuteInEditMode]
    public class LayoutBorderDragger : MonoBehaviourWithBg,
        IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler,
        IDropTarget, IBorderControlListener
        {
            public bool individualSettings;
            public bool targetGroupIfPossible = true;
            public bool columnMode;
            public static string baseName { get { return "[Panel]"; } }
            LayoutPanel panel;

            // public bool target
            //[HideInInspector] 
            public VerticalLayoutGroup groupToDisableWhenDragging;
            [SerializeField]
            protected Texture2D hoverCursor;
            [SerializeField] Side _side;
            [HideInInspector]
            [SerializeField] Side _lastside;
            LayoutFoldController foldController;
            [SerializeField] public bool bordersPlacedInside;
            RectTransform rect { get { if (_rect == null) _rect = GetComponent<RectTransform>(); return _rect; } }
            RectTransform _rect;
            Image image { get { if (_image == null) _image = GetComponent<Image>(); return _image; } }
            Image _image;
            public bool isHorizontal { get { return side == Side.Left || side == Side.Right; } }
            public static Vector2 cursorCenter { get { return Vector2.one * 16; } }
            public bool enableSetSize = true;

            bool isFolded;
            RectTransform targetRect;
            public Image enableOnHover;
            public ColumnController column;

            bool restorePivotAfterMove = true;
            Vector2 savedPivot;
            public LayoutElement _elementToResize;
            public LayoutElement elementToResize
            {
                get
                {
                    if (_elementToResize == null)
                    {
                        if (panel == null) panel = GetComponentInParent<LayoutPanel>();
                        if (panel == null) return null;
                        _elementToResize = panel.GetTargetElementForSide(side);
                    }

                    return _elementToResize;
                }
                set
                {
                    _elementToResize = value;
                }
            }
            LayoutSetup _setup;
            LayoutSetup setup
            {
                get
                {
                    if (_setup == null)
                    {
                        //if (Lay)
                        _setup = LayoutBorderControl.setup;
                    }
                    return _setup;
                }
            }
            public bool enableDrag
            {
                get
                {
                    if (isFolded) return false;
                    // if (elementToResize == null) return false;
                    if (panel == null) panel = GetComponentInParent<LayoutPanel>();
                    if (panel != null)
                    {
                        if (panel.detachedMode) return true;
                        // if (side == Side.Top) return panel.isAlignedBottom;
                        // if (side == Side.Bottom) return !panel.isAlignedBottom;
                    }
                    return true;
                }
            }
            public bool freeResizeMode
            {
                get
                {
                    if (panel == null) return true; // false; // was true
                    return panel.detachedMode;
                }
            }
            public bool isPanelHorizontal
            {
                get
                {
                    if (panel == null) return false;
                    return panel.isInHorizontalGroup;
                }
            }

            public void UpdateLayoutSetupObject(LayoutSetup setup)
            {
                if (individualSettings) return;
                _setup = setup;
                if (setup == null || setup.borderSetup == null) 
                {
                    Debug.Log("no borders eup");
                     return;
                }
                SetColor(setup.borderSetup.normalColor);
                setup.borderSetup.ApplyRectSettings(rect, side);
            }
            public Transform dropTarget
            {
                get
                {
                    if (panel == null) return transform.transform.parent;
                    return panel.GetTargetTransformForSide(side);
                }
            }

            void Start()
            {
                if (column == null)
                    column = GetComponentInParent<ColumnController>();
                if (targetGroupIfPossible && column != null)
                {
                    _elementToResize = column.targetLayoutElement;
                    //   var newe = column.GetComponentInChildren<LayoutElement>();

                    if (_elementToResize == null)
                    {
                        Debug.Log("not assigne new element ");
                    }
                    else
                    {
                        // Debug.Log("assigne new element _elementToResize " + _elementToResize.name, _elementToResize);
                        //   _elementToResize = newe;
                    }
                }
                ScrollRect scroll = transform.parent.GetComponentInChildren<ScrollRect>();
                if (scroll != null)
                {
                    if (scroll.content != null)
                    {
                        VerticalLayoutGroup vertical = scroll.content.GetComponentInChildren<VerticalLayoutGroup>();
                        if (vertical != null) groupToDisableWhenDragging = vertical;
                    }
                }
            }
            // public bool extendToFillCorners;

            public Side side
            {
                get { return _side; }
                set
                {
                    // if (_side == value) return;
                    _side = value;
                    if (setup != null)
                        setup.borderSetup.ApplyRectSettings(rect, side);
                    GetCursor();
                }
            }
            void SetName()
            {
                string sidestring = side.GetObjectLabel();
                if (zGizmos.PrefabModeIsActive(gameObject)) return;
                if (!name.Equals(sidestring))
                {
                    name = side.GetObjectLabel();
                }
            }
            public int targetDropIndex
            {
                get
                {
                    if (columnMode)
                    {
                        if (side == Side.Top)
                            return 0;
                        else
                            return -1;
                    }
                    var thissib = panel.transform.GetSiblingIndex();
                    if (side == Side.Bottom) thissib++;
                    return thissib;
                }
            }
            public bool isHorizontalBar { get { return isHorizontal; } }

            void GetCursor()
            {
                if (isHorizontal)
                    hoverCursor = zResourceLoader.horizontalCursor;
                else
                    hoverCursor = zResourceLoader.vertialCursor;
            }
            public LayoutElement layoutElement { get { if (_layoutElement == null) _layoutElement = GetComponent<LayoutElement>(); return _layoutElement; } }
            private LayoutElement _layoutElement;
            public void OnValidate()
            {
                if (zBench.PrefabModeIsActive(gameObject)) return;
                layoutElement.ignoreLayout = true;
                GetCursor();
                GetTargets();
                if (_side != _lastside)
                {
                    side = _side;
                    _lastside = side;
                    SetName();
                }
#if PALETTES
                //  var colorsync = gameObject.AddOrGetComponent<ColorSync>();
                //   if (string.IsNullOrEmpty(colorsync.presetName)) colorsync.presetName = "Borders"; // sorry for hardcode
#endif
                // if (!LayoutBorderControl.setup.bordersPlacedInside)
                //     extendToFillCorners = !side.isHorizontal(); // will fail on inner side
                // else
                //     extendToFillCorners = false;
            }

            void GetTargets()
            {
                //      name = side.GetObjectLabel();
                SetName();
                if (panel == null)
                    panel = GetComponentInParent<LayoutPanel>();
                //   enableDrag = (elementToResize != null);
            }
            public bool takeColorFromBorderHide;
            void OnEnable()
            {
                LayoutBorderControl.Subscribe(this);

                // LayoutSettings.onBorderSizeChange += SetSize;
                // SetSize(LayoutBorderControl.setup);
                GetTargets();
                foldController = GetComponentInParent<LayoutFoldController>();
                if (foldController != null)
                {
                    foldController.onFold += OnFoldToggle;
                    OnFoldToggle(foldController.isFolded);
                }
                // var vh = GetComponentInParent<LayoutBorderHide>();
                // if (vh != null)
                // {
                SetColor(setup.borderSetup.normalColor);
                // }
            }

            void OnFoldToggle(bool b)
            {
                isFolded = b;
                side = side;
            }
            void OnDisable()
            {
                LayoutBorderControl.UnsSubscribe(this);
                // LayoutSettings.onBorderSizeChange -= SetSize;
                if (foldController != null)
                    foldController.onFold -= OnFoldToggle;
            }
            bool isDragging;
            void ResizeLayoutElement(LayoutElement e)
            {

            }
            void ResizeRect(RectTransform rect)

            {

            }
            public void OnBeginDrag(PointerEventData eventData)
            {
                if (!enableDrag)
                {
                    Debug.Log("drag not enabled");
                    return;
                }
                isDragging = true;
                SetImageEnable(true);
                if (targetGroupIfPossible)
                {
                    var g = GetComponentsInParent<ColumnController>();
                    foreach (var ggg in g)
                    {
                        Debug.Log("found component " + ggg.name, ggg.gameObject);
                        if (ggg.group == ColumnController.GroupPick.column)
                        {
                            _elementToResize = ggg.targetLayoutElement;
                        }
                    }

                    // var le = g.GetComponent<LayoutElement>();
                    // if (le != null)
                    // {
                    //     _elementToResize = le;
                    //     Debug.Log("assigned");
                    //     // ResizeLayoutElement(le);
                    //     // return;
                    // }

                }
                else { Debug.Log("no grouypt"); }
                if (groupToDisableWhenDragging != null) groupToDisableWhenDragging.enabled = false;
                if (hoverCursor == null) GetCursor();
                if (freeResizeMode)
                {
                    targetRect = transform.parent.GetComponent<RectTransform>();
                    savedPivot = targetRect.pivot;
                }
                else
                {
                    if (_elementToResize == null)
                    {
                        Debug.Log("did not find elementToResize");
                        return;
                    }

                    if (isHorizontal)
                    {
                        if (elementToResize.preferredWidth == -1)
                            elementToResize.preferredWidth = elementToResize.GetComponent<RectTransform>().rect.width / 3; //hahha
                    }
                    else
                    {
                        if (elementToResize.preferredHeight == -1)
                            elementToResize.preferredHeight = elementToResize.GetComponent<RectTransform>().rect.height / 3; //hahha

                    }
                }
            }
            static Vector3[] corners = new Vector3[4];
            float GetActualWidth(RectTransform rect)
            {
                rect.GetWorldCorners(corners);
                return Mathf.Abs(corners[0].x - corners[2].x);

            }
            float GetActualHeight(RectTransform rect)
            {
                rect.GetWorldCorners(corners);
                return Mathf.Abs(corners[0].y - corners[1].y);

            }
            public void OnEndDrag(PointerEventData eventData)
            {
                if (!setup.borderSetup.GetDragEnabled(side)) return;
                if (groupToDisableWhenDragging != null) groupToDisableWhenDragging.enabled = true;
                if (targetRect != null && restorePivotAfterMove)
                    targetRect.SetPivot(savedPivot);
                isDragging = false;
                SetImageEnable(false);
            }
            public void OnDrag(PointerEventData thisEventData)
            {
                if (!enableDrag)
                {
                    Debug.Log("drag note enabled");
                    return;
                }
                if (!setup.borderSetup.GetDragEnabled(side)) return;
                Vector2 delta = thisEventData.delta;

                if (elementToResize == null && !freeResizeMode)
                {
                    Debug.Log(" sorry, no target element "+name, gameObject);
                    return;
                }
                if (elementToResize != null || !freeResizeMode)
                {
                    delta = side.SideDelta(delta);
                    // Debug.Log($"nonfreedrag {side.GetPivot()} {delta}");
                    elementToResize.preferredWidth += delta.x;
                    elementToResize.preferredHeight += delta.y;
                    if (elementToResize.preferredWidth <= 1) elementToResize.preferredWidth = 32;
                }
                else
                {
                    // Debug.Log("freedrag "+side.GetPivot()+" "+delta);
                    targetRect.SetPivot(side.GetPivot());
                    targetRect.sizeDelta = targetRect.sizeDelta + side.SideDelta(delta);

                    // while (targetRect.rect.width < LayoutSettings.minRectWidth)
                    //     targetRect.sizeDelta += Vector2.right * 5;
                    // while (targetRect.rect.height < LayoutSettings.minRectHeight)
                    //     targetRect.sizeDelta += Vector2.up * 5;
                }
                // Debug.Log($"target { elementToResize.preferredWidth} { elementToResize.preferredHeight}");
            }
            void SetColor(Color c)
            {
                if (image.color.Equals(c)) return;
                image.color = c;
            }
            public void OnPointerEnter(PointerEventData eventData)
            {
                if (!setup.borderSetup.GetDragEnabled(side)) return;
                //      Debug.Log("pointerneterd " + name, gameObject);
                if (enableDrag && ((!columnMode && elementToResize != null) || (panel != null && panel.freeMoveMode))) //wtf
                    Cursor.SetCursor(hoverCursor, cursorCenter, CursorMode.Auto);
                //            neutralColor = image.color;
                //          SetColor(new Color(neutralColor.r, neutralColor.g, neutralColor.b, neutralColor.a * alphaMultiplier));
                SetColor(setup.borderSetup.hoveredColor);
                if (LayoutTopControl.draggedItem != null && LayoutTopControl.draggedItem.transform.parent != transform.parent)
                {
                    // if (panel!=null)
                    // if (panel.isInHorizontalGroup && isHorizontal)
                    // {

                    // }
                    // if (panel.isInVerticalGroup)
                    // if (!isHorizontal)
                    {
                        LayoutDropTarget.currentTargetObject = gameObject;
                        if (panel != null)
                        {
                            if ((panel.isInHorizontalGroup && !isHorizontal) ||
                                (panel.isInVerticalGroup && isHorizontal))
                                SetColor(setup.borderSetup.dropTargetColorWhenSplit);
                            else
                                SetColor(setup.borderSetup.dropTargetColor);
                        }
                        else
                        {
                            Debug.Log("no panel");
                        }
                    }
                }
                // if (setup != null)
                //     SetSize(setup);
                SetImageEnable(true);
            }

            void SetImageEnable(bool v)
            {
                if (enableOnHover != null)
                {
                    var mask = enableOnHover.GetComponent<Mask>();
                    enableOnHover.enabled = v;
                    if (mask != null) mask.enabled = !v;
                }
            }
            public void OnPointerExit(PointerEventData eventData)
            {
                if (!setup.borderSetup.GetDragEnabled(side)) return;
                if (LayoutTopControl.draggedItem != null && LayoutDropTarget.currentTargetObject == gameObject)
                    LayoutDropTarget.currentTargetObject = null;
                SetColor(setup.borderSetup.normalColor);
                // if (setup != null)
                //     SetSize(setup);
                if (!isDragging)
                    SetImageEnable(false);
                Cursor.SetCursor(null, cursorCenter, CursorMode.Auto);
            }
#if UNITY_EDITOR
            [ExposeMethodInEditor]
            void ManageVisibility()
            {
                LayoutBorderHide bh = transform.parent.gameObject.AddOrGetComponent<LayoutBorderHide>();
                UnityEditor.Selection.activeGameObject = bh.gameObject;
            }
#endif

        }

}