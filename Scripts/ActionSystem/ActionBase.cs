/****************************************************************************/
/*!
\file   ActionBase.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

Contains the base ActionBase class that all derivitive action types inherit from.
All actions can be Paused, Resumed, Completed, and Restarted.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
namespace ActionSystem
{
    namespace Internal
    {
        public class ActionBase
        {
            //Booleans for different states of the current action.
            protected bool Paused = false;
            protected bool Completed = false;

            public void Pause() { Paused = true; }
            public void Resume() { Paused = false; }
            public bool IsPaused() { return Paused; }
            public bool IsCompleted() { return Completed; }

            //I have Update as a virtual function in order to make adding new action types incredibly simple.
            public virtual void Update(double dt) { }
            //Restart is virtual in the case that the user wants to ake their own custom 
            //derived Action class with different restart functionality.
            public virtual void Restart() { Completed = false; }

            //Protected so that this class is always a base class.
            protected ActionBase() { }
        };
    }//namespace Internal
} //namespace ActionSystem
