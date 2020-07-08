using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{
    public class UIBuilderTester : MonoBehaviour
    {
        public Transform target { get { if (_target == null) _target = transform; return _target; } set { _target = value; } }
        public Transform _target;


        // [ExposeMethodInEditor]

        // void BuildHierarchyPanelWithFrames()
        // {
        //     target = UIBuilder.BuildScrollPanelWithFrames(PanelInfo.FromRect(target), "HR").content;
        //     for (int i = 0; i < 4; i++)
        //     {
        //         var scenePanel = UIBuilder.BuildBasicPanelTopOnly(target, "HR", false);
        //         for (int h = 0; h < 4; h++)
        //         {
        //             var sub = UIBuilder.BuildBasicPanelTopOnly(scenePanel.mainRect, "bt", false);
        //         }
        //     }
        //     //SetSelectnew(target);
        // }

        // [ExposeMethodInEditor]

        // void BuildScrollcPanelWithFrames()
        // {
        //     PanelInfo info = new PanelInfo(target);
        //     target = UIBuilder.AddFrameAndTop(info, "SCR").mainRect;
        //     //SetSelectnew(target);
        // }

        // [ExposeMethodInEditor]

        // void BuildInput()
        // {
        //     // PanelInfo info=new PanelInfo(target);
        //     //target =
        //     UIBuilder.BuildInputField(target, "SCR");

        //     //SetSelectnew(target);
        // }

        // [ExposeMethodInEditor]
        // void BuildBasicPanelWithFrames()
        // {
        //     target = UIBuilder.AddFrameAndTop(target, "basci").mainRect;
        //     //SetSelectnew(target);
        // }

//         [ExposeMethodInEditor]

//         void BuildBasicPanelTopOnly()
//         {
//             target = UIBuilder.BuildBasicPanelTopOnly(target, "bas").mainRect;
//             //SetSelectnew(target);
//         }
//         void SetSelectnew(Transform what)
//         {
// #if UNITY_EDITOR
//             UnityEditor.Selection.activeGameObject = what.gameObject;
// #endif
//         }

        // [ExposeMethodInEditor]
        // void BuildScrollbar()
        // {
        //     UIBuilder.BuildScrollBar(target);
        // }

        // [ExposeMethodInEditor]
        // void BuildButtons()
        // {
        //     UIBuilder.BuildButton(target);
        // }

        // [ExposeMethodInEditor]
        // void BuildSlider()
        // {
        //     UIBuilder.BuildSlider(target, "Slider");
        // }

        // [ExposeMethodInEditor]

        // void AddTop()
        // {
        //     //target.RemoveChildren();
        //     UIBuilder.AddTop(target);
        // }

        // [ExposeMethodInEditor]

        // void BuildScrollRect()
        // {
        //     //	target.RemoveChildren();
        //     var Pan
        //     var scrolcontent = UIBuilder.BuildScrollRectWithConentSizeFitter(target);
        //     for (int i = 0; i < 10; i++)
        //     {
        //         UIBuilder.BuildButton(scrolcontent);
        //     }

        // }

    }
}