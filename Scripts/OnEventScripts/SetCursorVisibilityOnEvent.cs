using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class SetCursorVisibilityOnEvent : OnEvent
{
    public bool CursorVisible = true;

    public override void OnEventFunc(EventData data)
    {
        Cursor.visible = CursorVisible;
    }

}

