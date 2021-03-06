﻿/****************************************************************************/
/*!
\file   ActionProperty.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

Contains an action system that allows the user to interpolate between two
values over a specified duration of time with a given easing curve.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using ActionSystem.Internal;
using UnityEngine;

namespace ActionSystem
{

    public class ActionProperty<T> : ActionBase
    {
        //I have the value passed in by pointer to make it clear that this value IS going to change.
        public ActionProperty(Property<T> startVal, T endVal, double duration, Ease ease) : base()
        {
            if (duration == 0)
            {
                duration = 0.0000001;
            }
            StartVal = startVal.Get();
            CurrentVal = startVal;
            EndVal = endVal;
            EndTime = duration;
            EasingCurve = new Curve(ease);
        }
        public ActionProperty(Property<T> startVal, T endVal, double duration, Curve ease) : base()
        {
            if (duration == 0)
            {
                duration = 0.0000001;
            }
            StartVal = startVal.Get();
            CurrentVal = startVal;
            EndVal = endVal;
            EndTime = duration;
            EasingCurve = ease;
        }
        public ActionProperty(Property<T> startVal, T endVal, double duration, AnimationCurve curve) : base()
        {
            if (duration == 0)
            {
                duration = 0.0000001;
            }
            StartVal = startVal.Get();
            CurrentVal = startVal;
            EndVal = endVal;
            EndTime = duration;
            EasingCurve = new Curve(curve);
        }

        //Restarts the action, using the same memory location and the current starting value.
        public override void Restart()
        {
            Completed = false;
            CurrentTime = 0;
            StartVal = CurrentVal.Get();
        }
        //Restarts the action, using the same memory location but a different starting value.
        public void Restart(T startVal)
        {
            CurrentVal.Set(startVal);
            Restart();
        }


        public override void Update(double dt)
        {
            if (IsPaused() || IsCompleted())
            {
                return;
            }

            CurrentVal.Set(EasingCurve.Sample<T>(CurrentTime, StartVal, EndVal, EndTime));

            CurrentTime += dt;
            //Because the action is based entirely on duration, we do not need to worry
            //about precision errors.
            if (CurrentTime >= EndTime)
            {
                CurrentVal.Set(EndVal);
                CurrentTime = EndTime;
                Completed = true;
            }
        }

        //The user must restart to edit the starting value.
        public T StartVal { get; private set; }
        public T EndVal { get; set; }
        public Property<T> CurrentVal { get; private set; }


        public double CurrentTime { get; set; }
        public double EndTime { get; set; }

        Curve EasingCurve;
    }
}

