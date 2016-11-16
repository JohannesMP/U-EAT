using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditCameraSizeOnEvent : EditOnEvent
{
    public Camera TargetCamera;
    public bool Additive = false;
    public float TargetSize = 10;
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    
    protected ActionSequence Seq;

    public override void Awake()
    {
        base.Awake();
        if(!TargetCamera)
        {
            TargetCamera = GetComponent<Camera>();
        }
        
        Seq = Action.Sequence(Actions);
        
        
        
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);
            
        }
        var targetSize = TargetSize;
        if (Additive)
        {
             targetSize = TargetSize + TargetCamera.orthographicSize;
        }
        Action.Property(Seq, TargetCamera.GetProperty(cam => cam.orthographicSize), targetSize, Duration, EasingCurve);
        EditChecks(Seq);
    }

}
