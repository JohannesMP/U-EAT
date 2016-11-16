using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//public enum TimeScaleTypes
//{
//    GameTimeScale,
//    GlobalTimeScale,
//    Both
//}
public class CycleTimeScaleOnEvent : EditOnEvent
{
    public List<float> TimeScaleList = new List<float>(1);
    public List<float>.Enumerator NextTimeScale;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        TimeScaleList.Add(Game.GameTimeScale);

        NextTimeScale = TimeScaleList.GetEnumerator();
        NextTimeScale.MoveNext();
    }

    public override void OnEventFunc(EventData data)
    {
        Game.GameTimeScale = NextTimeScale.Current;
        if (!NextTimeScale.MoveNext())
        {
            NextTimeScale = TimeScaleList.GetEnumerator();
            NextTimeScale.MoveNext();
        }
    }
}


