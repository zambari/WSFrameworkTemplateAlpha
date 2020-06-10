using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System;

// public static class StructureExtensions
// {
//     public static byte[] ToByteArray<T>(this T structure) where T : struct
//     {
//         var bufferSize = Marshal.SizeOf(structure);
//         var byteArray = new byte[bufferSize];

//         IntPtr handle = Marshal.AllocHGlobal(bufferSize);
//         Marshal.StructureToPtr(structure, handle, true);
//         Marshal.Copy(handle, byteArray, 0, bufferSize);

//         return byteArray;
//     }

//     public static T ToStructure<T>(this byte[] byteArray) where T : struct
//     {
// #if NET_4_6
//         var packet = new T();
//         var bufferSize = Marshal.SizeOf(packet);
//         IntPtr handle = Marshal.AllocHGlobal(bufferSize);
//         Marshal.Copy(byteArray, 0, handle, bufferSize);

//         return Marshal.PtrToStructure<T>(handle);
// #else
//         throw new System.InvalidProgramException("To use structure casting, switch your project to NET4.6");
// #endif
//     }
// }
