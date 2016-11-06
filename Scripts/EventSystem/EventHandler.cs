/****************************************************************************/
/*!
\file   EventHandler.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

This file contains the EventHandler class. This class manages what events
this object is listening to and what functions should be called when it
recieves an event.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System;

public class EventHandler : MonoBehaviour
{
    //Is this component visible in the inspector on every object that is listening to events?
    const bool IsVisibleInInspector = false;
    //This Dictionary stores all the different lists of functions this object is going to call when it recieves an event.
    Dictionary<string, List<Action<EventData>>> EventList = new Dictionary<string, List<Action<EventData>>>();

    void Awake()
    {
        Hide();
    }

    public void Hide()
    {
        if (!IsVisibleInInspector)
        {
            hideFlags = HideFlags.HideInInspector;
        }
    }
    //If the EventList does not have that event as a key, add it, then push the function to the list of functions to be called.
    public void EventConnect(string eventName, Action<EventData> func)
    {
        if (!EventList.ContainsKey(eventName))
        {
            EventList.Add(eventName, new List<Action<EventData>>());
        }
        EventList[eventName].Add(func);
    }
    //Removes ALL the functions with the given this pointer from the function list.
    public void EventDisconnect(string eventName, object thisPointer = null)
    {
        if (!EventList.ContainsKey(eventName))
        {
            return;
        }
        var functionList = EventList[eventName];
        for (int i = 0; i < functionList.Count; ++i)
        {
            if (functionList[i].Target.Equals(thisPointer))
            {
                functionList.RemoveAt(i);
                --i;
            }
        }
        //If there are no more functions to be called for that event, remove it from the list.
        if (functionList.Count == 0)
        {
            EventList.Remove(eventName);
        }
    }
    /*Removes the first equivalent function from the function list.
      If a function is connected to be called twice, it must be disconnected twice.*/
    public void EventDisconnect(string eventName, Action<EventData> function)
    {
        if (!EventList.ContainsKey(eventName))
        {
            return;
        }
        var functionList = EventList[eventName];
        for (int i = 0; i < functionList.Count; ++i)
        {
            if (functionList[i] == function)
            {
                functionList.RemoveAt(i);
                break;
            }
        }
        //If there are no more functions to be called for that event, remove it from the list.
        if (functionList.Count == 0)
        {
            EventList.Remove(eventName);
        }
    }
    //Calls all the functions associated with the given event, and passes them the given EventData.
    public void EventSend(string eventName, EventData eventData = null)
    {
        if (!EventList.ContainsKey(eventName))
        {
            return;
        }
        if (eventData == null)
        {
            eventData = EventSystem.DefaultEvent;
        }
        var functionList = EventList[eventName];
        for (var i = 0; i < functionList.Count; ++i)
        {
            var func = functionList[i];

            if (func.Method.IsStatic || !func.Target.Equals(null))
            {
                func(eventData);
            }
            else
            {
                //Remove any invalid functions.
                functionList[i] = functionList[functionList.Count - 1];
                functionList.RemoveAt(functionList.Count - 1);
                --i;
            }
        }
    }
    //Clears the Dictionary of all events.
    public void DisconnectAll()
    {
        EventList.Clear();
    }   
}
