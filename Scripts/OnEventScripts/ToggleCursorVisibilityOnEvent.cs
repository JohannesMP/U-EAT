﻿using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class ToggleCursorVisibilityOnEvent : OnEvent
{


    public override void OnEventFunc(EventData data)
    {
        Cursor.visible = !Cursor.visible;
    }

}

