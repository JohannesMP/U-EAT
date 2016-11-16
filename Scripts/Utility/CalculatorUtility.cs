/****************************************************************************/
/*!
    \author Joshua Biggs  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

// <summary>
/// This interface defines all of the operations that can be done in generic classes
/// These operations can be assigned to operators in class Number<T>
/// </summary>

/// <typeparam name="T">Type that
/// we will be doing arithmetic with</typeparam>

namespace ActionSystem
{
    struct QuaternionCalculator
    {
        static QuaternionCalculator()
        {
            
        }

        public Quaternion Decrement(Quaternion a)
        {
            --a.x;
            --a.y;
            --a.z;
            --a.w;
            return a;
        }

        static public Quaternion Sum(Quaternion a, Quaternion b)
        {

            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        static public Quaternion Difference(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public bool Compare(Quaternion a, Quaternion b)
        {
            return (a.x + a.y + a.z + a.w) > (b.x + b.y + b.z + b.w);
        }

        public bool Compare(Quaternion a, int b)
        {
            return (a.x + a.y + a.z + a.w) > (b);
        }

        static public Quaternion Multiply(Quaternion a, Double b)
        {
            a.x *= (float)b;
            a.y *= (float)b;
            a.z *= (float)b;
            a.w *= (float)b;
            return a;
        }

        static public Quaternion Multiply(Quaternion a, Quaternion b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;
            a.w *= b.z;
            return a;
        }

        static public Quaternion Divide(Quaternion a, Double b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            a.w /= (float)b;
            return a;
        }

        static public Quaternion Divide(Quaternion a, Quaternion b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            a.w /= b.z;
            return a;
        }

        public Quaternion Divide(Quaternion a, int b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            a.w /= (float)b;
            return a;
        }
    }

    struct ColorCalculator
    {
        public Color Decrement(Color a)
        {
            --a.r;
            --a.g;
            --a.b;
            --a.a;
            return a;
        }

        public Color Sum(Color a, Color b)
        {

            return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
        }

        public Color Difference(Color a, Color b)
        {
            return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
        }

        public bool Compare(Color a, Color b)
        {
            return (a.r + a.g + a.b + a.a) > (b.r + b.g + b.b + b.a);
        }

        public bool Compare(Color a, int b)
        {
            return (a.r + a.g + a.b + a.a) > (b);
        }

        public Color Multiply(Color a, Double b)
        {
            a.r *= (float)b;
            a.g *= (float)b;
            a.b *= (float)b;
            a.a *= (float)b;
            return a;
        }

        public Color Multiply(Color a, Color b)
        {
            a.r *= b.r;
            a.g *= b.g;
            a.b *= b.b;
            a.a *= b.b;
            return a;
        }

        public Color Divide(Color a, Double b)
        {
            a.r /= (float)b;
            a.g /= (float)b;
            a.b /= (float)b;
            a.a /= (float)b;
            return a;
        }

        public Color Divide(Color a, Color b)
        {
            a.r /= b.r;
            a.g /= b.g;
            a.b /= b.b;
            a.a /= b.b;
            return a;
        }

        public Color Divide(Color a, int b)
        {
            a.r /= (float)b;
            a.g /= (float)b;
            a.b /= (float)b;
            a.a /= (float)b;
            return a;
        }
    }

    public class Number<T>
    {
        static IArithmatic Arithmatic;
        static Number()
        {
            if (GenericCalculator<T, double, T>.MultiplyFunc == null || GenericCalculator<T, double, T>.DivideFunc == null)
            {
                Arithmatic = new FloatArithmatic();
            }
            else
            {
                Arithmatic = new DoubleArithmatic();
            }
        }
        readonly T Value;

        public Number(T value)
        {
            Value = value;
        }

        public static implicit operator Number<T>(T value)
        {
            return new Number<T>(value);
        }

        public static implicit operator T(Number<T> lhs)
        {
            return lhs.Value;
        }

        public static Number<T> operator +(Number<T> lhs, Number<T> rhs)
        {
            return Arithmatic.Add(lhs, rhs);
        }

        public static Number<T> operator -(Number<T> lhs, Number<T> rhs)
        {
            return Arithmatic.Sub(lhs, rhs);
        }

        public static Number<T> operator *(Number<T> lhs, double rhs)
        {
            return Arithmatic.Mult(lhs, rhs);
        }

        public static Number<T> operator *(double lhs, Number<T> rhs)
        {
            return rhs * lhs;
        }

        public static Number<T> operator /(Number<T> lhs, double rhs)
        {
            return Arithmatic.Div(lhs, rhs);
        }

        static void GetAddSubFunctions(out Func<T, T, T> addFunc, out Func<T, T, T> subFunc)
        {
            addFunc = GenericCalculator<T, T, T>.AddFunc;
            subFunc = GenericCalculator<T, T, T>.SubtractFunc;
            if (addFunc == null || subFunc == null)
            {
                throw new Exception("In order to interpolate the type '" +
                                            typeof(T).Name +
                                            "', it must be able to add and subtract with its own type.");
            }
        }

        static void GetMultDivFunctions<T2>(out Func<T, T2, T> multFunc, out Func<T, T2, T> divFunc)
        {
            multFunc = GenericCalculator<T, T2, T>.MultiplyFunc;
            divFunc = GenericCalculator<T, T2, T>.DivideFunc;
            if (multFunc == null || divFunc == null)
            {
                throw new Exception("In order to interpolate the type '" +
                                            typeof(T).Name +
                                            "', it must be able to both multiply with and divide with either floats or a doubles.");
            }
        }

        interface IArithmatic
        {
            T Add(T lhs, T rhs);
            T Sub(T lhs, T rhs);
            T Mult(T lhs, double rhs);
            T Div(T lhs, double rhs);
        }

        class DoubleArithmatic : IArithmatic
        {
            Func<T, T, T> AddFunc;
            Func<T, T, T> SubFunc;
            Func<T, double, T> MultFunc;
            Func<T, double, T> DivFunc;

            public DoubleArithmatic()
            {
                GetAddSubFunctions(out AddFunc, out SubFunc);
                GetMultDivFunctions(out MultFunc, out DivFunc);
            }

            public T Add(T lhs, T rhs)
            {
                return AddFunc(lhs, rhs);
            }
            public T Sub(T lhs, T rhs)
            {
                return SubFunc(lhs, rhs);
            }
            public T Mult(T lhs, double rhs)
            {
                return MultFunc(lhs, rhs);
            }
            public T Div(T lhs, double rhs)
            {
                return DivFunc(lhs, rhs);
            }
        }

        class FloatArithmatic : IArithmatic
        {
            Func<T, T, T> AddFunc;
            Func<T, T, T> SubFunc;
            Func<T, float, T> MultFunc;
            Func<T, float, T> DivFunc;

            public FloatArithmatic()
            {
                GetAddSubFunctions(out AddFunc, out SubFunc);
                GetMultDivFunctions(out MultFunc, out DivFunc);
            }

            public T Add(T lhs, T rhs)
            {
                return AddFunc(lhs, rhs);
            }
            public T Sub(T lhs, T rhs)
            {
                return SubFunc(lhs, rhs);
            }
            public T Mult(T lhs, double rhs)
            {
                return MultFunc(lhs, (float)rhs);
            }
            public T Div(T lhs, double rhs)
            {
                return DivFunc(lhs, (float)rhs);
            }
        }
    }
}


static public class Types
{
    static private Dictionary<String, Type> StoredTypes = new Dictionary<String, Type>();

    static Types()
    {
        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type t in a.GetTypes())
            {
                if (!HasType(t.Name))
                {
                    StoredTypes.Add(t.Name, t);
                }
            }
        }
    }

    static public Type GetType(String typeName)
    {
        return StoredTypes[typeName];
    }

    static public bool HasType(String typeName)
    {
        return StoredTypes.ContainsKey(typeName);
    }
}