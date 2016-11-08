using UnityEngine;
using System.Collections;
using ActionSystem;
[ExecuteInEditMode]
public class DispatchEventOnCollision : MonoBehaviour
{
    bool Active { get { return enabled; } set { enabled = value; } }
    public enum CollisionModes
    {
        TriggerAndCollision,
        TriggerOnly,
        CollisionOnly
    }
    public CollisionModes CollisionMode = CollisionModes.TriggerAndCollision;
    public LayerMask CollisionMask;
    public Events DispatchEventName = Events.DefaultEvent;
    public GameObject EventTarget;
    CollisionEvent2D Data2D = new CollisionEvent2D();
    //CollisionEvent3D Data3D = new CollisionEvent3D();
    void Awake()
    {
        if(!EventTarget)
        {
            EventTarget = gameObject;
        }
    }


    public void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (!Active) return;
        Data2D = otherObj;
        if(CollisionMode == CollisionModes.CollisionOnly)
        {
            return;
        }
        if(CanHitObject(otherObj.gameObject))
        {
            EventTarget.DispatchEvent(DispatchEventName, Data2D);
        }
    }

    public void OnTriggerEnter(Collider otherObj)
    {
        if (!Active) return;
        //Data3D.StoredCollision = otherObj;
        if (CollisionMode == CollisionModes.CollisionOnly)
        {
            return;
        }
        if (CanHitObject(otherObj.gameObject))
        {
            EventTarget.DispatchEvent(DispatchEventName);
        }
    }

    public void OnCollisionEnter2D(Collision2D otherObj)
    {
        if (!Active) return;
        Data2D = otherObj;
        if (CollisionMode == CollisionModes.TriggerOnly)
        {
            return;
        }
        if (CanHitObject(otherObj.gameObject))
        {
            EventTarget.DispatchEvent(DispatchEventName, Data2D);
        }
    }

    public void OnCollisionEnter(Collision otherObj)
    {
        if (!Active) return;
        if (CollisionMode == CollisionModes.TriggerOnly)
        {
            return;
        }
        if (CanHitObject(otherObj.gameObject))
        {
            EventTarget.DispatchEvent(DispatchEventName);
        }
    }


    bool CanHitObject(GameObject obj)
    {
        return (CollisionMask.value & (1 << obj.layer)) != 0;
    }
}

