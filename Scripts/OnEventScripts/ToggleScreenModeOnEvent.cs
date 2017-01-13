using UnityEngine;
using System.Collections;
using ActionSystem;
public class ToggleScreenModeOnEvent : OnEvent
{   

    public override void Awake()
    {
        base.Awake();
    }
    public override void OnEventFunc(EventData data)
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
