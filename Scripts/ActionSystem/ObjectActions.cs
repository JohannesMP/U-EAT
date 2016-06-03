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
    const bool IsVisibleInInspector = false;
    public ActionGroup Actions = new ActionGroup();
	
    void Awake()
    {
        if (!IsVisibleInInspector)
        {
            hideFlags = HideFlags.HideInInspector;
        }
    }

	// Update is called once per frame
	void Update()
    {
        Actions.Update(Time.smoothDeltaTime);
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

    public static T GetOrAddComponent<T>(this MonoBehaviour me) where T : Behaviour
    {
        return me.gameObject.GetOrAddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this GameObject me) where T : Behaviour
    {
        var comp = me.GetComponent<T>();
        if (comp == null)
        {
            comp = me.gameObject.AddComponent<T>();
        }
        return comp;
    }
}