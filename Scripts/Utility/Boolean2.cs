using UnityEngine;
using System.Collections;
using System;
using System.Text;

[Flags]
public enum EditorNameFlags
{
    Default = UseTitle | IndentBelow | IndentBelowAtShortDistances,
    None = 0,
    UseTitle = 2,
    IndentBelow = 4,
    IndentBelowAtShortDistances = 8,
    //Broken
    ToggleRight = 16
}

[Serializable]
public class Boolean2
{
    public bool x;
    public bool y;
    public Boolean2(bool xVal = false, bool yVal = false)
    {
        x = xVal;
        y = yVal;
    }
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using System.Reflection;
    using System.Text;
    using UnityEditor;
    // IngredientDrawer
    [CustomPropertyDrawer(typeof(Boolean2))][CanEditMultipleObjects]
    public class Boolean2Drawer : PropertyDrawer
    {
        protected const char Seperator = '|';
        static StringBuilder Builder = new StringBuilder();
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var xRef = property.FindPropertyRelative("x");
            var yRef = property.FindPropertyRelative("y");
            var newBool = Draw2(position, new Boolean2(xRef.boolValue, yRef.boolValue), label, fieldInfo);
            xRef.boolValue = newBool.x;
            yRef.boolValue = newBool.y;
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
        static protected void DrawMultiBoolean(ref bool [] outputBools, Rect position, GUIContent content)
        {
            var names = SplitStringIntoNames(content.text);
            var totalBools = outputBools.Length;
            char drawFlags = names[0].ToCharArray()[0];
            bool useTitle = (drawFlags & (char)EditorNameFlags.UseTitle) > 0;
            bool indentBelow = (drawFlags & (char)EditorNameFlags.IndentBelow) > 0;
            bool indentBelowAtShortDistances = (drawFlags & (char)EditorNameFlags.IndentBelowAtShortDistances) > 0;
            bool toggleRight = (drawFlags & (char)EditorNameFlags.ToggleRight) > 0;
            Func<string, bool, GUIStyle, GUILayoutOption[], bool> drawFunc = EditorGUILayout.ToggleLeft;
            var layout = GUI.skin.label;
            layout.fontStyle = FontStyle.Normal;
            int namesIndex = 1;

            if(toggleRight)
            {
                drawFunc = EditorGUILayout.Toggle;
            }

            if (useTitle && namesIndex < names.Length)
            {
                EditorGUI.PrefixLabel(position, new GUIContent(names[namesIndex]), layout);
                ++namesIndex;
                position.x += InspectorValues.LabelWidth;
            }

            if (indentBelow && (!indentBelowAtShortDistances || (indentBelowAtShortDistances && position.width < InspectorValues.MultiLineToggle)))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(InspectorValues.Indent));
                var elementWidth = InspectorValues.MinElementWidth / totalBools;
                GUILayoutOption[] layoutOptions = new GUILayoutOption[] { GUILayout.MinWidth(elementWidth) };
                for (int i = 0; i < totalBools; ++i, ++namesIndex)
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
                    if(i == totalBools - 1)
                    {
                        elementWidth += InspectorValues.EdgePadding - InspectorValues.Indent;
                        outputBools[totalBools - 1] = drawFunc(name, outputBools[totalBools - 1], layout, layoutOptions);
                    }
                    else
                    {
                        outputBools[i] = drawFunc(name, outputBools[i], layout, layoutOptions);
                    }
                }
                
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (position.width > InspectorValues.MinWidthBeforeClip)
                {
                    position.x += (position.width - InspectorValues.MinWidthBeforeClip) / InspectorValues.WidthScaler;
                }
                position.width -= position.x;
                position.width /= totalBools;
                for (int i = 0; i < totalBools; ++i, ++namesIndex)
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
                    if(toggleRight)
                    {
                        outputBools[i] = EditorGUI.Toggle(position, name, outputBools[i], layout);
                    }
                    else
                    {
                        outputBools[i] = EditorGUI.ToggleLeft(position, name, outputBools[i], layout);
                    }
                    
                    position.x += position.width;
                }
            }
        }

        [ExposeDrawMethod]
        static public Boolean2 Draw2(Rect position, object eventsObj, GUIContent content, ICustomAttributeProvider info = null)
        {
            Boolean2 boolPair = (Boolean2)eventsObj;
            var boolArray = new bool[2]{ boolPair.x, boolPair.y };
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
            DrawMultiBoolean(ref boolArray, position, content);
            boolPair.x = boolArray[0];
            boolPair.y = boolArray[1];
            return boolPair;
        }
    }

}
#endif

public class CustomNamesAttribute : Attribute
{
    StringBuilder Builder = new StringBuilder();
    //If this is false, then the first variable name in the array should be the name of the group.
    public bool UseVariableNameAsTitle { get; private set; }
    public string CombinedName { get { return Builder.ToString(); } }
    public EditorNameFlags CustomFlags { get; private set; }

    public CustomNamesAttribute(string[] names, bool useVariableNameAsTitle = true, EditorNameFlags customFlags = EditorNameFlags.Default)
    {
        UseVariableNameAsTitle = useVariableNameAsTitle;
        CustomFlags = customFlags;
        foreach (var i in names)
        {
            Builder.Append('|');
            Builder.Append(i);
        }
    }
}