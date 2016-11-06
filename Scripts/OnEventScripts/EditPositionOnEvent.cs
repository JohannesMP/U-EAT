using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditPositionOnEvent : EditOnEvent
{
    static Dictionary<GameObject, ActionSequence> ActionMap = new Dictionary<GameObject, ActionSequence>();
    [CustomNames(new string[] { "Additive", "Local Position" }, false, EditorNameFlags.None)]
    public Boolean2 AdditiveOrLocalPosition = new Boolean2(false, true);
    public bool Additive { get { return AdditiveOrLocalPosition.x; } set{ AdditiveOrLocalPosition.x = value; } }
    public bool LocalPosition { get { return AdditiveOrLocalPosition.y; } set { AdditiveOrLocalPosition.y = value; } }
    [CustomNames(new string[] { "Clear Actions", "Deactivate Until Finished" }, false, EditorNameFlags.None)]
    [ExposeProperty]
    public Boolean2 ClearActionsOrDeactivateUnilFinished
    {
        get
        {
            return new Boolean2(ClearActions, DeactivateUntilFinished);
        }
        set
        {
            ClearActions = value.x;
            DeactivateUntilFinished = value.y;
        }
    }
    [HideInInspector]
    public bool ClearActions = false;
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
        if(ActionMap.ContainsKey(gameObject))
        {
            Seq = ActionMap[gameObject];
        }
        else
        {
            Seq = Action.Sequence(Actions);
            ActionMap.Add(gameObject, Seq);
        }
        
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
        if(ClearActions)
        {
            Seq.Clear();
            Seq = Action.Sequence(Actions);
            ActionMap[gameObject] = Seq;
        }
        else if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);
            ActionMap[gameObject] = Seq;
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

    void OnDestroyed()
    {
        ActionMap.Remove(gameObject);
    }
}
