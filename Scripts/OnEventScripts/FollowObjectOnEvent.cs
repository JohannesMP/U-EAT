using UnityEngine;
using System.Collections;
using ActionSystem;
public class FollowObjectOnEvent : OnEvent
{
    GameObject ObjectToFollowProp;
    [ExposeProperty]
    public GameObject ObjectToFollow
    {
        get
        {
            return ObjectToFollowProp;
        }
        set
        {
            ObjectToFollowProp = value;
            if(ObjectToFollowProp)
            {
                TargetPos = ObjectToFollowProp.transform.position;
            }
            else
            {
                TargetPos = new Vector3();
            }
        }
    }
    public float Speed = 5;
    public bool UseTimeScale = true;
    public bool UpdateTargetPosition = true;
    public float MinDistance = 1;
    public bool FollowX = true;
    public bool FollowY = true;
    public bool FollowZ = true;
    public bool UseLerp = false;
    public bool UseBounds = false;
    public Vector2 XBounds = new Vector2(-1000, 1000);
    public Vector2 YBounds = new Vector2(-1000, 1000);
    Vector3 TargetPos = new Vector3();
    public float DistanceFromTarget { get; private set; }
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        
    }

    public override void Start()
    {
        base.Start();

        if (!ObjectToFollow)
        {
            throw new System.Exception("FollowObjectOnEvent needs an object to follow.");
        }
        
    }

    public override void OnEventFunc(EventData data)
    {
        Vector3 newPos = transform.position;
        if(UpdateTargetPosition)
        {
            TargetPos = ObjectToFollow.transform.position;
        }
        DistanceFromTarget = (TargetPos - gameObject.transform.position).magnitude;
        if(DistanceFromTarget > MinDistance && DistanceFromTarget > float.Epsilon)
        {
            var speed = Speed * Time.smoothDeltaTime;
            if(UseTimeScale)
            {
                speed *= Game.GameTimeScale;
            }
            if (UseLerp)
            {
                newPos = Vector3.Lerp(transform.position, ObjectToFollow.transform.position, speed);
            }
            else
            {
                var dir = (TargetPos - gameObject.transform.position) / DistanceFromTarget;
                newPos = transform.position + (dir * speed);
            }
        }
        
        newPos.x = Mathf.Clamp(newPos.x, XBounds.x, XBounds.y);
        newPos.y = Mathf.Clamp(newPos.y, YBounds.x, YBounds.y);

        if(!FollowX)
        {
            newPos.x = transform.position.x;
        }
        if (!FollowY)
        {
            newPos.y = transform.position.y;
        }
        if (!FollowZ)
        {
            newPos.z = transform.position.z;
        }
        transform.position = newPos;


    }
}
