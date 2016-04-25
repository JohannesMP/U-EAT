using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditScaleOtherOnEvent : EditScaleOnEvent
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
        if(AdditiveSelf)
        {
            TargetScale.x *= transform.localScale.x;
            TargetScale.y *= transform.localScale.y;
            TargetScale.z *= transform.localScale.z;
        }
    }

    public override void OnEventFunc(EventData data)
    {
        //Seq.Clear();
        base.OnEventFunc(data);
    }
}
