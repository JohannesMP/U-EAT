using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SetTimeScaleOnEvent : OnEvent
{
    [Range(0, 100)]
    public float TimeScale = 0;
    public override void OnEventFunc(EventData data)
    {
        Time.timeScale = TimeScale;
    }
}

