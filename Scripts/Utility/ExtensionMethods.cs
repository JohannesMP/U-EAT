/****************************************************************************/
/*!
    \author Joshua Biggs  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using ActionSystem;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class ExtensionMethods
{
    //Takes an element at the initialIndex and inserts it into the destination index by shifting all the other elements over.
    public static T[] Reorder<T>(this T[] me, int initialIndex, int destinationIndex)
    {
        if (initialIndex < 0 || initialIndex > me.Count() || destinationIndex < 0 || destinationIndex > me.Count())
        {
            throw new Exception("The given index is out of bounds.");
        }
        var val = me[initialIndex];
        if(initialIndex > destinationIndex)
        {
            for (int i = initialIndex; i > destinationIndex; --i)
            {
                me[i] = me[i - 1];
            }
        }
        else
        {
            for (int i = initialIndex; i < destinationIndex; ++i)
            {
                me[i] = me[i + 1];
            }
        }
        me[destinationIndex] = val;
        return me;
    }

    public static List<MethodInfo> GetMethods(this Type me, String methodName)
    {
        List<MethodInfo> info = new List<MethodInfo>();
        var methods = me.GetMethods();
        foreach (MethodInfo i in methods)
        {
            if (methodName == i.Name)
            {
                info.Add(i);
            }
        }

        return info;
    }

    public static MethodInfo GetMethod(this Type me, String methodName, Type returnType, params Type[] argumentTypes)
    {
        var methods = me.GetMethods();
        foreach (MethodInfo i in methods)
        {
            if (methodName == i.Name)
            {
                if (i.ReturnType == returnType)
                {

                    var args = i.GetParameters();
                    for (int j = 0; j < args.Count(); ++j)
                    {
                        if (j >= argumentTypes.Count())
                        {
                            break;
                        }
                        if (args[j].ParameterType != argumentTypes[j])
                        {
                            break;
                        }
                        if (j == (args.Count() - 1))
                        {
                            return i;
                        }
                    }

                }
            }

        }

        return null;
    }

    public static MethodInfo GetMethodByName(this Type me, String name)
    {
        var infoArray = me.GetMethods();
        foreach (var i in infoArray)
        {
            if (i.Name == name)
            {
                return i;
            }

        }
        return null;
    }

    static public Vector4 ToVector4(this Quaternion me)
    {
        return new Vector4(me.x, me.y, me.z, me.w);
    }

    static public Quaternion FromVector4(this Quaternion me, Vector4 vec)
    {
        me.x = vec.x;
        me.y = vec.y;
        me.z = vec.z;
        me.w = vec.w;
        return me;
    }

    static public void Destroy(this GameObject obj)
    {
        Destroy(obj);
    }

    public static GameObject GetSpace(this MonoBehaviour instance)
    {
        var obj = GameObject.FindGameObjectWithTag("Space");
        if (obj.Equals(null))
        {
            obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Space"));
        }
        return obj;
    }

    public static ActionSequence DispatchEventNextFrame(this GameObject instance, string eventName, EventData data = null)
    {
        var seq = ActionSystem.Action.Sequence(instance.GetActions());
        ActionSystem.Action.Delay(seq, Time.deltaTime / 2);
        ActionSystem.Action.Call(seq, EventSystem.DispatchEvent, instance, eventName, data);
        return seq;
    }

    public static int IndexOf<T>(this T[] instance, T param)
    {
        for (int i = 0; i < instance.Length; ++i)
        {
            if (instance[i].Equals(param))
            {
                return i;
            }
        }

        return -1;
    }
    public static bool Contains<T>(this T[] instance, T param)
    {
        for (int i = 0; i < instance.Length; ++i)
        {
            if (instance[i].Equals(param))
            {
                return true;
            }
        }

        return false;
    }

    public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider me, bool inherit = false) where T : Attribute
    {
        return (T[])me.GetCustomAttributes(typeof(T), inherit);
    }


    public static bool IsDerivedFrom(this Type me, Type otherType, bool includeSelf = true)
    {
        if (!includeSelf && otherType == me)
        {
            return false;
        }
        return otherType.IsAssignableFrom(me);
    }

    static public IEnumerable<Type> GetDerivedTypes(this Assembly assembly, Type baseType, bool includeSelf = false)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsDerivedFrom(baseType, includeSelf))
            {
                yield return type;
            }
        }
    }

    static public IEnumerable<Type> GetDerivedTypes(this Type baseType, bool includeSelf = false)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsDerivedFrom(baseType, includeSelf))
                {
                    yield return type;
                }
            }
        }
    }

    static public bool HasAttribute(this ICustomAttributeProvider me, Type attributeType, out Attribute[] attributes)
    {
        attributes = me.GetCustomAttributes(attributeType, true) as Attribute[];
        return attributes.Length > 0;
    }

    static public bool HasAttribute(this ICustomAttributeProvider me, Type attributeType)
    {
        var attributes = me.GetCustomAttributes(attributeType, true) as Attribute[];
        return attributes.Length > 0;
    }

    static public IEnumerable<Type> GetTypesWithAttribute(this Assembly assembly, Type attributeType)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.GetCustomAttributes(attributeType, true).Length > 0)
            {
                yield return type;
            }
        }
    }

    static public IEnumerable<Type> GetTypesWithAttribute(Type attributeType)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(attributeType, true).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }

    static public void LookAt2D(this Transform me, Transform target, float angleOffset = 0)
    {
        var aimVec = me.position - target.position;
        aimVec = new Vector3(me.eulerAngles.x, me.eulerAngles.y, Mathf.Atan2(aimVec.y, aimVec.x) * 180 / Mathf.PI + angleOffset);
        me.eulerAngles = aimVec;
    }
}