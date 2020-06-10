using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// can be used to provide drag and drop gameobject field, verifyng that tehre is an interface listener present (specified by the generic argument).
// really only useful for interfaces as monobehaviours dont need this
// v.02 seach in soucrce in case there is no targget. not necesarily an impor

[System.Serializable]

public class GenericSelector<T>
{
    [SerializeField] GameObject _referenceGameObject;
    public GameObject referenceGameObject
    {
        get { return _referenceGameObject; }
        set
        {

            _referenceGameObject = value;
        }
    }
    protected GameObject lastGameObject;
    [ReadOnly]
    [SerializeField]
    string message = "";
    public T _valueSource;
    public T valueSource { get { return _valueSource; } set { _valueSource = value; } }

    // public Transform referenceGameObjectTransform { get { if (referenceGameObject == null) return null; return referenceGameObject.transform; } }

    public virtual void OnValidate(MonoBehaviour source = null)
    {

        if (referenceGameObject != null)
        {
            if (valueSource == null || lastGameObject != referenceGameObject)
                valueSource = referenceGameObject.GetComponent<T>();
        }
        if (valueSource == null)
        {
            if (source != null)
                valueSource = source.GetComponent<T>();
            if (valueSource == null)
            {
                lastGameObject = null;
            }
            else
            {
                referenceGameObject = source.gameObject;
            }
        }
       // lastGameObject = referenceGameObject;

        if (valueSource == null)
        {
            message = "[No source]";
            referenceGameObject = null;
        }
        else
            message = referenceGameObject.name + " [" + valueSource.GetType() + "]";

        lastGameObject = referenceGameObject;

        //GetValue();
    }

    // /// <summary>    
    // /// Handles null case
    // /// </summary>
    // /// <value></value>
    public string GetMessage()
    {
        string n = "[no source]";
        if (referenceGameObject != null && valueSource != null)
            n = referenceGameObject.name + " [" + valueSource.GetType() + "]";
        // if (overrideValue) n += " [Override]";
        return n;
    }
}