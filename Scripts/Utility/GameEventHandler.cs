using UnityEngine;
using System.Collections;

public class GameEventHandler : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        Game.GameSession.DispatchEvent(Events.Create);
    }

    void Start ()
    {
        Game.GameSession.DispatchEvent(Events.Initialize);
    }

    // Update is called once per frame
    void Update ()
    {
        Game.GameSession.DispatchEvent(Events.LogicUpdate);
    }

    void LateUpdate()
    {
        Game.GameSession.DispatchEvent(Events.LateUpdate);
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            Game.GameSession.DispatchEvent(Events.ApplicationGainFocus);
        }
        else
        {
            Game.GameSession.DispatchEvent(Events.ApplicationLoseFocus);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {
            Game.GameSession.DispatchEvent(Events.ApplicationPause);
        }
        else
        {
            Game.GameSession.DispatchEvent(Events.ApplicationResume);
        }
    }

    void OnApplicationQuit()
    {
        Game.GameSession.DispatchEvent(Events.Destroy);
    }
}
