/****************************************************************************/
/*!
\file   ActionDelay.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

The ActionDelay class simply waits for the specified amount of time. Primarily
useful in action sequences.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using ActionSystem.Internal;

namespace ActionSystem
{
    class ActionDelay : ActionBase
    {
        public ActionDelay(double duration = 0) : base()
        {
            EndTime = duration;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }

            CurrentTime += dt;
            if (CurrentTime >= EndTime)
            {
                Completed = true;
            }
        }

        public override void Restart()
        {
            Completed = false;
            CurrentTime = 0;
        }

        void Restart(double duration) 
        {
            EndTime = duration;
            Restart();
        }

        public double CurrentTime { get; private set; }
        public double EndTime { get; set; }
    }
}
