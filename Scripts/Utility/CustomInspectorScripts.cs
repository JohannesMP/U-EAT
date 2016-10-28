using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace CustomInspector
{
    public enum InspectorSortMode
    {
        PropertiesBeforeFields,
        FieldsBeforeProperties
    }

    static class CustomInspectorScripts
    {
        //A private Unity function that gets the drawer type for the given class type.
        public static Func<Type, Type> GetDrawerTypeForType;

        //Add a function with the type that is being drawn for as the key and the draw function as the value in order for that type to be drawn when exposing a property.
        public static Dictionary<Type, Func<Rect, object, GUIContent, ICustomAttributeProvider, object>> TypeDrawFunctions = new Dictionary<Type, Func<Rect, object, GUIContent, ICustomAttributeProvider, object>>();

        static CustomInspectorScripts()
        {
            //Unity should really make this public...
            var type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.ScriptAttributeUtility");
            MethodInfo info = type.GetMethod("GetDrawerTypeForType", BindingFlags.NonPublic | BindingFlags.Static);
            GetDrawerTypeForType = (Func<Type,Type>)Delegate.CreateDelegate(typeof(Func<Type, Type>), info);

            var customPropertyDrawers = ExtensionMethods.GetTypesWithAttribute(typeof(CustomPropertyDrawer));
            foreach(var drawer in customPropertyDrawers)
            {
                foreach(var method in drawer.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    if (method.HasAttribute(typeof(ExposeDrawMethod)))
                    {
                        if (method.ReturnType == null)
                        {
                            throw new Exception("Method of with the 'ExposeDrawMethod' attribute must have a return type.");
                        }
                        var func = (Func<Rect, object, GUIContent, ICustomAttributeProvider, object>)Delegate.CreateDelegate(typeof(Func<Rect, object, GUIContent, ICustomAttributeProvider, object>), method);
                        TypeDrawFunctions.Add(method.ReturnType, func);
                    }
                }
            }
        }

        

        public static InspectorSortMode SortMode = InspectorSortMode.PropertiesBeforeFields;
        //Draws the default inspector for THAT TYPE ONLY. Does NOT draw the inspectors for any base types.
        public static void DrawDefaultInspector(this Editor me, Type type, bool drawScriptReference = false)
        {
            if (drawScriptReference)
            {
                DrawScriptReference(me);
            }
            BindingFlags flags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;
            //List of all the members. We only care about the properties and the fields.
            MemberInfo[] fields;
            
            if (SortMode == InspectorSortMode.PropertiesBeforeFields)
            {
                fields = (type.GetProperties(flags) as MemberInfo[]).Concat(type.GetFields(flags)).ToArray();
            }
            else
            {
                fields = (type.GetFields(flags) as MemberInfo[]).Concat(type.GetProperties(flags)).ToArray();
            }
            for(int i = 0; i < fields.Count(); ++i)
            {
                var orderAttribute = fields[i].GetCustomAttributes<OrderAttribute>();
                if (orderAttribute.Length > 0)
                {
                    fields = fields.Reorder(i, orderAttribute[0].Order);
                    if(i < orderAttribute[0].Order)
                    {
                        --i;
                    }
                }
            }
            
            for(var index = 0; index < fields.Length; ++index)
            {
                var i = fields[index];
                if (i.MemberType != MemberTypes.Property && i.MemberType != MemberTypes.Field)
                {
                    continue;
                }
                if (i.HasAttribute(typeof(HideInInspector)))
                {
                    continue;
                }
                else
                {
                    var register = i.GetCustomAttributes<MegaComponent.MegaRegister>();
                    if(register.Count() > 0 && register[0].HideInInspector)
                    {
                        continue;
                    }
                }
                bool readOnly = i.HasAttribute(typeof(ReadOnlyAttribute));
                if (readOnly)
                {
                    GUI.enabled = false;
                }

                if (i.MemberType == MemberTypes.Field)
                {
                    var prop = me.serializedObject.FindProperty(i.Name);
                    if (prop != null)
                    {
                        EditorGUILayout.PropertyField(prop);
                        var depth = prop.depth + 1;
                        if (prop.isArray && prop.isExpanded)
                        {
                            while (prop.NextVisible(true))
                            {
                                if (prop.depth == 0)
                                {
                                    break;
                                }
                                else if(prop.depth > depth)
                                {
                                    continue;
                                }
                                EditorGUILayout.PropertyField(prop);
                            }
                        }
                    }
                }
                else
                {
                    if (!i.HasAttribute(typeof(ExposeProperty)))
                    {
                        continue;
                    }

                    ExposeProperty(me, i as PropertyInfo);
                }
                if (readOnly)
                {
                    GUI.enabled = true;
                }
            }
            me.serializedObject.ApplyModifiedProperties();
        }

        static readonly object[] EmptyObjectArray = new object[] { };
        static readonly Type[] EmptyTypeArray = new Type[0] { };
        //Will expose a property REGARDLESS OF ITS ATTRIBUTES
        public static void ExposeProperty(this Editor me, PropertyInfo info)
        {
            if (!info.CanRead)
            {
                throw new Exception("An exposed property must at least have a getter.");
            }
            if (!info.CanWrite)
            {
                GUI.enabled = false;
            }
            SerializedPropertyType type = GetSerializedPropertyTypeFromType(info.PropertyType);
            var name = AddSpacesBeforeCapitals(info.Name);
            switch (type)
            {
                case SerializedPropertyType.Integer:
                    {
                        var getter = me.target.GetPropertyGetter<object, int>(info);
                        int val = EditorGUILayout.IntField(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, int>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.Boolean:
                    {
                        var getter = me.target.GetPropertyGetter<object, bool>(info);
                        bool val = EditorGUILayout.Toggle(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, bool>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.Float:
                    {
                        var getter = me.target.GetPropertyGetter<object, float>(info);
                        float val = EditorGUILayout.FloatField(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, float>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.String:
                    {
                        var getter = me.target.GetPropertyGetter<object, string>(info);
                        string val = EditorGUILayout.TextField(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, string>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.Color:
                    {
                        var getter = me.target.GetPropertyGetter<object, Color>(info);
                        Color val = EditorGUILayout.ColorField(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, Color>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.ObjectReference:
                    {
                        var getter = me.target.GetPropertyGetter<object, UnityEngine.Object>(info);
                        UnityEngine.Object val = getter();
                        var drawerType = CustomInspectorScripts.GetDrawerTypeForType(info.PropertyType);
                        if (drawerType == null)
                        {
                            val = EditorGUILayout.ObjectField(name, getter(), info.PropertyType, true);
                        }
                        else
                        {
                            PropertyDrawer drawer = drawerType.GetConstructor(EmptyTypeArray).Invoke(EmptyObjectArray) as PropertyDrawer;
                            SerializedPropertyWrapper obj = ScriptableObject.CreateInstance<SerializedPropertyWrapper>();
                            obj.StoredObject = val;
                            var editor = Editor.CreateEditor(obj);
                            var iter = editor.serializedObject.GetIterator();

                            iter.NextVisible(true);//Go past Base
                            iter.NextVisible(true);//Go past m_script
                            var content = new GUIContent();
                            content.text = name;
                            GUILayout.Label("");
                            var pos = GUILayoutUtility.GetLastRect();
                            drawer.OnGUI(pos, iter, content);
                            val = iter.objectReferenceValue;
                        }
                        if (info.CanWrite)
                        {
                            //Must use a dynamic function call for this one. Otherwise I would have to check with all the different object types in Unity.
                            info.SetValue(me.target, val, EmptyObjectArray);
                        }
                    }
                    break;
                case SerializedPropertyType.LayerMask:
                    {
                        var getter = me.target.GetPropertyGetter<object, LayerMask>(info);
                        LayerMask val = EditorGUILayout.LayerField(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, LayerMask>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.Vector2:
                    {
                        var getter = me.target.GetPropertyGetter<object, Vector2>(info);
                        Vector2 val = EditorGUILayout.Vector2Field(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, Vector2>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.Vector3:
                    {
                        var getter = me.target.GetPropertyGetter<object, Vector3>(info);
                        Vector3 val = EditorGUILayout.Vector3Field(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, Vector3>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.Vector4:
                    {
                        var getter = me.target.GetPropertyGetter<object, Vector4>(info);
                        Vector4 val = EditorGUILayout.Vector4Field(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, Vector4>(info);
                            setter(val);
                        }
                    }
                    break;
                //case SerializedPropertyType.ArraySize:
                //{
                //    //Will work out later. Will need to be recursive most likely.
                //    //var getter = me.target.GetPropertyGetter<object, Vector4>(info);
                //    //Vector4 val = EditorGUILayout.(name, getter());
                //    //if (info.CanWrite)
                //    //{
                //    //    var setter = me.target.GetPropertySetter<object, Vector4>(info);
                //    //    setter(val);
                //    //}
                //}
                //break;
                case SerializedPropertyType.AnimationCurve:
                    {
                        var getter = me.target.GetPropertyGetter<object, AnimationCurve>(info);
                        AnimationCurve val = EditorGUILayout.CurveField(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, AnimationCurve>(info);
                            setter(val);
                        }
                    }
                    break;
                case SerializedPropertyType.Bounds:
                    {
                        var getter = me.target.GetPropertyGetter<object, Bounds>(info);
                        Bounds val = EditorGUILayout.BoundsField(name, getter());
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, Bounds>(info);
                            setter(val);
                        }
                    }
                    break;
                //case SerializedPropertyType.Gradient:
                //{
                //    //Will work out later
                //    //var getter = me.target.GetPropertyGetter<object, Gradient>(info);
                //    //Gradient val = EditorGUILayout.(name, getter());
                //    //if (info.CanWrite)
                //    //{
                //    //    var setter = me.target.GetPropertySetter<object, Gradient>(info);
                //    //    setter(val);
                //    //}
                //}
                //break;
                case SerializedPropertyType.Quaternion:
                    {
                        var getter = me.target.GetPropertyGetter<object, Quaternion>(info);
                        Quaternion val = new Quaternion();
                        val.FromVector4(EditorGUILayout.Vector4Field(name, getter().ToVector4()));
                        if (info.CanWrite)
                        {
                            var setter = me.target.GetPropertySetter<object, Quaternion>(info);
                            setter(val);
                        }
                    }
                    break;
                //The draw function for the type must be exposed using the ExposeDrawFunction attribute.
                case SerializedPropertyType.Generic:
                default:
                    {
                        var rect = EditorGUILayout.BeginVertical();
                        if (TypeDrawFunctions.ContainsKey(info.PropertyType))
                        {
                            GUILayout.Label("");
                            var val = info.GetGetMethod().Invoke(me.target, EmptyObjectArray);
                            var contentName = new GUIContent();
                            contentName.text = name;
                            val = TypeDrawFunctions[info.PropertyType](rect, val, contentName, info);
                            if (info.CanWrite)
                            {
                                info.SetValue(me.target, val, EmptyObjectArray);
                            }
                            EditorGUILayout.EndVertical();
                        }

                    }
                    break;
            }
            GUI.enabled = true;
        }

        const float ScriptReferencePixelPadding = 2.5f;
        const string UnityScriptRefName = "m_Script";
        public static void DrawScriptReference(Editor me)
        {
            var prop = me.serializedObject.FindProperty(UnityScriptRefName);
            if (prop.objectReferenceValue != null)
            {
                GUI.enabled = false;
            }
            EditorGUILayout.PropertyField(prop);
            GUI.enabled = true;
            GUILayout.Space(ScriptReferencePixelPadding);
        }

        [Serializable]
        public class SerializedPropertyWrapper : ScriptableObject
        {
            public UnityEngine.Object StoredObject;
        }

        public static string AddSpacesBeforeCapitals(string input)
        {
            int offset = 0;
            var builder = new StringBuilder(input);
            for (int i = 1; i < input.Length; ++i)
            {
                if (char.IsUpper(input[i]))
                {
                    builder.Insert(i + offset, " ");
                    ++offset;
                    ++i;
                }
            }
            return builder.ToString();
        }

        static public SerializedPropertyType GetSerializedPropertyTypeFromType(Type type)
        {
            if(type == typeof(int))
            {
                return SerializedPropertyType.Integer;
            }
            else if (type == typeof(bool))
            {
                return SerializedPropertyType.Boolean;
            }
            else if (type == typeof(float))
            {
                return SerializedPropertyType.Float;
            }
            else if (type == typeof(string))
            {
                return SerializedPropertyType.String;
            }
            else if (type == typeof(Color))
            {
                return SerializedPropertyType.Color;
            }
            else if (type.IsDerivedFrom(typeof(UnityEngine.Object)))
            {
                return SerializedPropertyType.ObjectReference;
            }
            else if (type == typeof(LayerMask))
            {
                return SerializedPropertyType.LayerMask;
            }
            else if (type == typeof(Vector2))
            {
                return SerializedPropertyType.Vector2;
            }
            else if (type == typeof(Vector3))
            {
                return SerializedPropertyType.Vector3;
            }
            else if (type == typeof(Vector4))
            {
                return SerializedPropertyType.Vector4;
            }
            else if (type == typeof(Rect))
            {
                return SerializedPropertyType.Rect;
            }
            else if (type.IsArray)
            {
                return SerializedPropertyType.ArraySize;
            }
            else if (type == typeof(AnimationCurve))
            {
                return SerializedPropertyType.AnimationCurve;
            }
            else if (type == typeof(Bounds))
            {
                return SerializedPropertyType.Bounds;
            }
            else if (type == typeof(Gradient))
            {
                return SerializedPropertyType.Gradient;
            }
            else if (type == typeof(Quaternion))
            {
                return SerializedPropertyType.Quaternion;
            }
            else
            {
                return SerializedPropertyType.Generic;
            }
        }
    } 
}
#endif
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
//Allows the property to be visible and editable in the inspector.
public class ExposeProperty : Attribute
{
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
//Allows the property to be visible and editable in the inspector.
public class ExposeDrawMethod : Attribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
//Allows the property to be visible and editable in the inspector.
public class ReadOnlyAttribute : Attribute
{
}


[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class OrderAttribute : Attribute
{
    public int Order { get; private set; }
    public OrderAttribute(int order = 0)
    {
        if(order < 0)
        {
            order = 0;
        }
        Order = order;
    }
}
