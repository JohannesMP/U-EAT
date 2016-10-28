using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum TimeScaleTypes
{
    GameTimeScale,
    GlobalTimeScale
}
public class CycleTimeScaleOnEvent : EditOnEvent
{
    public TimeScaleTypes TimeScaleType = TimeScaleTypes.GameTimeScale;
    public List<float> TimeScaleList = new List<float>(1);
    public List<float>.Enumerator NextTimeScale;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        if (TimeScaleType == TimeScaleTypes.GameTimeScale)
        {
            TimeScaleList.Add(Game.GameTimeScale);
        }
        else
        {
            TimeScaleList.Add(Time.timeScale);
        }
        
        NextTimeScale = TimeScaleList.GetEnumerator();
        NextTimeScale.MoveNext();
    }

    public override void OnEventFunc(EventData data)
    {
        if(TimeScaleType == TimeScaleTypes.GameTimeScale)
        {
            Game.GameTimeScale = NextTimeScale.Current;
        }
        else
        {
            Time.timeScale = NextTimeScale.Current;
        }
        if (!NextTimeScale.MoveNext())
        {
            NextTimeScale = TimeScaleList.GetEnumerator();
            NextTimeScale.MoveNext();
        }
    }
}
