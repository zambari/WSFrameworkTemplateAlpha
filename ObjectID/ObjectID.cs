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
public class ObjectID : MonoBehaviour, IRequestInitEarly//, IContextMenuBuilder
{
    //  public UniqueIDBadge badge;
    // public static System.Random rnd;// = new System.Random();
    [SerializeField] ulong _identifier;
    public ulong identifier { get { return _identifier; } private set { _identifier = value; } }
    public string idStringPartA;
    public string idStringPartB;

    byte[] bytes = new byte[8];
    public static List<ulong> identifierList; //= new List<ulong>();
    public static Dictionary<ulong, GameObject> objectDict = new Dictionary<ulong, GameObject>();
    void OnDestroy()
    {
        if (identifierList != null && identifierList.Contains(identifier))
            identifierList.Remove(identifier);
        if (objectDict != null && objectDict.ContainsKey(identifier))
            objectDict.Remove(identifier);
    }
    public static void Seed(int seed)
    {
        // Debug.Log("seed =" + seed);

        //   rnd = new System.Random(seed);
    }
    /*
       string toHash = thisTrans.name;
                if (thisTrans.parent != null)
                {
                    toHash += thisTrans.parent.name;
                    toHash += thisTrans.parent.GetSiblingIndex();
                }
                toHash += thisTrans.GetSiblingIndex();
             */
    public static byte[] GetFromString(string s)
    {

        using(MD5 md5 = MD5.Create())
        {
            md5.Initialize();
            md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            return md5.Hash;
        }
    }
    static long counter;
    public void ValidateIdentifier()
    {

        if (identifier == 0) identifier = CreateNewIdentifier(gameObject);
        //    // int AttemptCount = 0;
        ulong proposal = identifier;
        if (RegisterID(proposal, this)) return;
        string stringidea = name;
        if (RegisterID(ObtainFromString(stringidea), this)) return;
        if (RegisterID(ObtainFromString(stringidea), this)) return;
        for (int i = 0; i < 5; i++)
        {

            if (RegisterID(ObtainIdentifierFromTime(), this)) return;
        }
        Debug.Log("Failed getting unieqe iod "+name, gameObject);
        //  string nameoption= name;

        // proposal=ObtainFromName(name+(transform.parent==null?"root":transform.parent.name));
        // if (RegisterID(proposal, this)) return; }

        //         identifier = ObtainIdentifier();

    }
    public ulong ObtainFromString(string s)
    {
        var bytes = GetFromString(s);
        return BitConverter.ToUInt64(bytes, 0); //+ (ulong)Mathf.FloorToInt(10000 * Time.time);
    }
    public ulong ObtainIdentifierFromTime()
    {

        counter++;
        return (ulong) System.DateTime.Now.Ticks << 16; //+ (ulong)(gameObject.GetInstanceID() + counter);
        //int watchdog = 0;

    }
    System.Random random;
    public ulong ObtainRandomme()
    {
        if (random == null)
        {
            Debug.Log("  rnd = null");
            random = new System.Random();
            if (random == null) Debug.Log("still rnd");
            //return;
        }

        //         while (!sccess && watchdog < 50)
        random.NextBytes(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    } //+ (ulong)Mathf.FloorToInt(10000 * Time.time);
    //             success = RegisterID(identifier, gameObject);
    // #if NOT
    //      
    //         {
    //             watchdog++;
    //             if (watchdog > 10)
    //             {
    //                 random = new System.Random();
    //                 Debug.Log("swtiched randomgent");
    //             }
    //             random.NextBytes(bytes);
    //             identifier = BitConverter.ToUInt64(bytes, 0);//+ (ulong)Mathf.FloorToInt(10000 * Time.time);
    //             success = RegisterID(identifier, gameObject);
    //             if (!success)
    //             {
    //                 GameObject id = identifier.FindObject();
    //                 if (id == null)
    //                     Debug.Log("not presetnt, empty code ");
    //                 else
    //                 {
    //                     Debug.Log($" {name} our id { identifier } taken by {id.name}  { id } scenes are same = { gameObject.scene == id.gameObject.scene} ", id);

    //                     //  if (id.transform==transform.parent)
    //                     ulong ourParentid = transform.parent.GetObjectID();
    //                     var parentobject = ourParentid.FindObject();
    //                     Debug.Log($"{name}| we are here, our parent has id { ourParentid} {parentobject.name } ", gameObject);
    //                 }
    //             }
    //         }
    //         if (watchdog >= 10)
    //         {
    //             Debug.Log("conflict ", gameObject);
    //             Debug.Log("resolving");
    //             while (!RegisterID(identifier, gameObject) && watchdog < 100)
    //             {
    //                 watchdog++;

    //                 random.NextBytes(bytes);
    //                 identifier = BitConverter.ToUInt64(bytes, 0);
    //             }

    //         }
    // #endif
    //         PrepareNiceStrings(identifier);
    //         if (watchdog > 50) Debug.Log("Watchdog " + watchdog);

    //     }
    void Awake()
    {
        //if (identifier == 0)
        //  ValidateId(rnd);
        Init(this);

    }
    // string GetAsString(ulong id)
    // {

    // }
    bool reverse = true;
    public void PrepareNiceStrings(ulong id)
    {
        var bytes = BitConverter.GetBytes(identifier);
        idStringPartA = "";
        idStringPartB = "";
        if (reverse)
        {
            for (int i = bytes.Length - 1; i >= bytes.Length / 2; i--)
                idStringPartA += "[" + bytes[i].ToString("X2") + "]";

            for (int i = bytes.Length / 2 - 1; i >= 0; i--)
                idStringPartB += "[" + bytes[i].ToString("X2") + "]";
        }
        else
        {
            for (int i = 0; i < bytes.Length / 2; i++)
                idStringPartA += "[" + bytes[i].ToString("X2") + "]";
            for (int i = bytes.Length / 2; i < bytes.Length; i++)
                idStringPartB += "[" + bytes[i].ToString("X2") + "]";
        }
    }
    void OnValidate()
    {
        //  if (Prefab)
        if (zBench.PrefabModeIsActive(gameObject)) return;
        ValidateIdentifier();
#if UNITY_EDITOR
        //  if (UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(gameObject)== null)
        //  {
        //      Debug.Log("objectid in prefab mode ",gameObject);
        //      return;
        //  }
        //  this.MoveComponent(15);
#endif
        //ValidateIdentifier();

        // {
        //     ObtainIdentifier();
        // }
        PrepareNiceStrings(identifier);
    }

    public static int Count { get { return objectDict.Count; } }
    // static bool zeroglaged; //todo: temporary
    static bool RegisterID(ulong id, ObjectID objectID)
    {
        if (id == 0)
        {
            // if (!zeroglaged)
            {
                Debug.Log("zero id", objectID);
                // zeroglaged = true;
            }
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
        objectID.ConfirmID(id);

        return true;
    }
    void ConfirmID(ulong id)
    {
        if (identifier != id)
            // Debug.Log("confiremde id", gameObject);
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
        // rnd = new System.Random();
        //  ValidateId(rnd);
    }
    public static ulong CreateNewIdentifier(GameObject g)
    {
        return (ulong) (System.DateTime.Now.Ticks << 24 ^ g.GetInstanceID());
    }
    bool wasInit = false;
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
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/AssignNewObjectIds")]
    public static void AssignNewObjectID()
    {
        var allobjs = Resources.FindObjectsOfTypeAll(typeof(ObjectID)) as ObjectID[];
        Debug.Log("found " + allobjs.Length);
        foreach (var v in allobjs)
        {
            UnityEditor.Undo.RecordObject(v, "new id");
            v.GetNewId();
        }
    }

#endif
    static List<Material> copiedMaterials { get { if (_copiedMaterials == null) _copiedMaterials = new List<Material>(); return _copiedMaterials; } }
    static List<Material> _copiedMaterials;
    // public void BuildContextMenu(PrefabProvider prefabs, Transform target)
    // {
     
    //     prefabs.GetLabel(target, idStringPartA);
    //     prefabs.GetLabel(target, idStringPartB);

		
	// 	var toggle = prefabs.GetToggle(target, "activeSelf");
	// 	toggle.isOn = gameObject.activeSelf;
	// 	var toggle2 = prefabs.GetToggle(target, "ActiveInh");
	// 	toggle2.isOn = gameObject.activeInHierarchy;
	// 	toggle.AddCallback((x) => gameObject.SetActive(x));
	// 	toggle2.AddCallback((x) => gameObject.SetActive(x));
	// if (!enabled) return;
 
    //     var meshRenderers = GetComponentsInChildren<MeshRenderer>();
    //     if (meshRenderers.Length > 0)
    //     {
    //         prefabs.GetLabel(target, "Meshrenderers");
    //         prefabs.GetButton(target, "Copy Materials from " + meshRenderers.Length).AddCallback(() =>
    //         {
    //             foreach (var m in meshRenderers)
    //             {
    //                 if (m.sharedMaterial != null)
    //                 {
    //                     if (!copiedMaterials.Contains(m.sharedMaterial))
    //                         copiedMaterials.Add(m.material);
    //                 }

    //             }
    //         });

    //     }
    //       prefabs.GetLabel(target, "Materials");
    //     foreach (Material copitedMaterial in copiedMaterials)
    //     {
    //         if (copitedMaterial != null)
    //         {
    //             prefabs.GetButton(target, "Paste " + copitedMaterial.name).AddCallback(() =>
    //             {
    //                 foreach (var mr in meshRenderers)
    //                     mr.sharedMaterial = copitedMaterial;

    //             });
    //         }
    //     }
    //       prefabs.GetLabel(target, "Common");
    //     var dn = prefabs.GetButton(target, "Destory");
    //     int indexd = dn.transform.GetSiblingIndex();
    //     dn.AddCallback(() =>
    //     {
    //         var br2 = prefabs.GetButton(target, "AREYOUSURE");
    //         br2.transform.SetSiblingIndex(indexd);
    //         br2.AddCallback(() =>
    //         {
    //             GameObject.Destroy(gameObject);
    //         });
    //     });

    // }

}