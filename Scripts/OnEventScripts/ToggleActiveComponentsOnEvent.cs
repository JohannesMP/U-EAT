using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ToggleActiveComponentsOnEvent : OnEvent
{
    public override void OnEventFunc(EventData data)
    {
        var comps = GetComponents<Behaviour>();
        foreach(var i in comps)
        {
            i.enabled = !i.enabled;
        }

    }
}
