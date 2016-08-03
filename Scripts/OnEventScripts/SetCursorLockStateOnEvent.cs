using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class SetCursorLockStateOnEvent : OnEvent
{

    public CursorLockMode LockState = CursorLockMode.None;


    public override void OnEventFunc(EventData data)
    {
        Cursor.lockState = LockState;
    }
}
