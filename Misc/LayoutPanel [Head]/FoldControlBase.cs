using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
public class FoldControlBase : MonoBehaviour
{
    [Header("will use parent if null")]
    public Transform foldRoot;
    public Button foldButton;
    public Text _foldLabelText;
    protected bool locked;
    public static string labelUnfolded { get { return "▼"; } }
    public static string labelFolded { get { return "◀"; } }
    public static string labelFoldedAlt { get { return "▶"; } }
    public List<GameObject> objectsToIgnore = new List<GameObject>();
    public System.Action<bool> onFold;
    public List<GameObject> objectsToToggle;

    public bool isFolded
    {
        get { return _isFolded; }
        protected set
        {
            _isFolded = value;
            if (onFold != null)
            {
                try
                {
                    onFold.Invoke(value);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Exception while signaling fold "+e.Message);
                }
            }

        }
    }
    public bool _isFolded;
    bool isLeftSide = false;
    public bool foldChildren;
    // public bool foldParent = true;

    public bool startFolded;
    protected void NaiveFold(bool shouldHide)
    {
        GetObjectsToToggle();
        if (objectsToToggle.Count == 0)
        {
            Debug.Log("no objects", gameObject);
        }
        foreach (var o in objectsToToggle)
        {
            o.SetActive(!shouldHide);
        }

        isFolded = shouldHide;
    }
    protected void GetObjectsToToggle()
    {
        objectsToToggle = new List<GameObject>();
        if (transform.parent == null) return;
        if (foldRoot == null) foldRoot = GetRoot();
        for (int i = 0; i < foldRoot.childCount; i++)
        {
            var thischild = foldRoot.GetChild(i);
            if (thischild == transform) continue;
            // var foldparent = foldButton.transform.parent;
            // if (thischild == foldparent) continue;
            // if (thischild == foldparent.parent) continue;
            // if (objectsToIgnore.Contains(thischild)) continue;
            //      if (thischild.GetComponent<LayoutTopControl>() != null) continue;
            var le = thischild.GetComponent<LayoutElement>();
            if (le != null && le.ignoreLayout) continue;

            // if (thischild.GetComponent<ScrollRect>()!=null) continue;
            //            if (thischild.GetComponent<ScrollPooled>()!=null) continue;
            objectsToToggle.Add(thischild.gameObject);
        }
        Debug.Log("found " + objectsToToggle.Count);
    }

    protected void GetIgnoredObjects()
    {
        if (!objectsToIgnore.Contains(gameObject)) objectsToIgnore.Add(gameObject);
        // if (foldButton != null)
        // {
        //     var parent = foldButton.transform.parent;
        //     while (parent != transform && parent != null)
        //     {
        //         if (objectsToIgnore.Contains(parent.gameObject)) break;
        //         objectsToIgnore.Add(parent.gameObject);
        //         parent = parent.parent;
        //     }
        //        Debug.Log($"added {objectsToIgnore.Count}",gameObject);
        // }
        //   else Debug.Log("no button",gameObject);
    }
    protected void SetFoldLabel()
    {
        _foldLabelText.SetText(GetFoldString());
    }
    public string GetFoldString()
    {
        if (isFolded)
            return isLeftSide ? labelFoldedAlt : labelFolded; //▲ ▶ ◀ ▼
        else
            return labelUnfolded;
    }

    protected virtual void Start()
    {
        if (foldButton != null) foldButton.onClick.AddListener(ToggleFold);
        if (foldRoot == null) foldRoot = GetRoot();
        GetIgnoredObjects();
        if (startFolded)
            Fold();
        SetFoldLabel();
    }

    protected virtual void OnValidate()
    {
        if (!zBench.PrefabModeIsActive(gameObject))
            if (foldRoot == null) foldRoot = GetRoot();

        //    if (foldButton == null && transform.childCount > 0) foldButton = transform.GetChild(0).GetComponentInChildren<Button>();
        //   if (_foldLabelText == null && foldButton != null) _foldLabelText = foldButton.GetComponentInChildren<Text>();
        // if (_foldLabelText != null) _foldLabelText.text = GetFoldString();
        //GetIgnoredObjects();
    }
    protected virtual void Reset()
    {
        if (foldRoot == null) foldRoot = transform.parent;

        if (foldButton == null && transform.childCount > 0) foldButton = transform.GetChild(0).GetComponentInChildren<Button>();
        if (_foldLabelText == null && foldButton != null) _foldLabelText = foldButton.GetComponentInChildren<Text>();
        if (_foldLabelText != null) _foldLabelText.text = GetFoldString();
        GetIgnoredObjects();
    }
    protected bool CanStartCoroutine()
    {
        if (!isActiveAndEnabled) return false;
        return (Application.isPlaying);
    }
    protected Transform GetRoot()
    {
        //if (foldParent) 
        if (foldRoot != null) return foldRoot;
        if (transform.parent.name.Contains("Frame"))
            foldRoot = transform.parent.parent;
        else
            foldRoot = transform.parent;
        return foldRoot;
        //return transform;
    }

    [ExposeMethodInEditor]
    public void ToggleFold()
    {
        bool newFold = !isFolded;

        if (foldChildren || Input.GetKey(KeyCode.LeftAlt))
        {

            var otherFolds = GetRoot().GetComponentsInChildrenIncludingInactive<FoldControlBase>();
            otherFolds.Add(this);
            StartCoroutine(FoldAll(otherFolds, newFold, newFold));
            // Debug.Log($"allunfold {otherFolds.Count}");
            //SetFold(newFold);
        }
        else
            SetFold(newFold);

    }
    IEnumerator FoldAll(List<FoldControlBase> list, bool newFold, bool reverse)
    {
        locked = true;
        var wait = new WaitForSeconds(0.03f);

        if (reverse)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                list[i].SetFold(newFold);
                yield return wait;
            }
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetFold(newFold);
                yield return wait;
            }
        }

        yield return null;
        yield return wait;
        locked = false;
    }
    public virtual void SetFold(bool newFold)
    {
        if (!gameObject.activeInHierarchy) return;
        _foldLabelText.SetText(GetFoldString());
        NaiveFold(newFold);
        // if (isFolded)
        //    foldLabelText.SetText(isLeftSide ? labelFoldedAlt : labelFolded); //▲ ▶ ◀ ▼
        // else
        //    foldLabelText.SetText(labelUnfolded);
    }
    public void Fold()
    {
        SetFold(true);
    }
    public void UnFold()
    {
        SetFold(false);
    }

}