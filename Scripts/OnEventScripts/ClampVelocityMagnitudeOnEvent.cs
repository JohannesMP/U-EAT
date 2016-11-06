using UnityEngine;
using System.Collections;
using ActionSystem;
public class ClampVelocityMagnitudeOnEvent : EditOnEvent
{
    public float MinVelocityMagnitude = 0.0f;
    float MinVelocitySqMagnitude { get { return MinVelocityMagnitude * MinVelocityMagnitude; } set { MinVelocityMagnitude = Mathf.Sqrt(value); } }
    public float MaxVelocityMagnitude = 5.0f;
    float MaxVelocitySqMagnitude { get { return MaxVelocityMagnitude * MaxVelocityMagnitude; } set { MaxVelocityMagnitude = Mathf.Sqrt(value); } }


    Rigidbody2D BodyComp;

    public override void Awake()
    {
        base.Awake();
        BodyComp = GetComponent<Rigidbody2D>();
    }

    public override void OnEventFunc(EventData data)
    {
        var velocitySqMag = BodyComp.velocity.sqrMagnitude;
        if(velocitySqMag < MinVelocitySqMagnitude)
        {
            BodyComp.velocity *= MinVelocitySqMagnitude / velocitySqMag;
            
        }
        else if(velocitySqMag > MaxVelocitySqMagnitude)
        {
            BodyComp.velocity *= MaxVelocitySqMagnitude / velocitySqMag;
        }
    }


}
