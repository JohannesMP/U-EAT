/****************************************************************************/
/*!
\file   SampleCurve.cs
\author Joshua Biggs
\par    email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

Contains a SampleCurve and Curve classes used to add eases to an Action/Interpolation.
The user can also specify their own custom easing function if they so wish.

The CustomCurve class can use Unity's animation curves for easing.

The 'Curve' class is a wrapper around both SampleCurves and CustomCurves and has its own 
custom property drawer in the inspector. The user can switch between using either a custom curve 
or a mathematical curve for easing if they want. Switching curve types is NOT destructive, and your 
previous curve selection will be saved.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using System;

using UnityEngine;

namespace ActionSystem
{
    [Serializable]
    public class Curve
    {
        public bool UseCustomCurve = false;
        public Ease CurveType = Ease.Linear;
        public AnimationCurve CustomCurveType = new AnimationCurve();
        BaseCurve StoredCurve;

        public Curve(Ease ease = Ease.Linear)
        {
            CurveType = ease;
        }

        public Curve(AnimationCurve curve)
        {
            UseCustomCurve = true;
            CustomCurveType = curve;
        }

        public T Sample<T>(Double currentTime, T startValue, T endValue, Double duration)
        {
            if (!UseCustomCurve)
            {
                if (StoredCurve == null || StoredCurve.GetType() != typeof(SampleCurve<T>))
                {
                    StoredCurve = new SampleCurve<T>(CurveType);
                }
                var curve = ((SampleCurve<T>)StoredCurve);
                curve.Sample = BaseCurve.GetEaseFromEnum<T>(CurveType);
                return curve.Sample(currentTime, startValue, endValue, duration);
            }
            else
            {
                if (StoredCurve == null || StoredCurve.GetType() != typeof(CustomCurve<T>))
                {
                    StoredCurve = new CustomCurve<T>(CustomCurveType);
                }
                return ((CustomCurve<T>)StoredCurve).Sample(currentTime, startValue, endValue, duration);
            }
        }

        public static implicit operator Ease(Curve value)
        {
            return value.CurveType;
        }

        public static implicit operator Curve(Ease value)
        {
            return new Curve(value);
        }

        public static implicit operator AnimationCurve(Curve value)
        {
            return value.CustomCurveType;
        }

        public static implicit operator Curve(AnimationCurve value)
        {
            return new Curve(value);
        }
    }

    [Serializable]
    public class BaseCurve
    {
        static public Func<double, T, T, double, T> GetEaseFromEnum<T>(Ease ease)
        {
            switch (ease)
            {
                case Ease.CircIn:
                    {
                        return ActionMath<T>.CircIn;
                    }
                case Ease.CircInOut:
                    {
                        return ActionMath<T>.CircInOut;
                    }
                case Ease.CircOut:
                    {
                        return ActionMath<T>.CircOut;
                    }
                case Ease.CubicIn:
                    {
                        return ActionMath<T>.CubicIn;
                    }
                case Ease.CubicInOut:
                    {
                        return ActionMath<T>.CubicInOut;
                    }
                case Ease.CubicOut:
                    {
                        return ActionMath<T>.CubicOut;
                    }
                case Ease.ExpoIn:
                    {
                        return ActionMath<T>.ExpoIn;
                    }
                case Ease.ExpoInOut:
                    {
                        return ActionMath<T>.ExpoInOut;
                    }
                case Ease.ExpoOut:
                    {
                        return ActionMath<T>.ExpoOut;
                    }
                case Ease.Linear:
                    {
                        return ActionMath<T>.Linear;
                    }
                case Ease.QntIn:
                    {
                        return ActionMath<T>.QuinticIn;
                    }
                case Ease.QntInOut:
                    {
                        return ActionMath<T>.QuinticInOut;
                    }
                case Ease.QntOut:
                    {
                        return ActionMath<T>.QuinticOut;
                    }
                case Ease.QuadIn:
                    {
                        return ActionMath<T>.QuadIn;
                    }
                case Ease.QuadInOut:
                    {
                        return ActionMath<T>.QuadInOut;
                    }
                case Ease.QuadOut:
                    {
                        return ActionMath<T>.QuadOut;
                    }
                case Ease.QuarticIn:
                    {
                        return ActionMath<T>.QuarticIn;
                    }
                case Ease.QuarticInOut:
                    {
                        return ActionMath<T>.QuarticInOut;
                    }
                case Ease.QuarticOut:
                    {
                        return ActionMath<T>.QuarticOut;
                    }
                case Ease.SinIn:
                    {
                        return ActionMath<T>.SinIn;
                    }
                case Ease.SinInOut:
                    {
                        return ActionMath<T>.SinInOut;
                    }
                case Ease.SinOut:
                    {
                        return ActionMath<T>.SinOut;
                    }
                default:
                    {
                        //No ease specified. Using linear.
                        Debug.Log("This ease is not yet implemented. Using Linear as default.");
                        return ActionMath<T>.Linear;
                    }

            }
        }
    }
    [Serializable]
    public class SampleCurve<T> : BaseCurve
    {
        public SampleCurve(Func<double, T, T, double, T> function)
        {
            Sample = function;
        }
        public SampleCurve(Ease ease = Ease.Linear)
        {
            Sample = GetEaseFromEnum<T>(ease);
        }
        //This is the delegate to the easing equation.
        //It is public because it doesn't really matter if the user changes it.
        public Func<double, T, T, double, T> Sample;
    }
    [Serializable]
    public class CustomCurve<T> : SampleCurve<T>
    {

        public CustomCurve(AnimationCurve animationCurve) : base()
        {
            Curve = animationCurve;
            Sample = UpdateCurve;
        }

        private T UpdateCurve(Double currentTime, T startValue, T endValue, Double duration)
        {
            return ActionMath<T>.Linear(Curve.Evaluate((float)(currentTime / duration)), startValue, endValue, 1);
        }

        public AnimationCurve Curve { get; private set; }
    }

}
#if UNITY_EDITOR
namespace CustomInspector
{
    using ActionSystem;
    using UnityEditor;
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(Curve), true)]
    public class SampleCurveDrawer : PropertyDrawer
    {
        const float ToggleWidth = 70;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enumProp = property.FindPropertyRelative("CurveType");
            var curveProp = property.FindPropertyRelative("CustomCurveType");
            var customCurveProp = property.FindPropertyRelative("UseCustomCurve");
            var curve = new Curve();
            curve.CurveType = (Ease)enumProp.enumValueIndex;
            curve.CustomCurveType = curveProp.animationCurveValue;
            curve.UseCustomCurve = customCurveProp.boolValue;
            EditorGUI.BeginProperty(position, label, property);
            curve = Draw(position, curve, label);
            EditorGUI.EndProperty();
            enumProp.enumValueIndex = (int)curve.CurveType;
            curveProp.animationCurveValue = curve.CustomCurveType;
            customCurveProp.boolValue = curve.UseCustomCurve;
            property.serializedObject.ApplyModifiedProperties();
        }

        [ExposeDrawMethod]
        static public Curve Draw(Rect position, object curveObj, GUIContent label)
        {
            Curve curve = curveObj as Curve;
            var labelRect = new Rect(position.x, position.y, InspectorValues.LabelWidth, position.height);
            //EditorGUILayout.LabelField(label);
            EditorGUI.LabelField(labelRect, label);
            //EditorGUILayout.PrefixLabel(label);
            var propStartPos = labelRect.position.x + labelRect.width;
            if (position.width > InspectorValues.MinRectWidth)
            {
                propStartPos += (position.width - InspectorValues.MinRectWidth) / InspectorValues.WidthScaler;
            }

            var toggleRect = new Rect(propStartPos, position.y, ToggleWidth, position.height);
            var enumRect = new Rect(toggleRect.position.x + toggleRect.width, position.y, position.width - (toggleRect.position.x + toggleRect.width) + 14, position.height);

            curve.UseCustomCurve = EditorGUI.ToggleLeft(toggleRect, "Custom", curve.UseCustomCurve);
            if (!curve.UseCustomCurve)
            {
                curve.CurveType = (Ease)EditorGUI.EnumPopup(enumRect, curve.CurveType);
            }
            else
            {
                curve.CustomCurveType = EditorGUI.CurveField(enumRect, curve.CustomCurveType);
            }
            return curve;
        }

    }
}
#endif
