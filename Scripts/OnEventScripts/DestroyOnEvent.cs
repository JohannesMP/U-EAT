using UnityEngine;
using System.Collections;
using ActionSystem;
public class DestroyOnEvent : OnEvent
{
    public float Delay = 0.001f;

    public override void Awake()
    {
        base.Awake();
        this.DelayedDispatch = true;
    }
    public override void OnEventFunc(EventData data)
    {
        this.DispatchEvent();
        DestroyObject(gameObject, Delay);
    }
}
