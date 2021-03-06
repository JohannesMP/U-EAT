﻿using UnityEngine;
using System.Collections;
using ActionSystem;
public class EditLightOnEvent : EditOnEvent
{
    [ExposeProperty]
    public Vector3 TestProp
    {
        get
        {
            return transform.position;
        }
        set
        {
            if(value.x >= 15)
            {
                value.x = 15;
            }
            transform.position = value;
        }
    }
    public float Intensity = 1.0f;
    public float Duration = 1.0f;
    public Curve EasingCurve = new Ease();

    Light LightComp = null;
    // Use this for initialization

    public override void OnEventFunc(EventData data)
    {
        LightComp = GetComponent<Light>();
        if(!LightComp)
        {
            throw new System.Exception("Object " + name + "Has not component 'Light'.");
        }
        var Seq = Action.Sequence(Actions);
        //Action.Delay(Seq, Delay);
        Action.Property(Seq, LightComp.GetProperty(o => o.intensity), Intensity, Duration, EasingCurve);

        EditChecks(Seq);

    }


}
