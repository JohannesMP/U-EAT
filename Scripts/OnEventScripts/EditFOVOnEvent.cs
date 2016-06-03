using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditPositionOnEvent : EditOnEvent
{
    public bool Additive = false;
    public bool LocalPosition = true;
    public Vector3 TargetPosition = new Vector3();
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    protected Transform TargetTransform;
    // Use this for initialization
    protected ActionSequence Seq = null;
    //USE START NOT AWAKE
    public override void Awake()
    {
        base.Awake();
        if(!TargetTransform)
        {
            TargetTransform = transform;
        }
        
        Seq = Action.Sequence(Actions);
        
        
        if(Additive)
        {
            if(LocalPosition)
            {
                TargetPosition += TargetTransform.localPosition;
            }
            else
            {
                TargetPosition += TargetTransform.position;
            }
            
        }
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);
            
        }
        if(LocalPosition)
        {
            Action.Property(Seq, TargetTransform.GetProperty(o => o.localPosition), TargetPosition, Duration, EasingCurve);
        }
        else
        {
            Action.Property(Seq, TargetTransform.GetProperty(o => o.position), TargetPosition, Duration, EasingCurve);
        }

        EditChecks(Seq);
    }

}
