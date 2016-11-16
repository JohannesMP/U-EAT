using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditRotationOtherOnEvent : EditRotationOnEvent
{
    public GameObject TargetObject;
    public bool AdditiveSelf = false;
    //USE START NOT AWAKE
    public override void Awake()
    {
        if (!TargetObject)
        {
            TargetObject = gameObject;
        }
        Actions = TargetObject.GetActions();
        TargetTransform = TargetObject.transform;
        base.Awake();
        if (AdditiveSelf)
        {
            TargetRotation += transform.eulerAngles;
        }
    }

    public override void OnEventFunc(EventData data)
    {
        //Seq.Clear();
        base.OnEventFunc(data);
    }
}
