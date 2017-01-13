using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct Boolean4
{
    public bool x;
    public bool y;
    public bool z;
    public bool w;
    public Boolean4(bool xVal = false, bool yVal = false, bool zVal = false, bool wVal = false)
    {
        x = xVal;
        y = yVal;
        z = zVal;
        w = wVal;
    }
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using System.Reflection;
    using System.Text;
    using UnityEditor;
    // IngredientDrawer
    [CustomPropertyDrawer(typeof(Boolean4))][CanEditMultipleObjects]
    public class Boolean4Drawer : Boolean3Drawer
    {
        static StringBuilder Builder = new StringBuilder();
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var xRef = property.FindPropertyRelative("x");
            var yRef = property.FindPropertyRelative("y");
            var zRef = property.FindPropertyRelative("z");
            var wRef = property.FindPropertyRelative("w");
            var newBool = Draw4(position, new Boolean4(xRef.boolValue, yRef.boolValue, zRef.boolValue, wRef.boolValue), label, fieldInfo);
            xRef.boolValue = newBool.Value.x;
            yRef.boolValue = newBool.Value.y;
            zRef.boolValue = newBool.Value.z;
            wRef.boolValue = newBool.Value.w;
        }

        [ExposeDrawMethod(typeof(Boolean4))]
        static public ReferenceType<Boolean4> Draw4(Rect position, object eventsObj, GUIContent content, ICustomAttributeProvider info = null)
        {
            Boolean4 boolPair = (Boolean4)eventsObj;
            var boolArray = new bool[] { boolPair.x, boolPair.y, boolPair.z, boolPair.w };
            Builder = new StringBuilder();
            char flags = (char)EditorNameFlags.Default;
            if (info != null && info.HasAttribute(typeof(CustomNamesAttribute)))
            {
                CustomNamesAttribute attribute = info.GetCustomAttributes<CustomNamesAttribute>()[0];
                flags = (char)attribute.CustomFlags;
                Builder.Append(flags);
                if (attribute.UseVariableNameAsTitle)
                {
                    Builder.Append(Seperator);
                    Builder.Append(content.text);
                }
                Builder.Append(attribute.CombinedName);
            }
            else
            {
                Builder.Append(flags);
                Builder.Append(Seperator);
                Builder.Append(content.text);
            }
            content.text = Builder.ToString();
            DrawMultiBoolean(ref boolArray, position, content);
            boolPair.x = boolArray[0];
            boolPair.y = boolArray[1];
            boolPair.z = boolArray[2];
            boolPair.w = boolArray[3];
            return boolPair;
        }
    }
}
#endif