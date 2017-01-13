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
    public Boolean2 UseTimeScaleOrPaused = new Boolean2(true, false);
    public bool UseTimeScale { get { return UseTimeScaleOrPaused.x; } set { UseTimeScaleOrPaused.x = value; } }
    public bool UseGamePaused { get { return UseTimeScaleOrPaused.y; } set { UseTimeScaleOrPaused.y = value; } }
    const bool IsVisibleInInspector = false;
    public ActionGroup Actions = new ActionGroup();
    public float IndividualTimeScale = 1;
    void Start()
    {
        if (!IsVisibleInInspector)
        {
            hideFlags = HideFlags.HideInInspector;
        }
        Game.GameSession.Connect("PausedStateChanged", OnPausedStateChanged);
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

	// Update is called once per frame
	void Update()
    {
        float dt = Time.smoothDeltaTime;
        if(!UseTimeScale)
        {
            dt /= Time.timeScale;
        }
        Actions.Update(dt * IndividualTimeScale);
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
    public static T GetOrAddComponent<T>(this Component me) where T : Component
    {
        return me.gameObject.GetOrAddComponent<T>();
    }
}