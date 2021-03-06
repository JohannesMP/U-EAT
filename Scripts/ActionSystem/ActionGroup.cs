﻿/****************************************************************************/
/*!
\file   ActionGroup.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2015
\brief

The ActionGroup class stores a group of actions which are all updated at the
same time. When all of the stored actions are complete, the group completes.
Groups can be placed inside of sequences.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using ActionSystem.Internal;
using System.Collections.Generic;
using System.Linq;

namespace ActionSystem
{
    public class ActionGroup : ActionBase
    {
        //A looping group does not clear 
        //itself when it completes
        public bool LoopingSequence {get; set; }
        private List<ActionBase> ActionQueue = new List<ActionBase>();
        private ActionBase CurrentAction;
        private int Index = 0;

        public ActionGroup(bool looping = false) : base()
        {
            LoopingSequence = looping;
            ActionQueue = new List<ActionBase>();
        }

        public ActionGroup(List<ActionBase> actionQueue, bool looping = false) : base()
        {
            LoopingSequence = looping;
            ActionQueue = actionQueue;
            CurrentAction = ActionQueue.First();
        }

        public bool IsEmpty()
        {
            return ActionQueue.Count == 0;
        }

        public override void Update(double dt)
        {
            if (IsPaused() || IsCompleted())
            {
                return;
            }

            Completed = true;
            while (Index < ActionQueue.Count)
            {
                CurrentAction = ActionQueue[Index];
                CurrentAction.Update(dt);
                if (CurrentAction.IsCompleted())
                {
                    ++Index;
                    continue;
                }
                //If we have not completed ALL of the actions in the group, the group has not finished.
                
                Completed = false;
                ++Index;
            }
            if (Completed)
            {
                if (LoopingSequence)
                {
                    Restart();
                }
            }
            Index = 0;
        }

        public void AddAction(ActionBase action)
        {
            Completed = false;
            ActionQueue.Add(action);
            CurrentAction = ActionQueue.First();
        }

        public override void Restart()
        {
            Completed = false;
            foreach (var i in ActionQueue)
            {
                i.Restart();
            }
            if(ActionQueue.Count > 0)
            {
                CurrentAction = ActionQueue.First();
            }  
        }

        public void Clear()
        {
            ActionQueue.Clear();
        }

        ~ActionGroup()
        {
            Clear();
        }
    }
}
