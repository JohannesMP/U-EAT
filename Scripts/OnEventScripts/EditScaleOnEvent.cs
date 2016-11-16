using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditScaleOnEvent : EditOnEvent
{
    static Dictionary<GameObject, ActionSequence> ActionMap = new Dictionary<GameObject, ActionSequence>();
    public bool Additive = false;
    bool LocalScale = true;
    public bool ClearActions = false;
    public Vector3 TargetScale = new Vector3(1, 1, 1);
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;


    protected Transform TargetTransform;
    protected ActionSequence Seq;
    public override void Awake()
    {
        base.Awake();
        if (!TargetTransform)
        {
            TargetTransform = transform;
        }
        if (Additive)
        {
            if (LocalScale)
            {
                TargetScale.x *= TargetTransform.localScale.x;
                TargetScale.y *= TargetTransform.localScale.y;
                TargetScale.z *= TargetTransform.localScale.z;
            }
            //else
            //{

            //    TargetScale.x *= gameObject.transform.lossyScale.x;
            //    TargetScale.y *= gameObject.transform.lossyScale.y;
            //    TargetScale.z *= gameObject.transform.lossyScale.z;
            //}

        }
        if (ActionMap.ContainsKey(gameObject))
        {
            Seq = ActionMap[gameObject];
        }
        else
        {
            Seq = Action.Sequence(Actions);
            ActionMap.Add(gameObject, Seq);
        }
    }

    public override void OnEventFunc(EventData data)
    {
        if (ClearActions)
        {
            Seq.Clear();
        }

        Seq = Action.Sequence(Actions);
        ActionMap[gameObject] = Seq;


        if (LocalScale)
        {
            Action.Property(Seq, TargetTransform.GetProperty(o => o.localScale), TargetScale, Duration, EasingCurve);
        }
        //else
        //{
        //    Debug.Log(transform.GetProperty(o => o.lossyScale));
        //    Action.Property(Seq, transform.GetProperty(o => o.lossyScale), TargetScale, Duration, EasingCurve);
        //}

        EditChecks(Seq);
    }

    void OnDestroyed()
    {
        ActionMap.Remove(gameObject);
    }
}

