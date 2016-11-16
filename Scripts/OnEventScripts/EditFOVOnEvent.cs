using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditFOVOnEvent : EditOnEvent
{
    public bool Additive = false;
    public float TargetFOV = 60;
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    protected Camera TargetCamera;
    protected ActionSequence Seq;

    public override void Awake()
    {
        base.Awake();
        if(!TargetCamera)
        {
            TargetCamera = GetComponent<Camera>();
        }
        
        Seq = Action.Sequence(Actions);
        
        
        if(Additive)
        {
            TargetFOV += TargetCamera.fieldOfView;
        }
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);
            
        }
        Action.Property(Seq, TargetCamera.GetProperty(cam => cam.fieldOfView), TargetFOV, Duration, EasingCurve);
        EditChecks(Seq);
    }

}
