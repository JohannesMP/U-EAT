/****************************************************************************/
/*!
\file   ActionMath.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

Contains a generic math library for all the different easing equations
used in the ActionSystem.

The math used in the equations was found thanks to: http://gizma.com/easing/

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using System;

namespace ActionSystem
{

    public static class ActionMath<T>
    {
        //A wrapper around the GenericCalculator class so that I don't need to worry about whether or not I should interpolate using floats or doubles.
        static IArithmatic Arithmatic;
        //I only need to find the necessary functions once.
        static ActionMath()
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

        //Linear
        public static T Linear(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            return (Add(Div(Mult(change, (float)currentTime), (float)duration), startValue));
        }

        //Quadratic
        public static T QuadIn(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            currentTime /= duration;

            return Add(Mult(Mult(change, currentTime), currentTime), startValue);
        }

        public static T QuadOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= duration;
            //I multiply by -1 on the right hand side so the user does not need to implement the unary operator-.
            return Add(Mult(Mult(Mult(change, -1), currentTime), currentTime - 2), startValue);
        }

        public static T QuadInOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= (duration / 2.0);
            ;
            if (currentTime < 1)
            {
                return Add(Mult(Mult((Div(change, 2)), currentTime), currentTime), startValue);
            }

            currentTime -= 1;
            return Add(Mult(Div(Mult(change, -1), 2), (currentTime * (currentTime - 2)) - 1), startValue);
        }

        //Sinusoidal
        public static T SinIn(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            return Add(Add(Mult(Mult(change, -1), Math.Cos(currentTime / duration * (Math.PI / 2))), change), startValue);
        }

        public static T SinOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            return Add(Mult(change, Math.Sin(currentTime / duration * (Math.PI / 2))), startValue);
        }

        public static T SinInOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            change = Mult(change, -0.5);
            return Add(Mult(change, (Math.Cos(((Math.PI * currentTime) / duration)) - 1)), startValue);
        }

        //Exponential
        public static T ExpoIn(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            return Add(Mult(change, Math.Pow(2, (10 * (currentTime / duration - 1)))), startValue);
        }

        public static T ExpoOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            return Add(Mult(change, -Math.Pow(2, -10 * currentTime / duration) + 1), startValue);
        }

        public static T ExpoInOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return Add(Mult(Div(change, 2), Math.Pow(2, 10 * (currentTime - 1))), startValue);
            }
            --currentTime;

            return Add(Mult((Div(change, 2)), (-Math.Pow(2, -10 * currentTime) + 2)), startValue);
        }

        ////Circular
        public static T CircIn(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            currentTime /= duration;

            return Add(Mult((Mult(change, -1)), (Math.Sqrt(1 - currentTime * currentTime) - 1)), startValue);
        }

        public static T CircOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            currentTime /= duration;
            --currentTime;

            return Add(Mult(change, Math.Sqrt(1 - currentTime * currentTime)), startValue);
        }

        public static T CircInOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);
            currentTime /= (duration / 2.0);

            if (currentTime < 1)
            {

                return Add(Mult(Div((Mult(change, -1)), 2.0), Math.Sqrt(1 - currentTime * currentTime) - 1), startValue);
            }
            currentTime -= 2;

            return Add(Mult(Div(change, 2), Math.Sqrt(1 - currentTime * currentTime) + 1), startValue);
        }

        //Cubic

        public static T CubicIn(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= duration;
            return Add(Mult(Mult(Mult(change, currentTime), currentTime), currentTime), startValue);
        }

        public static T CubicOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= duration;
            currentTime -= 1;
            return Add(Mult(change, currentTime * currentTime * currentTime + 1), startValue);
        }

        public static T CubicInOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return Add(Mult(Div(change, 2), currentTime * currentTime * currentTime), startValue);
            }

            currentTime -= 2;
            return Add(Mult(Div(change, 2), currentTime * currentTime * currentTime + 2), startValue);
        }

        //Quartic
        public static T QuarticIn(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= duration;
            return Add(Mult(Mult(change, currentTime), currentTime * currentTime * currentTime), startValue);
        }

        public static T QuarticOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= duration;
            currentTime -= 1;
            return Add(Mult(Mult(change, -1), currentTime * currentTime * currentTime * currentTime - 1), startValue);
        }

        public static T QuarticInOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return Add(Mult(Div(change, 2), currentTime * currentTime * currentTime * currentTime), startValue);
            }

            currentTime -= 2;
            return Add(Mult(Div(Mult(change, -1), 2.0), currentTime * currentTime * currentTime * currentTime - 2), startValue);
        }

        //Quintic
        public static T QuinticIn(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= duration;
            return Add(Mult(Mult(change, currentTime), currentTime * currentTime * currentTime * currentTime), startValue);
        }

        public static T QuinticOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= duration;
            currentTime -= 1;
            return Add(Mult(change, currentTime * currentTime * currentTime * currentTime * currentTime + 1), startValue);
        }

        public static T QuinticInOut(Double currentTime, T startValue, T endValue, Double duration)
        {
            T change = Sub(endValue, startValue);

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return Add(Mult(Div(change, 2), currentTime * currentTime * currentTime * currentTime * currentTime), startValue);
            }

            currentTime -= 2;
            return Add(Mult(Div(change, 2), currentTime * currentTime * currentTime * currentTime * currentTime + 2), startValue);
        }


        static void GetAddSubFunctions(ref Func<T, T, T> addFunc, ref Func<T, T, T> subFunc)
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

        static void GetMultDivFunctions<T2>(ref Func<T, T2, T> multFunc, ref Func<T, T2, T> divFunc)
        {
            multFunc = GenericCalculator<T, T2, T>.MultiplyFunc;
            divFunc = GenericCalculator<T, T2, T>.DivideFunc;
            if (multFunc == null || divFunc == null)
            {
                //Int thinks that it is special.
                if(typeof(T) == typeof(int))
                {
                    GenericCalculator<int, double, int>.MultiplyFunc = (int lhs, double rhs) => { return (int)(lhs * rhs); };
                    GenericCalculator<int, double, int>.DivideFunc = (int lhs, double rhs) => { return (int)(lhs / rhs); };
                    GenericCalculator<int, float, int>.MultiplyFunc = (int lhs, float rhs) => { return (int)(lhs * rhs); };
                    GenericCalculator<int, float, int>.DivideFunc = (int lhs, float rhs) => { return (int)(lhs / rhs); };
                    multFunc = GenericCalculator<T, T2, T>.MultiplyFunc;
                    divFunc = GenericCalculator<T, T2, T>.DivideFunc;
                }
                else
                {
                    throw new Exception("In order to interpolate the type '" +
                            typeof(T).Name +
                            "', it must be able to both multiply with and divide with either floats or doubles.");
                }
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
                GetAddSubFunctions(ref AddFunc, ref SubFunc);
                GetMultDivFunctions(ref MultFunc, ref DivFunc);
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
                GetAddSubFunctions(ref AddFunc, ref SubFunc);
                GetMultDivFunctions(ref MultFunc, ref DivFunc);
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

        static public T Add(T lhs, T rhs)
        {
            return Arithmatic.Add(lhs, rhs);
        }
        static public T Sub(T lhs, T rhs)
        {
            return Arithmatic.Sub(lhs, rhs);
        }
        static public T Mult(T lhs, double rhs)
        {
            return Arithmatic.Mult(lhs, rhs);
        }
        static public T Div(T lhs, double rhs)
        {
            return Arithmatic.Div(lhs, rhs);
        }
    }//namespace Math

    //All the various ease types.
    public enum Ease
    {
        Linear,
        QuadIn,
        QuadInOut,
        QuadOut,
        SinIn,
        SinInOut,
        SinOut,
        ExpoIn,
        ExpoInOut,
        ExpoOut,
        CircIn,
        CircInOut,
        CircOut,
        CubicIn,
        CubicInOut,
        CubicOut,
        QuarticIn,
        QuarticInOut,
        QuarticOut,
        QntIn,
        QntInOut,
        QntOut
    };

}//namespace ActionSystem
