using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
// using zUniqueness;
using Z;
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class ObjectID : MonoBehaviour, IRequestInitEarly //, IContextMenuBuilder
{
    //  public UniqueIDBadge badge;
    [SerializeField] ulong _identifier;
    public ulong identifier { get { return _identifier; } private set { _identifier = value; } }
    public void OverrideIdentifier(string idAsString)
    {
        Handle(idAsString.FromFingerPrint());
    }
    public void OverrideIdentifier(ulong id)
    {
        Handle(id);
    }
    void Handle(ulong id)
    {
        ObjectID.UnregisterID(id, this);
        _identifier = id;
        if (ObjectID.RegisterID(id, this))
        {
            Debug.Log("id ovveride succeeded ");
        }
        else
        {
            Debug.Log("id ovveride not succeeded ");
        }
    }
    public static List<ulong> identifierList; //= new List<ulong>();
    public static Dictionary<ulong, GameObject> objectDict = new Dictionary<ulong, GameObject>();

    #region idHandling
    public void ValidateIdentifier()
    {
        if (identifier == 0 || !ObjectID.RegisterID(identifier, this))
        {
            if (!ObjectIDExtensions.CreateAndValidateIdentifier(this))
            {
                Debug.Log("failed getting new objectid " + name, gameObject);
            }
        }
    }
    #endregion idHandling
    #region Unity
    [SerializeField] string idStringPartA;
    [SerializeField] string idStringPartB;
    private bool wasInit = false;
    void Awake()
    {
        Init(this);
    }
    void PrepareNiceStrings(ulong id)
    {
        var bytes = BitConverter.GetBytes(identifier);
        idStringPartA = ObjectIDExtensions.ToStringAsHexA(bytes);
        idStringPartB = ObjectIDExtensions.ToStringAsHexB(bytes);
    }
    void OnDestroy()
    {
        if (identifierList != null && identifierList.Contains(identifier))
            identifierList.Remove(identifier);
        if (objectDict != null && objectDict.ContainsKey(identifier))
            objectDict.Remove(identifier);
    }

    void OnValidate()
    {
        if (zBench.PrefabModeIsActive(gameObject)) return;
        if (identifier>=System.Int32.MaxValue) identifier=identifier>>2+UnityEngine.Random.Range(0,5053);
        ValidateIdentifier();
        PrepareNiceStrings(identifier);
    }

    void OnIDConfirmed(ulong id)
    {
        if (identifier != id)
            identifier = id;
        PrepareNiceStrings(id);
    }
    void OnEnable()
    {

        Init(this);
        PrepareNiceStrings(identifier);
    }
    void Reset()
    {
        Init(this);
        //   GetNewId();
    }

    void OnDisable()
    {
        wasInit = false;
    }
    public void Init(MonoBehaviour awakenSource)
    {
        if (wasInit) return;
        wasInit = true;
        ValidateIdentifier();
    }
    public void GetNewId()
    {
        identifier = 0;
        ValidateIdentifier();
    }
    #endregion
    #region statics

    static System.Int32 _incremental;

    /// <summary>
    /// always returns value +1 from the previous requested (globally)
    /// </summary>
    /// <value></value>
    public static System.Int32 incremental { get { return _incremental++; } set { _incremental = value; } }

    /// <summary>
    /// debug 
    /// </summary>
    public static void ResetIncremental()
    {
        _incremental = 0;
    }

    public static int Count { get { return objectDict.Count; } }
    // static bool zeroglaged; //todo: temporary
    public static void UnregisterID(ulong id, ObjectID objectID)
    {
        if (id == 0)
        {
            return;
        }
        if (objectDict != null && objectDict.ContainsKey(id))
        {
            objectDict.Remove(id);
        }
        if (identifierList != null || identifierList.Contains(id))
        {

            identifierList.Remove(id);
        }
    }

    public static bool RegisterID(ulong id, ObjectID objectID)
    {
        if (id == 0)
        {
            Debug.Log("zero id on " + objectID.name, objectID);
            return false;
        }
        if (id>=System.Int64.MaxValue) 
        {
            Debug.Log("more than long, sorry");
            return false;
        }

        if (identifierList == null || objectDict == null)
        {
            objectDict = new Dictionary<ulong, GameObject>();
            identifierList = new List<ulong>();
        }
        if (objectDict.ContainsKey(id))
        {
            if (objectDict[id] == objectID.gameObject)
            {
                return true;
            }
            ObjectID.incremental++; //skipping one
                                    // Debug.Log($" key {id} was present was pointintg at a diffent obejct '{ objectID.name}' other was '{ objectDict[id].NameOrNull()}'", objectID);
            return false;
        }
        identifierList.Add(id);
        objectDict.Add(id, objectID.gameObject);
        objectID.OnIDConfirmed(id);

        return true;
    }

    public static Transform FindTransform(ulong val)
    {
        return val.FindTransform();
    }
    #endregion statics

}