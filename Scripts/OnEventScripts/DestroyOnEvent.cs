﻿using UnityEngine;
using System.Collections;
using ActionSystem;
public class DestroyOnEvent : OnEvent
{

    public override void OnEventFunc(EventData data)
    {
        Destroy(gameObject);
    }

}