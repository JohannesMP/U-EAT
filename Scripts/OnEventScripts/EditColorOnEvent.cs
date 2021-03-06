﻿using UnityEngine;
using System.Collections;
using ActionSystem;

public class EditColorOnEvent : EditOnEvent
{
    public Color TargetColor = new Color(1,1,1,1);
    public float Duration = 1;
    public Curve EasingCurve = Ease.Linear;

    SpriteRenderer Renderer;
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
        Renderer = GetComponent<SpriteRenderer>();
    }
	
    public override void OnEventFunc(EventData data)
    {
        if(Duration <= 0)
        {
            Renderer.color = TargetColor;
            if(DispatchEvents)
            {
                DispatchEvent();
            }
            return;
        }
        var Seq = Action.Sequence(Actions);
        Action.Property(Seq, Renderer.GetProperty(o => o.color), TargetColor, Duration, EasingCurve);
        EditChecks(Seq);
    }


}

