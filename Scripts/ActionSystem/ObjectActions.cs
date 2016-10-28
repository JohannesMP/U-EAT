/****************************************************************************/
/*!
\file   ObjectActions.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

The ObjectActions component adds an ActionGroup to the gameobject that can be used 
by all the components on that gameobject.

The ActionExtensions can be used to GetOrAdd this component (or any component).
In addition, GetActions() can be called in order to get or add the ActionGroup on the Gameobject.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using UnityEngine;
using ActionSystem;

public class ObjectActions : MonoBehaviour
{
    [CustomNames(new string[] { "UseTimeScale", "UsePaused" }, false, EditorNameFlags.None)]
    public Boolean2 UseGameTimeScaleOrPaused = new Boolean2(false, false);
    public bool UseGameTimeScale { get { return UseGameTimeScaleOrPaused.x; } set { UseGameTimeScaleOrPaused.x = value; } }
    public bool UseGamePaused { get { return UseGameTimeScaleOrPaused.y; } set { UseGameTimeScaleOrPaused.y = value; } }
    const bool IsVisibleInInspector = false;
    public ActionGroup Actions = new ActionGroup();
    float TimeScaleProp = 1;
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
    void Start()
    {
        if (!IsVisibleInInspector)
        {
            hideFlags = HideFlags.HideInInspector;
        }
        Game.GameSession.Connect("PausedStateChanged", OnPausedStateChanged);
        Game.GameSession.Connect("TimeScaleChanged", OnTimeScaleChanged);
        if(UseGameTimeScale)
        {
            TimeScale = Game.GameTimeScale;
        }
    }

    void OnPausedStateChanged(EventData data)
    {
        if (!UseGamePaused)
        {
            return;
        }
        if (Game.Paused)
        {
            Actions.Pause();
        }
        else
        {
            Actions.Resume();
        }
    }

    void OnTimeScaleChanged(EventData data)
    {
        if(UseGameTimeScale)
        {
            TimeScale = Game.GameTimeScale;
        }
    }

	// Update is called once per frame
	void Update()
    {
        Actions.Update(Time.smoothDeltaTime * TimeScale);
	}
}

public static class ActionExtensions
{
    public static ActionGroup GetActions(this MonoBehaviour me)
    {
        return me.GetOrAddComponent<ObjectActions>().Actions;
    }

    public static ActionGroup GetActions(this GameObject me)
    {
        return me.GetOrAddComponent<ObjectActions>().Actions;
    }

    public static T GetOrAddComponent<T>(this MonoBehaviour me) where T : Component
    {
        return me.gameObject.GetOrAddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this GameObject me) where T : Component
    {
        var comp = me.GetComponent<T>();
        if (comp == null)
        {
            comp = me.gameObject.AddComponent<T>();
        }
        return comp;
    }
}