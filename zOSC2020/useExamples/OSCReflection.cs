// ///zambari codes unity

// using UnityEngine;
// //using UnityEngine.UI;
// //using UnityEngine.UI.Extensions;
// //using System.Collections;
// //using System.Collections.Generic;
// using UnityOSC;
// using Z.OSC;

// public class OSCReflection : MonoBehaviour
// {

//     public GameObject go;
//     OSCRouter reflectionRouter;
//     void Start()
//     {

//         reflectionRouter = new OSCRouter("/ref");
//         reflectionRouter.bind(this, "/find", findGO);
//         reflectionRouter.bind(this, "/ddaared", findGO);
//         reflectionRouter.bind(this, "/find2", findGO2);
//         reflectionRouter.bind(this, "/bbdd2", findGO2);
//         reflectionRouter.bind(this, "/froward", findGO);
//         reflectionRouter.bind(this, "/frrend2", findGO2);
//         reflectionRouter.bind(this, "/fxxffnd2", findGO2);
//         reflectionRouter.bind(this, "/aabind", findGO);
//         reflectionRouter.bind(this, "/ddaard", findGO);

//         reflectionRouter.bind(this, "/ddaared/ldpa", findGO);
//         reflectionRouter.bind(this, "/ddaa/dffrd", findGO);

//     }


//     void setActive(float f)
//     {
//         if (go == null)
//             zOSC_1.console("no gameObject selected");
//         else
//         {
//             go.transform.Translate(go.transform.forward * f);
//             zOSC_1.console("OK moved " + go.name + " by " + f);
//         }

//     }

//     void forward(float f)
//     {
//         if (go == null)
//             zOSC_1.console("no gameObject selected");
//         else
//         {
//             go.transform.Translate(go.transform.forward * f);
//             zOSC_1.console("OK moved " + go.name + " by " + f);
//         }

//     }


//     void findGO2(string[] s)
//     {
//         Debug.Log("go2 array ");

//     }
//     void findGO(string s)
//     {
//         Debug.Log("looking for gameobject " + s);
//         go = GameObject.Find(s);
//         if (go != null)
//             zOSC_1.console("found gameObject " + gameObject.name);
//         else
//             zOSC_1.console("not found");
//         //zOSC_1.console()
//     }


// }
