// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using LayoutPanelDependencies;
// namespace Z.LayoutPanel
// {
//     public static class LayoutSettings
//     {

//         public static readonly int minRectWidth = 50; // won't allow scaling lower than that
//         public static readonly int minRectHeight = 25; // won't allow scaling lower than that

//         public static Color dropTargetColor { get { return new Color(0.2f, 1, 0.2f, 0.8f); } }
//         public static Color dropTargetColorWhenSplit { get { return new Color(.7f, 0.2f, 0.7f, 0.8f); } }

//         const float columnModeOffset = 3f;

//         // defaults
// #if UNITY_ANDROID && !UNITY_EDITOR
//         static int __borderSpacing = 5;
// #else
//         static int __borderSpacing = 2;
// #endif
//         // static int _verticalSpacing = 1;
//         static int _topHeight = 15;
// #if UNITY_ANDROID && !UNITY_EDITOR
//         static int _borderSize =9;
// #else
//         static int _borderSize = 3;

// #endif
//         public static float borderSizeColumnOffset { get { return _borderSize * columnModeOffset; } } // set { _borderSize = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }

//         public static int borderSize { get { return _borderSize; } set { _borderSize = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }
//         public static int groupSpacing
//         {
//             get
//             {
//                 //  var x = 2 * _borderSize + borderSpacing; ;
//                 return 5;
//                 //  return x;
//             }
//             set
//             {
//                 value -= 2 * borderSize;
//                 if (value < 0) return;
//                 borderSpacing = value;
//                 if (onBorderSizeChange != null) onBorderSizeChange();
//             }
//         }
//         public static int borderSpacing { get { return __borderSpacing; } set { __borderSpacing = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }
//         // public static int verticalSpacing { get { return _verticalSpacing; } set { _verticalSpacing = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }
//         //static int _topHeight = 9;
//         public static int topHeight { get { return _topHeight; } set { _topHeight = value; if (onBorderSizeChange != null) onBorderSizeChange.Invoke(); } }
//         public static int minWidth { get { return topHeight * 4; } }
//         public static System.Action onBorderSizeChange;
//         public static int internalGroupPadding { get { return 5; } }
//         public static int groupPaddigNoPanel { get { return 0; } }
//         public static int groupPaddigPanel { get { return borderSize + internalGroupPadding; } }
//       

//      
// }
