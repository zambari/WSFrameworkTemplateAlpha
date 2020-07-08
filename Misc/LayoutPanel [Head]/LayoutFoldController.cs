using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.UI;
using zUI;
namespace Z.LayoutPanel
{
    //  [RequireComponent(typeof(VerticalLayoutGroup))]
    //   [RequireComponent(typeof(LayoutElement))]
    public class LayoutFoldController : SimpleFoldController
    {

        // [SerializeField] [HideInInspector] float savedPreferredHeight = -1;
        // [SerializeField] [HideInInspector] float savedFlexibleHeight = -1;
        [SerializeField]

        public bool ignoreSavedKeepDisabledList = true;
        public Text foldLabelText
        {
            get { return _foldLabelText; }
            set
            {
                _foldLabelText = value;
#if UNITY_EDITOR
                UnityEditor.EditorApplication.delayCall += () => _foldLabelText.SetText(GetFoldString());
#else
          SetFoldLabel();
#endif
            }
        }

        public bool autoStoreactive;

      


        // protected override void OnValidate()
        // {
        //     base.OnValidate();
        //     // if (_foldLabelText == null && foldButton != null) _foldLabelText = foldButton. GetComponentInChildren<Text>();
        //     foldLabelText.SetText(GetFoldString());
        //     // UpdateSize();
        // // }
        // void UpdateSize()
        // {
        //     if (layoutElement != null)
        //     {
        //         layoutElement.flexibleHeight = -1;
        //         layoutElement.minHeight = LayoutSettings.topHeight;
        //     }
        // }
        // void OnEnable()
        // {
        //     LayoutSettings.onBorderSizeChange += UpdateSize;
        // }
        // void OnDisable()
        // {
        //     LayoutSettings.onBorderSizeChange -= UpdateSize;
        // }


        // IEnumerator StoreActiveObjects()
        // {
        //     objectsToIgnore = new List<GameObject>();
        //     int objectsPerFrame = GetWaitAfterNObjects();
        //     for (int i = 0; i < transform.childCount; i++)
        //     {
        //         var thisChild = transform.GetChild(i).gameObject;
        //         if (DisableObjectCondition(thisChild))
        //         {
        //             if (!thisChild.activeSelf && autoStoreactive)
        //                 objectsToIgnore.Add(thisChild);
        //             if (!objectsToIgnore.Contains(thisChild))
        //                 thisChild.SetActive(false);
        //             if (Application.isPlaying && i > 0 && i % objectsPerFrame == 0)
        //             {
        //                 yield return null;
        //             }
        //         }
        //     }
        //     yield break;
        // }
        int GetWaitAfterNObjects()
        {
            int objectsPerFrame = transform.childCount / 5;
            if (objectsPerFrame < 1)
                objectsPerFrame = 1;
            return objectsPerFrame;
        }

        IEnumerator RestoreActiveObject()
        {
            if (objectsToIgnore == null) objectsToIgnore = new List<GameObject>(); //activeDict = new Dictionary<GameObject, bool>();
            int objectsPerFrame = GetWaitAfterNObjects();

            for (int i = 0; i < transform.childCount; i++)
            {

                var thisChild = transform.GetChild(i).gameObject;
                if (DisableObjectCondition(thisChild))
                {
                    if (ignoreSavedKeepDisabledList || !objectsToIgnore.Contains(thisChild))

                        thisChild.SetActive(true);
                    if (Application.isPlaying && i > 0 && i % objectsPerFrame == 0)
                    {
                        yield return null;
                    }
                    else
                    {
                        // Debug.Log("going forward ");

                    }
                }
            }
            yield break;
        }

        //     IEnumerator Fold()
        //     {
        //         if (isAnimating || isFolded) yield break;
        //         isAnimating = true;
        //         if (layoutElement != null)
        //         {
        //             savedPreferredHeight = layoutElement.preferredHeight;
        //             savedFlexibleHeight = layoutElement.flexibleHeight;
        //         }


        //         StartCoroutine(StoreActiveObjects());

        //         if (layoutElement != null)
        //         {
        //             layoutElement.preferredHeight = -1;
        //             layoutElement.flexibleHeight = -1;
        //         }
        //         isFolded = true;
        //         if (onFold != null) onFold(true);
        //         isAnimating = false;
        //     }

        //     IEnumerator UnFold()
        //     {
        //         if (isAnimating || !isFolded) yield break;
        //         isAnimating = true;
        //         StartCoroutine(RestoreActiveObject());
        //         if (layoutElement != null)
        //         {
        //             layoutElement.preferredHeight = savedPreferredHeight;
        //             layoutElement.flexibleHeight = savedFlexibleHeight;
        //         }
        //         isAnimating = false;
        //         if (onFold != null) onFold(false);
        //         isFolded = false;
        //     }
    }

}