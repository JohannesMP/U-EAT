using UnityEngine;
using System.Collections;
using System;
using System.Text;

[Serializable]
public struct Integer2
{
    public int x;
    public int y;

    public Integer2(int xVal = 0, int yVal = 0)
    {
        x = xVal;
        y = yVal;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }

    public Integer2 Set(int xVal, int yVal)
    {
        this.x = xVal;
        this.y = yVal;
        return this;
    }

    static public implicit operator Vector2(Integer2 rhs)
    {
        return new Vector2(rhs.x, rhs.y);
    }
    static public implicit operator Integer2(Vector2 rhs)
    {
        return new Integer2((int)rhs.x, (int)rhs.y);
    }
    static public implicit operator Integer3(Integer2 rhs)
    {
        return new Integer3(rhs.x, rhs.y);
    }
    static public implicit operator Integer4(Integer2 rhs)
    {
        return new Integer4(rhs.x, rhs.y);
    }

    static public bool operator ==(Integer2 lhs, Integer2 rhs)
    {
        return (lhs.x == rhs.x) && (lhs.y == rhs.y);
    }
    static public bool operator !=(Integer2 lhs, Integer2 rhs)
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
    // IngredientDrawer
    [CustomPropertyDrawer(typeof(Integer2))][CanEditMultipleObjects]
    public class Integer2Drawer : PropertyDrawer
    {
        protected const char Seperator = '|';
        static StringBuilder Builder = new StringBuilder();
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var xRef = property.FindPropertyRelative("x");
            var yRef = property.FindPropertyRelative("y");
            var newInt = Draw2(position, new Integer2(xRef.intValue, yRef.intValue), label, fieldInfo);
            xRef.intValue = newInt.Value.x;
            yRef.intValue = newInt.Value.y;
        }

        //Use pipe '|' to seperate the names. 
        //"@p" is the escape for '|'.
        //"@a" is the escape for '@'.
        static string[] SplitStringIntoNames(string srcString)
        {
            var names = srcString.Split(Seperator);
            foreach(var i in names)
            {
                i.Replace("@p", Seperator.ToString());
                i.Replace("@a", "@");
            }
            return names;
        }

        static readonly char[] VectorTitles = new char[] { 'X', 'Y', 'Z', 'W' };
        static protected void DrawMultiInteger(ref int [] outputInts, Rect position, GUIContent content)
        {
            var names = SplitStringIntoNames(content.text);
            var totalInts = outputInts.Length;
            char drawFlags = names[0].ToCharArray()[0];
            bool useTitle = (drawFlags & (char)EditorNameFlags.UseTitle) > 0;
            bool indentBelow = (drawFlags & (char)EditorNameFlags.IndentBelow) > 0;
            bool indentBelowAtShortDistances = (drawFlags & (char)EditorNameFlags.IndentBelowAtShortDistances) > 0;
            //bool toggleRight = (drawFlags & (char)EditorNameFlags.ToggleRight) > 0;
            //Func<int, GUIStyle, GUILayoutOption[], int> drawFunc = EditorGUILayout.IntField;
            var layout = GUI.skin.label;
            layout.fontStyle = FontStyle.Normal;
            int namesIndex = 1;

            //if(toggleRight)
            //{
            //    drawFunc = EditorGUILayout.Toggle;
            //}
            if (useTitle && namesIndex < names.Length)
            {
                EditorGUI.PrefixLabel(position, new GUIContent(names[namesIndex]), layout);
                ++namesIndex;
                position.x += InspectorValues.LabelWidth;
            }

            if (indentBelow && (!indentBelowAtShortDistances || (indentBelowAtShortDistances && position.width < InspectorValues.MultiLineToggle)))
            {
                var belowRect = position;
                belowRect.y -= position.height;
                GUILayout.BeginHorizontal();
                
                EditorGUILayout.LabelField("", GUILayout.Width(InspectorValues.Indent + 1));
                //var elementWidth = InspectorValues.MinElementWidth / totalInts;
                GUILayoutOption[] layoutOptions = new GUILayoutOption[] { };
                for (int i = 0; i < totalInts; ++i, ++namesIndex)
                {
                    
                    string name;
                    if(namesIndex >= names.Length)
                    {
                        name = VectorTitles[i].ToString();
                    }
                    else
                    {
                        name = names[namesIndex];
                    }
                    
                    //if (i == totalInts - 1)
                    //{
                    //    //elementWidth += InspectorValues.EdgePadding - InspectorValues.Indent;
                    //    GUILayout.Label(name, layout, layoutOptions);
                        
                    //    //EditorGUILayout.LabelField("", layoutOptions);
                    //    outputInts[totalInts - 1] = EditorGUILayout.IntField(outputInts[totalInts - 1]);
                    //}
                    //else
                    {
                        GUILayout.Label(name, layout, layoutOptions);
                        
                        outputInts[i] = EditorGUILayout.IntField(outputInts[i], GUILayout.MinWidth(0));
                    }
                }
                
                GUILayout.EndHorizontal();
            }
            else
            {
                
                if (position.width > InspectorValues.MinWidthBeforeClip)
                {
                    position.x += ((position.width - InspectorValues.MinWidthBeforeClip) / InspectorValues.WidthScaler);
                }
                position.width -= position.x;
                position.width += InspectorValues.EdgePadding - 1;
                //position.width -= 4;// + totalInts;
                position.width /= totalInts;
                
                for (int i = 0; i < totalInts; ++i, ++namesIndex)
                {
                    string name;
                    if (namesIndex >= names.Length)
                    {
                        name = VectorTitles[i].ToString();
                    }
                    else
                    {
                        name = names[namesIndex];
                    }
                    //if(toggleRight)
                    //{
                    //    outputBools[i] = EditorGUI.Toggle(position, name, outputBools[i], layout);
                    //}
                    //else
                    {
                        Rect labelPos = position;
                        var charIndent = GUI.skin.label.CalcSize(new GUIContent(name)).x;
                        labelPos.width = charIndent;
                        Rect intPos = position;
                        intPos.width -= charIndent;
                        intPos.x += charIndent;
                        EditorGUI.LabelField(labelPos, name);
                        outputInts[i] = EditorGUI.IntField(intPos, outputInts[i]);
                    }
                    
                    position.x += position.width;
                }
            }
        }

        [ExposeDrawMethod(typeof(Integer2))]
        static public ReferenceType<Integer2> Draw2(Rect position, object eventsObj, GUIContent content, ICustomAttributeProvider info = null)
        {
            Integer2 intPair = (Integer2)eventsObj;
            var intArray = new int[2]{ intPair.x, intPair.y };
            Builder = new StringBuilder();
            char flags = (char)EditorNameFlags.Default;
            if (info != null && info.HasAttribute(typeof(CustomNamesAttribute)))
            {
                CustomNamesAttribute attribute = info.GetCustomAttributes<CustomNamesAttribute>()[0];
                flags = (char)attribute.CustomFlags;
                Builder.Append(flags);
                if(attribute.UseVariableNameAsTitle)
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
            return intPair;
        }
    }

}
#endif


