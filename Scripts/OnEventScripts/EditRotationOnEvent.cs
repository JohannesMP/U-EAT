using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditRotationOnEvent : EditOnEvent
{

    public bool Additive = false;
    public bool LocalRotation = true;
    public Vector3 TargetRotation = new Vector3(0, 0, 0);
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    // Use this for initialization
    protected Transform TargetTransform;
    protected ActionSequence Seq;
    //USE START NOT AWAKE
    public override void Awake()
    {
        base.Awake();
        if (!TargetTransform)
        {
            TargetTransform = transform;
        }

        if (Additive)
        {
            if(LocalRotation)
            {
                TargetRotation += TargetTransform.localEulerAngles;
            }
            else
            {
                TargetRotation += TargetTransform.eulerAngles;
            }
            
        }
        Seq = Action.Sequence(Actions);
        
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);

        }
        if (LocalRotation)
        {
            Action.Property(Seq, TargetTransform.GetProperty(o => o.localEulerAngles), TargetRotation, Duration, EasingCurve);
        }
        else
        {
            Action.Property(Seq, TargetTransform.GetProperty(o => o.eulerAngles), TargetRotation, Duration, EasingCurve);
        }

        EditChecks(Seq);
    }
}
