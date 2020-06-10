
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityOSC;
// using System;
// namespace Z.OSC
// {
//     public partial class OSCRouter
//     {
//         public void bindGeneric<T>(MonoBehaviour requester, string addr, T listener)
//         {
//             if (String.IsNullOrEmpty(addr)) return;

//             //     Debug.Log("bind failed, address "+addr+" is already taken ");
//             //  else
//             //   {
//             if (addr[addr.Length - 1] == '*')
//             {
//                 Debug.Log("binding class");
//                 bindClass(requester, addr, listener);

//             }
//             else
//             {
//                 Binding newbinding = new Binding();
//                 newbinding.address = addr;
//                 newbinding.bindMethod = listener;
//                 newbinding.objectType = listener.GetType();
//                 newbinding.requester = requester;
//                 if (oscBinding.ContainsKey(addr))
//                 {
//                     Binding currentBinding = oscBinding[addr];
//                     newbinding.nextBind = currentBinding;
//                     //  oscBinding.Remove(addr);
//                     oscBinding[addr] = newbinding;
//                     if (zOSC_1.logDetails)
//                         zOSC_1.Log("warning: dupliace osc address bind " + addr + "  " + requester.name + " previous was " + currentBinding.requester.name);
//                 }
//                 else
//                     oscBinding.Add(addr, newbinding);
//             }
//         }

//         public void bindClass<T>(MonoBehaviour requester, string addr, T listener)
//         {
//             //  Debug.Log("classs");
//             if (String.IsNullOrEmpty(addr)) return;

//             if (addr[addr.Length - 1] == '*') addr = addr.Substring(0, addr.Length - 1);
//             zOSC_1.Log(addr + " by " + requester.name);

//             Binding newbinding = new Binding();
//             newbinding.address = addr;
//             newbinding.bindMethod = listener;
//             newbinding.objectType = listener.GetType();
//             newbinding.requester = requester;
//             zOSC_1.Log("adding partial binding " + addr + "  " + requester.name);
//             //            oscPartialBinding.Add(new KeyValuePair<string, Binding>(addr, newbinding));
//             //  List<Binding> thisList;
//             /*
//             if (oscPartialBinding.ContainsKey(addr))
//              {
//                  thisList=oscPartialBinding[add]
//                  Binding currentBinding = oscBinding[addr];
//                  newbinding.nextBind = currentBinding;
//                  oscBinding.Remove(addr);
//                  Debug.Log("chained bind "+addr+"  " + requester.name + " previous was " + currentBinding.requester.name);
//              }
//              oscBinding.Add(addr, newbinding);*/

//         }
//         public void bind(MonoBehaviour requester, string addr, Action listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<float> listener)
//         { bindGeneric(requester, addr, listener); }
//         // public void Bind(MonoBehaviour requester, string addr, Action<OSCPacketExtensions.PositionAndRotation> listener)
//         // { bindGeneric(requester, addr, listener); }

//         public void bind(MonoBehaviour requester, string addr, Action<int> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<string> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<byte[]> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<string, byte[]> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<float[]> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<string[]> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<List<object>> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<Vector2> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<Vector3> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void bind(MonoBehaviour requester, string addr, Action<Quaternion> listener)
//         { bindGeneric(requester, addr, listener); }
//         public void addStringArrayProvider(MonoBehaviour monoSource, string addr, Func<string[]> provider)
//         { bindGeneric(monoSource, addr, provider); }
//         public void addFloatArrayProvider(MonoBehaviour monoSource, string addr, Func<float[]> provider)
//         { bindGeneric(monoSource, addr, provider); }
//         public void addStringProvider(MonoBehaviour monoSource, string addr, Func<string> provider)
//         { bindGeneric(monoSource, addr, provider); }
//         public void addFloatProvider(MonoBehaviour monoSource, string addr, Func<float> provider)
//         { bindGeneric(monoSource, addr, provider); }
//         public virtual void bind<T>(MonoBehaviour monoSource, string addr, Func<T> provider)
//         { bindGeneric(monoSource, addr, provider); }




//     }
// }