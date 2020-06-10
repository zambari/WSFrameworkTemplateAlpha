// ///zambari codes unity

// using System.Reflection;
// using UnityEngine;
// //using UnityEngine.UI;
// //using UnityEngine.UI.Extensions;
// //using System.Collections;
// //using System.Collections.Generic;

// public class zOSCReflectionMapper : OSCBindBasic
// {
    
//     Component component;
//     FieldInfo field;
//     PropertyInfo property;
    
//     [Header("Component to look for")]
//     public string componentName;
//     [Header("Field/propety")]
//     public string fieldOrPropety;

//     [Header("Test value sending")]

//     [Range(0, 100)]
//     public float testValueSet;
//     string lastbind;
//     [Header("Check status:")]
//     public string statusRead;
//     [Header("Available fields:")]
//     public string[] availableComponents;
//     public string[] availableFields;
//     public string[] availableProperties;
//     protected override void OnValidate()
//     {
//         reflectionMagic();
//         oscRecieve(testValueSet);

//         base.OnValidate();
//            if (component == null) statusRead = "no component"; else statusRead = "component found";
//         if (property != null) statusRead += " / found property ";
//         else
//         if (field == null) statusRead += " / no field / no property "; else statusRead = " /   field found";
//     }
//     void oscRecieve(float f)
//     {
//         if (field != null)
//             field.SetValue(component, testValueSet);
//         else
//              if (property != null)
//             property.SetValue(component, testValueSet, null);
//     }
//     void reflectionMagic()
//     {
//         Component[] all = GetComponents<Component>();
//         availableComponents = new string[all.Length];
//         for (int i = 0; i < all.Length; i++)
//         {
//             availableComponents[i] = all[i].GetType().ToString();
//             if (all[i].GetType().ToString().Equals(componentName))
//                 component = all[i];
//         }
//         if (component==null) return;
//         FieldInfo[] fields = component.GetType().GetFields();
//         availableFields = new string[fields.Length];
//         for (int i = 0; i < fields.Length; i++)
//         {
//             availableFields[i] = fields[i].Name;
//             if (fields[i].Name.Equals(fieldOrPropety))
//                 field = fields[i];
//         }
//         if (field == null) 
//         {
//             PropertyInfo[] props = component.GetType().GetProperties();
//             availableProperties = new string[props.Length];
//             for (int i = 0; i < props.Length; i++)
//             {
//                 availableProperties[i] = props[i].Name;
//                 if (props[i].Name.Equals(fieldOrPropety))
//                     property = props[i];
//             }
//         }
//     }

//     protected override void OSCBind()
//     {
//         // zOSC_1.bind(this, oscRecieve,OSCAddress);
//     }
//     protected override void OSCUnbind()
//     {
//         zOSC_1.Unbind(OSCAddress);
//     }
//     void Start()
//     {
//         OSCBind();
//     }

// }
