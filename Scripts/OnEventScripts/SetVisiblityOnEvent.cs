using UnityEngine;
using System.Collections;
using ActionSystem;

[RequireComponent(typeof(Renderer))]
public class SetVisiblityOnEvent : OnEvent
{
    public bool SetVisible = false;
    //public float Duration = 1;
    //public Curve EasingCurve = Ease.Linear;
    Renderer Renderer;
    // Use this for initialization
    public override void Awake ()
    {
        base.Awake();
        Renderer = GetComponent<Renderer>();
    }
	
    public override void OnEventFunc(EventData data)
    {
        Renderer.enabled = SetVisible;
    }


}

