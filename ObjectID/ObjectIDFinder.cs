using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
public class ObjectIDFinder : MonoBehaviour
{
    [TextArea(2, 4)]
    public string input;
    [ReadOnly] public string status;
    public GameObject foundObject;
    public ulong numberFromString;
    public bool toggleAvtion;
    [ReadOnly] [SerializeField] int objectCount;
    void OnValidate()
    {

        foundObject = null;
        if (string.IsNullOrEmpty(input))
        {
            status = "Paste a string containing objectID";
            return;
        }
        status = "notfound";
        var split = input.Split(' ');
        input = split.LastItem();
        ulong value;
        if (System.UInt64.TryParse(input, out  value))
        {
            numberFromString = value;
            foundObject = value.FindObject();
            if (foundObject == null)
                status = "it is a known value but object is dead";
            else
                status = " FOUND!";
        }
        else
        {
            status = "string did not parse :(";
            Debug.Log("could not parse");
        }
        objectCount = ObjectID.Count;
    }

}
