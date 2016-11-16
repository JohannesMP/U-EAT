using UnityEngine;
using System.Collections;
using System;
using ActionSystem;

[Serializable]
public class MegaButtonColors : MegaComponent
{
    [ExposeProperty]
    public Color MouseEnterColor
    {
        get
        {
            return MouseEnterColorComp.TargetColor;
        }
        set
        {
            MouseEnterColorComp.TargetColor = value;
        }
    }

    [ExposeProperty]
    public Color MouseExitColor
    {
        get
        {
            return MouseExitColorComp.TargetColor;
        }
        set
        {
            MouseExitColorComp.TargetColor = value;
        }
    }

    [ExposeProperty]
    public Color MouseUpColor
    {
        get
        {
            return MouseUpColorComp.TargetColor;
        }
        set
        {
            MouseUpColorComp.TargetColor = value;
        }
    }

    [ExposeProperty]
    public Color MouseDownColor
    {
        get
        {
            return MouseDownColorComp.TargetColor;
        }
        set
        {
            MouseDownColorComp.TargetColor = value;
        }
    }

    [MegaRegister]
    public EditColorOnEvent MouseEnterColorComp;
    [MegaRegister]
    public EditColorOnEvent MouseExitColorComp;
    [MegaRegister]
    public EditColorOnEvent MouseUpColorComp;
    [MegaRegister]
    public EditColorOnEvent MouseDownColorComp;
    // Use this for initialization
    void Start ()
    {
        var reactive = this.GetOrAddComponent<Reactive>();
        reactive.MouseEvents = true;
        MouseEnterColorComp.ListenEventName = Events.MouseEnter;
        MouseExitColorComp.ListenEventName = Events.MouseExit;
        MouseUpColorComp.ListenEventName = Events.MouseUpAsButton;
        MouseDownColorComp.ListenEventName = Events.MouseDown;
        MouseEnterColorComp.Duration = 0;
        MouseExitColorComp.Duration = 0;
        MouseUpColorComp.Duration = 0;
        MouseDownColorComp.Duration = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}