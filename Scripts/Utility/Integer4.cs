using System;
using UnityEngine;

[Serializable]
public struct Integer4
{
    public int x;
    public int y;
    public int z;
    public int w;
    public Integer4(int xVal = 0, int yVal = 0, int zVal = 0, int wVal = 0)
    {
        x = xVal;
        y = yVal;
        z = zVal;
        w = wVal;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    static public implicit operator Vector4(Integer4 rhs)
    {
        return new Vector4(rhs.x, rhs.y, rhs.z, rhs.w);
    }
    static public implicit operator Integer4(Vector4 rhs)
    {
        return new Integer4((int)rhs.x, (int)rhs.y, (int)rhs.z, (int)rhs.w);
    }
    static public implicit operator Integer2(Integer4 rhs)
    {
        return new Integer2(rhs.x, rhs.y);
    }
    static public implicit operator Integer3(Integer4 rhs)
    {
        return new Integer3(rhs.x, rhs.y);
    }

    static public bool operator ==(Integer4 lhs, Integer4 rhs)
    {
        return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z) && (lhs.w == rhs.w);
    }
    static public bool operator !=(Integer4 lhs, Integer4 rhs)
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

    
    [CustomPropertyDrawer(typeof(Integer4))]
    [CanEditMultipleObjects]
    public class Integer4Drawer : Integer3Drawer
    {
        static StringBuilder Builder = new StringBuilder();
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var xRef = property.FindPropertyRelative("x");
            var yRef = property.FindPropertyRelative("y");
            var zRef = property.FindPropertyRelative("z");
            var wRef = property.FindPropertyRelative("w");
            var newInt = Draw4(position, new Integer4(xRef.intValue, yRef.intValue, zRef.intValue, wRef.intValue), label, fieldInfo);
            xRef.intValue = newInt.Value.x;
            yRef.intValue = newInt.Value.y;
            zRef.intValue = newInt.Value.z;
            wRef.intValue = newInt.Value.w;
        }

        [ExposeDrawMethod(typeof(Integer4))]
        static public ReferenceType<Integer4> Draw4(Rect position, object eventsObj, GUIContent content, ICustomAttributeProvider info = null)
        {
            Integer4 intPair = (Integer4)eventsObj;
            var intArray = new int[] { intPair.x, intPair.y, intPair.z, intPair.w };
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
            intPair.w = intArray[3];
            return intPair;
        }
    }
}
#endif