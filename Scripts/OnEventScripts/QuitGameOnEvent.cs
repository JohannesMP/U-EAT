using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitGameOnEvent : OnEvent
{
    public override void OnEventFunc(EventData data)
    {
        Application.Quit();
    }
}
