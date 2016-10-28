using UnityEngine;
using System.Collections;
using ActionSystem;
public class DestroyOnEvent : OnEvent
{
    public override void Awake()
    {
        base.Awake();
        this.DelayedDispatch = true;
    }
    public override void OnEventFunc(EventData data)
    {
        this.DispatchEvent();
        Destroy(gameObject);
    }

}
