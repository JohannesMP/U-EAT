using UnityEngine;
using System.Collections;
using ActionSystem;
public class FollowObjectOnEvent : OnEvent
{
    [SerializeField]
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
    [CustomNames(new string[] { "Use TimeScale", "Update Target Position" }, false, EditorNameFlags.None)]
    public Boolean2 UseTimeScaleOrUpdateTargetPosition = new Boolean2(true, true);
    public bool UseTimeScale { get { return UseTimeScaleOrUpdateTargetPosition.x; } set { UseTimeScaleOrUpdateTargetPosition.x = value; } }
    public bool UpdateTargetPosition { get { return UseTimeScaleOrUpdateTargetPosition.y; } set { UseTimeScaleOrUpdateTargetPosition.y = value; } }
    public float MinDistance = 1;
    public bool FollowX { get{ return FollowXYZ.x; } set { FollowXYZ.x = value; } }
    public bool FollowY { get { return FollowXYZ.y; } set { FollowXYZ.y = value; } }
    public bool FollowZ { get { return FollowXYZ.z; } set { FollowXYZ.z = value; } }
    [CustomNames(new string[] { }, true, EditorNameFlags.Default  ^ EditorNameFlags.IndentBelow)]
    public Boolean3 FollowXYZ = new Boolean3(true, true, true);
    [CustomNames(new string[] { "Use Lerp", "Use Bounds" }, false, EditorNameFlags.None)]
    public Boolean2 UseLerpOrBounds = new Boolean2(false, false );
    public bool UseLerp { get { return UseLerpOrBounds.x; } set { UseLerpOrBounds.x = value; } }
    public Vector3 TargetOffset = new Vector3();
    public bool UseBounds { get { return UseLerpOrBounds.y; } set { UseLerpOrBounds.y = value; } }
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
        TargetPos = ObjectToFollow.transform.position + TargetOffset;
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
            TargetPos = ObjectToFollow.transform.position + TargetOffset;
        }
        var offset = TargetPos - gameObject.transform.position;
        
        if (!FollowX)
        {
            offset.x = 0;
        }
        if (!FollowY)
        {
            offset.y = 0;
        }
        if (!FollowZ)
        {
            offset.z = 0;
        }
        DistanceFromTarget = offset.magnitude;
        if((DistanceFromTarget > MinDistance) && (DistanceFromTarget > float.Epsilon))
        {
            //Time.fixedDeltaTime *= Time.timeScale;
            //Time.fixedDeltaTime = Time.deltaTime;
            
            var speed = Speed * Time.smoothDeltaTime;
            if(!UseTimeScale)
            {
                speed /=  Time.timeScale;
            }
            if (UseLerp)
            {
                newPos = Vector3.Lerp(transform.position, TargetPos, speed);
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
        //try reflecting for lead camera?
        transform.position = newPos;


    }
}
