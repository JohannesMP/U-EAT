using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SetTimeScaleOnEvent : OnEvent
{
    [Range(0, 100)]
    public float TimeScale = 0;
    public TimeScaleTypes TimeScaleType = TimeScaleTypes.GameTimeScale;
    public override void OnEventFunc(EventData data)
    {
        if(TimeScaleType == TimeScaleTypes.Both)
        {
            Time.timeScale = TimeScale;
            Game.GameTimeScale = TimeScale;
        }
        else if(TimeScaleType == TimeScaleTypes.GameTimeScale)
        {
            Time.timeScale = TimeScale;
        }
        else
        {
            Game.GameTimeScale = TimeScale;
        }
        
    }
}
