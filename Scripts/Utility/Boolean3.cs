using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct Boolean3
{
    public bool x;
    public bool y;
    public bool z;
    public Boolean3(bool xVal = false, bool yVal = false, bool zVal = false)
    {
        x = xVal;
        y = yVal;
        z = zVal;
    }
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using System.Reflection;
    using System.Text;
    using UnityEditor;
    // IngredientDrawer
    [CustomPropertyDrawer(typeof(Boolean3))]
    [CanEditMultipleObjects]
    public class Boolean3Drawer : Boolean2Drawer
    {
        static StringBuilder Builder = new StringBuilder();
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var xRef = property.FindPropertyRelative("x");
            var yRef = property.FindPropertyRelative("y");
            var zRef = property.FindPropertyRelative("z");
            var newBool = Draw3(position, new Boolean3(xRef.boolValue, yRef.boolValue, zRef.boolValue), label, fieldInfo);
            xRef.boolValue = newBool.Value.x;
            yRef.boolValue = newBool.Value.y;
            zRef.boolValue = newBool.Value.z;
        }

        [ExposeDrawMethod(typeof(Boolean3))]
        static public ReferenceType<Boolean3> Draw3(Rect position, object eventsObj, GUIContent content, ICustomAttributeProvider info = null)
        {
            Boolean3 boolPair = (Boolean3)eventsObj;
            var boolArray = new bool[] { boolPair.x, boolPair.y, boolPair.z };
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
            return boolPair;
        }
    }
}
#endif