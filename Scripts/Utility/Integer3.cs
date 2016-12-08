using System;
using UnityEngine;

[Serializable]
public struct Integer3
{
    public int x;
    public int y;
    public int z;
    public Integer3(int xVal = 0, int yVal = 0, int zVal = 0)
    {
        x = xVal;
        y = yVal;
        z = zVal;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    static public implicit operator Vector3(Integer3 rhs)
    {
        return new Vector3(rhs.x, rhs.y, rhs.z);
    }
    static public implicit operator Integer3(Vector3 rhs)
    {
        return new Integer3((int)rhs.x, (int)rhs.y, (int)rhs.z);
    }
    static public implicit operator Integer2(Integer3 rhs)
    {
        return new Integer2(rhs.x, rhs.y);
    }
    static public implicit operator Integer4(Integer3 rhs)
    {
        return new Integer4(rhs.x, rhs.y, rhs.z);
    }

    static public bool operator ==(Integer3 lhs, Integer3 rhs)
    {
        return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z);
    }
    static public bool operator !=(Integer3 lhs, Integer3 rhs)
    {
        return !(lhs == rhs);
    }
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using System.Reflection;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    
    [CustomPropertyDrawer(typeof(Integer3))]
    [CanEditMultipleObjects]
    public class Integer3Drawer : Integer2Drawer
    {
        static StringBuilder Builder = new StringBuilder();
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var xRef = property.FindPropertyRelative("x");
            var yRef = property.FindPropertyRelative("y");
            var zRef = property.FindPropertyRelative("z");
            var newInt = Draw3(position, new Integer3(xRef.intValue, yRef.intValue, zRef.intValue), label, fieldInfo);
            xRef.intValue = newInt.Value.x;
            yRef.intValue = newInt.Value.y;
            zRef.intValue = newInt.Value.z;
        }

        [ExposeDrawMethod(typeof(Integer3))]
        static public ReferenceType<Integer3> Draw3(Rect position, object eventsObj, GUIContent content, ICustomAttributeProvider info = null)
        {
            Integer3 intPair = (Integer3)eventsObj;
            var intArray = new int[] { intPair.x, intPair.y, intPair.z };
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
            DrawMultiInteger(ref intArray, position, content);
            intPair.x = intArray[0];
            intPair.y = intArray[1];
            intPair.z = intArray[2];
            return intPair;
        }
    }
}
#endif