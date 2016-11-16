using UnityEngine;
using System.Collections;

public class SetVelocityMagnitudeOnEvent : OnEvent
{
    public float VelocityMagnitude = 1.0f;
    // Use this for initialization
    Rigidbody2D Body2D;
    public override void Start ()
    {
        Body2D = GetComponent<Rigidbody2D>();
	}
	

    public override void OnEventFunc(EventData data)
    {
        Body2D.velocity = Body2D.velocity.normalized * VelocityMagnitude;
    }
}
