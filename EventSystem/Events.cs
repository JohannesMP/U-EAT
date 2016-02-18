﻿/****************************************************************************/
/*!
\author Joshua Biggs
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Events
{
    public static readonly String DefaultEvent = "DefaultEvent";
    public static readonly String KeyboardEvent = "KeyboardEvent";
    public static readonly String MouseUp = "MouseUp";
    public static readonly String MouseDown = "MouseDown";
    public static readonly String MouseEnter = "MouseEnter";
    public static readonly String MouseExit = "MouseExit";
    public static readonly String MouseDrag = "MouseDrag";
    public static readonly String MouseOver = "MouseOver";

    //
    public static readonly String Create = "CreateEvent";
    public static readonly String Initialize = "InitializeEvent";
    public static readonly String LogicUpdate = "LogicUpdate";
    public static readonly String LateUpdate = "LateUpdateEvent";
    public static readonly String Destroy = "DestroyEvent";

    public static readonly String ApplicationPause = "PauseEvent";
    public static readonly String ApplicationResume = "ResumeEvent";
    public static readonly String ApplicationFocus = "FocusEvent";

    //OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    public static readonly String OnCollisionEnter = "OnCollisionEnter";
    //Sent when an incoming collider makes contact with this object's collider (2D physics only).
    public static readonly String OnCollisionEnter2D = "OnCollisionEnter2D";
    //OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
    public static readonly String OnCollisionExit = "OnCollisionExit";
    //Sent when a collider on another object stops touching this object's collider (2D physics only).
    public static readonly String OnCollisionExit2D = "OnCollisionExit2D";
    //OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
    public static readonly String OnCollisionStay = "OnCollisionStay";
    //Sent each frame where a collider on another object is touching this object's collider (2D physics only).
    public static readonly String OnCollisionStay2D = "OnCollisionStay2D";

    public static readonly String OnPreRender = "OnPreRender";
    public static readonly String OnPostRender = "OnPostRender";

    public static readonly String OnParticleCollision= "OnParticleCollision";

    public static readonly String OnBecameVisible = "OnBecameVisible";
    public static readonly String OnBecameInvisible = "OnBecameInvisible";

    public static readonly String LightOff = "LightOff";
    public static readonly String LightOn = "LightOn";

    public static readonly String ValidateText = "ValidateTextEvent";
    public static readonly String LoadNextLevel = "LoadNextLevel";
    public static readonly String SpawnExitButton = "SpawnExitButton";
    public static readonly String MouseAppear = "MouseAppear";
    public static readonly String IndexAppear = "IndexAppear";
    public static readonly String IndexDisappear = "IndexDisappear";
    public static readonly String SpawnOptionsButton = "SpawnOptionsButton";
    //Nonstatic
    public string EventName = DefaultEvent;
    
    public Events() { }
    public Events(string eventName)
    {
        EventName = eventName;
    }

    public static implicit operator string(Events value)
    {
        return value.EventName;
    }
    
    public static implicit operator Events (string value)
    {
        return new Events(value);
    }
}

public class IntegerEvent : EventData
{
    public int value;
    public IntegerEvent(int intValue = 0)
    {
        value = intValue;
        
    }
}

public class StringEvent : EventData
{
    public string value;
    public StringEvent(string stringValue = "")
    {
        value = stringValue;

    }
}

public class CollisionEvent2D : EventData
{
    public Collision2D StoredCollision;
    public CollisionEvent2D(Collision2D collision = null)
    {
        StoredCollision = collision;
    }

    public static implicit operator Collision2D(CollisionEvent2D value)
    {
        return value.StoredCollision;
    }

    public static implicit operator CollisionEvent2D(Collision2D value)
    {
        return new CollisionEvent2D(value);
    }
}

public class CollisionEvent3D : EventData
{
    public Collision StoredCollision;
    public CollisionEvent3D(Collision collision = null)
    {
        StoredCollision = collision;
    }

    public static implicit operator Collision(CollisionEvent3D value)
    {
        return value.StoredCollision;
    }

    public static implicit operator CollisionEvent3D(Collision value)
    {
        return new CollisionEvent3D(value);
    }
}

public class FloatEvent : EventData
{
    public float value;
    public FloatEvent(float floatValue = 0.0f)
    {
        value = floatValue;
    }
}
#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;

    public static class InspectorValues
    {
        public static readonly float LabelWidth = 120;
        public static readonly float MinRectWidth = 340;
        //How fast the property scales with the width of the window.
        public static readonly float WidthScaler = 2.21f;
    }

    
    [CustomPropertyDrawer(typeof(Events))]
    public class EventPropertyDrawer : PropertyDrawer
    {
        static string[] EventNames;
        static string[] EventValues;

        bool AsString = false;

        float ToggleWidth = 70;

        static EventPropertyDrawer()
        {
            List<string> eventNames = new List<string>();
            List<string> eventValues = new List<string>();
            var info = typeof(Events).GetFields();
            foreach (var i in info)
            {
                if (i.IsStatic && i.FieldType == typeof(string))
                {
                    eventValues.Add(i.Name);
                    eventNames.Add(i.GetValue(i.FieldType) as string);
                }
            }

            EventNames = eventNames.ToArray();
            EventValues = eventValues.ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            var objectRef = property.FindPropertyRelative("EventName");
            var labelRect = new Rect(position.x, position.y, InspectorValues.LabelWidth, position.height);
            EditorGUI.LabelField(labelRect, property.name);
            var propStartPos = labelRect.position.x + labelRect.width;
            if (position.width > InspectorValues.MinRectWidth)
            {
                propStartPos += (position.width - InspectorValues.MinRectWidth) / InspectorValues.WidthScaler;
            }

            var toggleRect = new Rect(propStartPos, position.y, ToggleWidth, position.height);
            var eventRect = new Rect(toggleRect.position.x + toggleRect.width, position.y, position.width - (toggleRect.position.x + toggleRect.width) + 14, position.height);
            AsString = EditorGUI.ToggleLeft(toggleRect, "AsString", AsString);
            if (!AsString && EventNames.Length != 0)
            {
                var index = EventNames.IndexOf(objectRef.stringValue);

                if (index == -1)
                {
                    objectRef.stringValue = EditorGUI.TextField(eventRect, objectRef.stringValue);
                    AsString = true;
                    return;
                }
                
                objectRef.stringValue = EventNames[EditorGUI.Popup(eventRect, "", index, EventValues)];
                
            }
            else
            {
                objectRef.stringValue = EditorGUI.TextField(eventRect, objectRef.stringValue);
            }
        }
    }
}
#endif