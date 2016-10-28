using UnityEngine;
using System.Collections;
using ActionSystem;
public class MoveInDirectionOnEvent : OnEvent
{
    [SerializeField]
    GameObject ObjectToMoveTowardsProp;
    [ExposeProperty]
    public GameObject ObjectToMoveTowards
    {
        get
        {
            return ObjectToMoveTowardsProp;
        }
        set
        {
            if(ObjectToMoveTowardsProp == value)
            {
                return;
            }
            ObjectToMoveTowardsProp = value;
            UpdateAngleAndRotation();
        }
    }
    [CustomNames(new string[] { "OrientToObject", "UseTimeScale" , "UsePhysics"}, false, EditorNameFlags.None)]
    public Boolean3 OrientToObjectOrUseTimeScaleOrUsePhysics = new Boolean3(true, true, true);
    public bool OrientToObject { get { return OrientToObjectOrUseTimeScaleOrUsePhysics.x; } set { OrientToObjectOrUseTimeScaleOrUsePhysics.x = value; } }
    public bool UseTimeScale { get { return OrientToObjectOrUseTimeScaleOrUsePhysics.y; } set { OrientToObjectOrUseTimeScaleOrUsePhysics.y = value; } }
    public bool UsePhysics { get { return OrientToObjectOrUseTimeScaleOrUsePhysics.z; } set { OrientToObjectOrUseTimeScaleOrUsePhysics.z = value; } }
    public float RotateOffset = 0;
    public float Speed = 5;
    public Vector3 MoveDir = new Vector3();
    [CustomNames(new string[] { }, true, EditorNameFlags.UseTitle)]
    public Boolean3 FollowXYZ = new Boolean3(true, true, false);
    Rigidbody2D RigidBody;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        RigidBody = GetComponent<Rigidbody2D>();
    }

    public override void Start()
    {
        base.Start();
        UpdateAngleAndRotation();
    }

    public void UpdateAngleAndRotation()
    {
        if (ObjectToMoveTowardsProp)
        {
            MoveDir = (ObjectToMoveTowardsProp.transform.position - transform.position).normalized;
        }
        else
        {
            MoveDir = new Vector3();
        }
        if (OrientToObject)
        {
            RotateTowardsDir();
        }
    }

    void RotateTowardsDir()
    {
        var aimVec = MoveDir;
        aimVec = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(aimVec.y, aimVec.x) * 180 / Mathf.PI + RotateOffset);

        transform.eulerAngles = aimVec;

    }

    public override void OnEventFunc(EventData data)
    {
        var newPos = transform.position;
        var speed = Speed;
        if(!UsePhysics)
        {
            speed *= Time.smoothDeltaTime;
        }
        if(UseTimeScale)
        {
            speed *= Game.GameTimeScale;
        }
        var addVec = (MoveDir * speed);
        if (!FollowXYZ.x)
        {
            addVec.x = 0;
        }
        if (!FollowXYZ.y)
        {
            addVec.y = 0;
        }
        if (!FollowXYZ.z)
        {
            addVec.z = 0;
        }
        if(!UsePhysics)
        {
            transform.position = transform.position + addVec;
            transform.position = newPos;
        }
        else
        {
            RigidBody.velocity = addVec;
        }
    }
}
