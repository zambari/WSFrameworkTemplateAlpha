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
    public static List<ulong> identifierList; //= new List<ulong>();
    public static Dictionary<ulong, GameObject> objectDict = new Dictionary<ulong, GameObject>();

    #region idHandling
    public void ValidateIdentifier()
    {
        if (!ObjectIDExtensions.CreateAndValidateIdentifier(this))
        {
            Debug.Log("failed getting new objectid " + name, gameObject);
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
        wasInit = false;
        Init(this);
        PrepareNiceStrings(identifier);
    }
    void Reset()
    {
        Init(this);
        GetNewId();
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
    public static System.Int32 incremental { get { return _incremental++; } }

    /// <summary>
    /// debug 
    /// </summary>
    public static void ResetIncremental() 
    {
         _incremental = 0; 
    }

    public static int Count { get { return objectDict.Count; } }
    // static bool zeroglaged; //todo: temporary
    public static bool RegisterID(ulong id, ObjectID objectID)
    {
        if (id == 0)
        {
            Debug.Log("zero id", objectID);
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