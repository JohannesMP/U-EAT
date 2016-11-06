using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
[InitializeOnLoad]
#endif

public class Game : MonoBehaviour
{
    static public float GameTimeScale
    {
        get
        {
            return Time.timeScale;
        }
        set
        {
            //Time.fixedDeltaTime = value * 0.02f;
            Time.timeScale = value;
            GameSession.DispatchEvent("TimeScaleChanged");
        }
    }
    [ExposeProperty]
    public float TimeScale
    {
        get
        {
            return TimeScaleProp;
        }
        set
        {
            TimeScaleProp = value;
        }
    }
    static public bool Paused
    {
        get
        {
            return PausedProp;
        }
        set
        {
            if(value != PausedProp)
            {
                PausedProp = value;
                GameSession.DispatchEvent("PausedStateChanged");
            }
        }
    }
    static public bool GameWasPaused { get; private set; }
    static public bool GameWasPlaying { get; private set; }

    static bool PausedProp = false;
    static float TimeScaleProp = 1;
    Game()
    {
#if UNITY_EDITOR
        EditorApplication.update += GlobalUpdate;
#endif
    }
    public static GameObject GameSession { get; private set; }
    static Game GameComp { get; set; }
    static GameObject GameEventHandler { get; set; }

    static GameObject HandlerResource;

    static Game()
    {
    }

    static void InitializeGame()
    {
        GameSession = Resources.Load<GameObject>("Prefabs/GameSession");
        if(!GameSession)
        {
            //return;
            throw new System.Exception("THE GAME MUST HAVE A GAMSESSION PREFAB!");
        }
        GameComp = GameSession.GetComponent<Game>();
        if (!GameComp)
        {
            throw new System.Exception("THE GAMESESSION MUST HAVE A 'GAME' COMPONENT!");
        }
        //GameSession.Connect(Events.Create, GameComp.OnGameCreate);
        //GameSession.Connect(Events.Initialize, GameComp.OnGameInitialize);
        GameSession.Connect(Events.LogicUpdate, GameComp.OnLogicUpdate);
        //GameSession.Connect(Events.LateUpdate, GameComp.OnGameLateUpdate);
        //GameSession.Connect(Events.Destroy, GameComp.OnGameDestroy);

        HandlerResource = Resources.Load<GameObject>("Prefabs/GameEventHandler");
        if (!HandlerResource)
        {
            throw new System.Exception("THE GAMESESSION MUST HAVE A 'GameEventHandler' RESOURCE!");
        }
        
    }

    [RuntimeInitializeOnLoadMethod]
    public static void InitializeGameEventHandler()
    {
        InitializeGame();
        if(!GameEventHandler)
        {
            GameEventHandler = Instantiate(HandlerResource);
        }
    }

    //void OnGameCreate(EventData data)
    //{
    //    Debug.Log("Game Started");
    //}

    //void OnGameInitialize(EventData data)
    //{
    //    Debug.Log("Game Initialized");
    //}

    void GlobalUpdate()
    {
        GameWasPlaying = Application.isPlaying;
    }

    void OnLogicUpdate(EventData data)
    {
        GameWasPaused = Paused;
    }


    //~Game()
    //{
    //    //GameSession.Disconnect(Events.LateUpdate, GameComp.OnGameLateUpdate);
    //}
    //void OnGameDestroy(EventData data)
    //{
    //    Debug.Log("Game Quitting");
    //}
}

