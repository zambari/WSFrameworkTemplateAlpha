using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.UI;
namespace Z.LayoutPanel
{
    [System.Serializable]
    public class LayoutGroupSettings
    {
        public bool useSettings = true;
        //  public RectOffset offsets;
        public float spacing = 5;
        public bool sharedSpacing = true;

        public RectOffset padding = new RectOffset();
        public int spacingTimesBorder = 1;
        public void ApplyTo(VerticalLayoutGroup group, LayoutSetup setup)
        {
            if (!useSettings) return;
            if (group != null)
            {
                group.padding = padding;
                group.spacing = spacing + setup.borderSetup.GetSize(Side.Top) + setup.borderSetup.GetSize(Side.Bottom);
//                Debug.Log($" detting settngs  {setup.borderSetup.GetSize(Side.Top)}setup.borderSetup.GetSize(Side.Bottom) { setup.borderSetup.GetSize(Side.Top)} ");
                // Debug.Log("offsets" + paddings.top + " applyyinh " + group.name + " ofs :" + paddings.left + " pd " + paddings);
            }
            //             if (useContentSizeFitters)
            //             {
            //                 var contentfitter = group.gameObject.AddOrGetComponent<ContentSizeFitter>();
            //                 contentfitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            //                 var le = group.gameObject.GetComponent<LayoutElement>();
            //                 if (le != null)
            //                 {
            // #if UNITY_EDITOR
            //                     UnityEditor.EditorApplication.delayCall += () => GameObject.DestroyImmediate(le);

            // #else
            //                     GameObject.DestroyImmediate(le);
            // #endif
            //                 }
            //             }
            //             else
            //             {
            //                 var le = group.gameObject.AddOrGetComponent<LayoutElement>();
            //             }
        }
        public void ApplyTo(HorizontalLayoutGroup group, LayoutSetup setup)
        {
            if (!useSettings) return;
            if (group != null)
            {
                group.padding = padding;
                group.spacing = spacing + setup.borderSetup.borderSizeH * spacingTimesBorder;
                // Debug.Log("offsets" + paddings.top + " applyyinh " + group.name + " ofs :" + paddings.left + " pd " + paddings);
            }
            // if (group != null)
            // {
            //     //   offsets = new RectOffset(offsetLeft, offsetRight, offsetTop, offsetBottom);
            //     //   group.padding = offsets;
            //     var paddings = group.padding;
            //     paddings.left = offsetLeft;
            //     paddings.right = offsetRight;
            //     paddings.top = offsetTop;
            //     paddings.bottom = offsetBottom;
            //     group.padding = paddings;
            //     //RectOffset offset=group.
            //     if (sharedSpacing) group.spacing = spacing;
            //     // Debug.Log("offsets" + paddings.top + " applyyinh " + group.name + " ofs :" + paddings.left + " pd " + paddings);
            // }
            //             if (useContentSizeFitters)
            //             {
            //                 var contentfitter = group.gameObject.AddOrGetComponent<ContentSizeFitter>();
            //                contentfitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            //                 var le = group.gameObject.GetComponent<LayoutElement>();
            //                 if (le != null)
            //                 {
            // #if UNITY_EDITOR
            //                     UnityEditor.EditorApplication.delayCall += () => GameObject.DestroyImmediate(le);

            // #else
            //                     GameObject.DestroyImmediate(le);
            // #endif
            //                 }
            //             }
            //             else
            //             {
            //                 var le = group.gameObject.AddOrGetComponent<LayoutElement>();
            //             }
        }
    }
}