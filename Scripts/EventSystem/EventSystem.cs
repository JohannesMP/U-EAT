/****************************************************************************/
/*!
\file   EventSystem.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

This file contains the static EventSystem class. The methods in this class
make interfacing with the EventHandler components easier.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using System;
using UnityEngine;


public static class EventSystem
{
    //An empty EventData object to be used when no data needs to be passed.
    public static EventData DefaultEvent = new EventData();
    
    public static void EventConnect(GameObject target, string eventName, Action<EventData> func)
    {
        var targetHandler = target.GetComponent<EventHandler>();
        if (!targetHandler)
        {
            targetHandler = target.AddComponent<EventHandler>();
            targetHandler.Hide();
        }
        targetHandler.EventConnect(eventName, func);
    }

    public static void EventDisconnect(GameObject target, String eventName, object thisPointer = null)
    {
        var targetHandler = target.GetComponent<EventHandler>();
        if (!targetHandler)
        {
            return;
        }
        targetHandler.EventDisconnect(eventName, thisPointer);
    }

    public static void EventDisconnect(GameObject target, string eventName, Action<EventData> function)
    {
        var targetHandler = target.GetComponent<EventHandler>();
        if (!targetHandler)
        {
            return;
        }
        targetHandler.EventDisconnect(eventName, function);
    }

    public static void EventSend(GameObject target, string eventName, EventData eventData = null)
    {
        var targetHandler = target.GetComponent<EventHandler>();
        if (!targetHandler)
        {
            return;
        }
        targetHandler.EventSend(eventName, eventData);
    }

    public static void DisconnectObject(GameObject target)
    {
        var targetHandler = target.GetComponent<EventHandler>();
        if (!targetHandler)
        {
            return;
        }
        targetHandler.DisconnectAll();
    }

    //Extension Methods to the GameObject class.
    public static void DispatchEvent(this GameObject target, string eventName, EventData eventData = null)
    {
        EventSend(target, eventName, eventData);
    }
    public static void Connect(this GameObject target, string eventName, Action<EventData> func)
    {
        EventConnect(target, eventName, func);
    }
    public static void Disconnect(this GameObject target, string eventName, Action<EventData> func)
    {
        EventDisconnect(target, eventName, func);
    }
    public static void Disconnect(this GameObject target, string eventName, object funcThisPointer)
    {
        EventDisconnect(target, eventName, funcThisPointer);
    }
}

//The default class that all custom events must inherit from.
public class EventData
{
    
}



