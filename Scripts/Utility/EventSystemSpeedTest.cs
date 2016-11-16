using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ActionSystem;

public class EventSystemSpeedTest : MonoBehaviour
{
    public int ConnectCount = 10000;
    public int DispatchCount = 100;
    public int DisconnectCount = 10000;
    int FunctionCalls = 0;
    const double MillisecondsToSeconds = 1000;

    // Use this for initialization
	void Start ()
    {
        //GenericCalculator<Quaternion, Quaternion, Quaternion>.AddFunc = QuaternionCalculator.Sum;
        //GenericCalculator<Quaternion, Quaternion, Quaternion>.SubtractFunc = QuaternionCalculator.Difference;
        //GenericCalculator<Quaternion, double, Quaternion>.MultiplyFunc = QuaternionCalculator.Multiply;
        //GenericCalculator<Quaternion, double, Quaternion>.DivideFunc = QuaternionCalculator.Divide;

        //var startTime = Time.realtimeSinceStartup;
        //for (int i = 0; i < 1000000; ++i)
        //{
        //    ActionSystem.ActionMath<Quaternion>.Linear(0.5, Quaternion.Euler(23,32,532432), Quaternion.identity, 1);
        //}

        //startTime = Time.realtimeSinceStartup;
        //for (int i = 0; i < 1000000; ++i)
        //{
        //    ActionSystem.ActionMath.LinearOld(0.5, 5.0f, 6.0f, 1);
        //}
        //Debug.Log(Time.realtimeSinceStartup - startTime);
        //for (int i = 0; i < ConnectCount; ++i)
        //{
        //    gameObject.Connect(Events.DefaultEvent, OnEventFunc);
        //}
        //Debug.Log(ConnectCount.ToString() + " Connects: " + ((Time.realtimeSinceStartup - startTime) / MillisecondsToSeconds) + " seconds.");

        //startTime = Time.realtimeSinceStartup;
        //for (int i = 0; i < DispatchCount; ++i)
        //{
        //    gameObject.DispatchEvent(Events.DefaultEvent);
        //}
        //Debug.Log(DispatchCount.ToString() + " Dispatches: " + ((Time.realtimeSinceStartup - startTime) / MillisecondsToSeconds) + " seconds.");
        //Debug.Log("The function was called " + FunctionCalls + " times.");
        //startTime = Time.realtimeSinceStartup;
        //for (int i = 0; i < DisconnectCount; ++i)
        //{
        //    gameObject.Disconnect(Events.DefaultEvent, OnEventFunc);
        //}
        //Debug.Log(DisconnectCount.ToString() + " Disconnects: " + ((Time.realtimeSinceStartup - startTime) / MillisecondsToSeconds) + " seconds.");
    }


    void OnEventFunc(EventData data)
    {
        ++FunctionCalls;
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
