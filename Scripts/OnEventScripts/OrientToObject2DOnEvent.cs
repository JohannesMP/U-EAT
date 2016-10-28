using UnityEngine;
using System.Collections;
using ActionSystem;

public class OrientToObject2DOnEvent : OnEvent
{

    // Use this for initialization
    public GameObject TargetObject;
    public float Offset = 0.0f;
    public float LerpSpeed = 100f;
    public bool UseTimeScale = true;
    public override void Awake()
    {
        base.Awake();
        
    }

    public override void Start()
    {
        base.Start();
        if (!TargetObject)
        {
            throw new System.Exception("OrientToObject2DOnEvent needs an object to follow.");
        }
    }

	// Update is called once per frame
	public override void OnEventFunc (EventData data)
    {
        
        var aimVec = transform.position - TargetObject.transform.position;
        aimVec = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(aimVec.y, aimVec.x) * 180 / Mathf.PI + Offset);
        var speed = LerpSpeed * Time.smoothDeltaTime;
        if (UseTimeScale)
        {
            speed *= Game.GameTimeScale;
        }
        aimVec.z = Mathf.LerpAngle(transform.eulerAngles.z, aimVec.z, speed);
        transform.eulerAngles = aimVec;
    }
}


