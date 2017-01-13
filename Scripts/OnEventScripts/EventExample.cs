using UnityEngine;
using System.Collections;

public class CustomEventData : EventData
{
    public int StoredInt;
    public CustomEventData(int val)
    {
        StoredInt = val;
    }
}

public class EventExample : MonoBehaviour
{
    public Events ListenEvent = Events.DefaultEvent;
    void Start()
    {
        EventSystem.EventConnect(this.gameObject, "HelloEvent", SayHello);
        EventSystem.EventSend(this.gameObject, "HelloEvent", new CustomEventData(5));
    }
    
    void SayHello(EventData data)
    {
        CustomEventData customData = (CustomEventData)data;
        Debug.Log("Hello World");
        Debug.Log(customData.StoredInt);
    }
}