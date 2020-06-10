// ///zambari codes unity

// using UnityEngine;
// //using UnityEngine.UI;
// //using UnityEngine.UI.Extensions;
// using System.Collections;
// //using System.Collections.Generic;

// public class OSCSendRotation : OSCSendBase
// {
//     Quaternion lastSentRotation;
//     [SerializeField] float _interval=0.1f;
//      public float interval { get { return _interval;} set { _interval=value;} }
//     void Start()
//     {
//           StartCoroutine(sendRots());
//     }

//     IEnumerator sendRots()
//     {

//         while (true)
//         {    yield return new WaitForSeconds(interval);
//             if (transform.localRotation != lastSentRotation)
//             {
//                 lastSentRotation = transform.localRotation;

//                 zOSC_1.BroadcastOSC(OSCAddress, lastSentRotation);

//             }
        
//         }
//     }

// }
