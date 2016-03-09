using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

public class Game : MonoBehaviour
{
    public static GameObject GameSession { get; private set; }
    static Game GameComp { get; set; }
    static GameObject GameEventHandler { get; set; }

    static GameObject HandlerResource;
    static void InitializeGame()
    {
        GameSession = Resources.Load<GameObject>("Prefabs/Core/GameSession");
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
        //GameSession.Connect(Events.LogicUpdate, GameComp.OnGameLogicUpdate);
        //GameSession.Connect(Events.Destroy, GameComp.OnGameDestroy);

        HandlerResource = Resources.Load<GameObject>("Prefabs/Core/GameEventHandler");
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

    //void OnGameLogicUpdate(EventData data)
    //{
    //    Debug.Log("Game Updating");
    //}

    //void OnGameDestroy(EventData data)
    //{
    //    Debug.Log("Game Quitting");
    //}
}
