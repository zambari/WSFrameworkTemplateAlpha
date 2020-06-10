// ///zambari codes unity

// using UnityEngine;
// //using UnityEngine.UI;
// //using UnityEngine.UI.Extensions;
// //using System.Collections;
// //using System.Collections.Generic;
// using Z.OSC;

// public class OSCRecieveRotationEuler : OSCBindBasic
// {
//     public bool useLocal;
//     public Vector3 lastRecieved;
//         public bool useNormalised;
//     public void onOsc(Vector3 q)
    
//     {
//         lastRecieved=q;
//   if (useNormalised) q=q*360;
//       //  Debug.Log(name+"recevn gor".MakeGreen()+q,gameObject);
//         if (enabled)
//         if (useLocal)
//             transform.localEulerAngles = q;
//             else
//             transform.eulerAngles = q;

//     }
//        protected override void OSCUnbind()
//     {
//         zOSC_1.Unbind(OSCAddress);
//     }

//     protected override void OSCBind()
//     {
//         zOSC_1.BindVector3(this,OSCAddress, onOsc);
//     }
//     void Start()
//     {
//         OSCBind();
//     }
 

// }
