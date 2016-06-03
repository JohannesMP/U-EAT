using UnityEngine;
using ActionSystem;

public class ActionExample : MonoBehaviour 
{
    public Curve EasingCurve = Ease.QuarticIn;
    public float InterpolatedValue { get; set; }
    ActionGroup Grp = new ActionGroup();
    
	void Start () 
    {
        //Create an ActionSequence inside of the ActionGroup.
        ActionSequence seq = Action.Sequence(Grp);
        //Add an ActionProperty to the sequence which will interpolate from 0 to 5 over 3 seconds.
        Action.Property(seq, this.GetProperty(val => val.InterpolatedValue), 5, 3, EasingCurve);
        Reset(seq);
    }
	
    void Reset(ActionSequence seq)
    {
        //Then, interpolate this objects position to the point [5, 3, 2] over 2 seconds.
        Action.Property(seq, transform.GetProperty(val => val.position), new Vector3(5, 3, 2), 2, Ease.QuadInOut);
        //Then, interpolate this objects position to the point [-5, 3, 2] over 2 seconds.
        Action.Property(seq, transform.GetProperty(val => val.position), new Vector3(-5, 3, 2), 2, Ease.QuadInOut);
        //Then call this function again with another sequence, causing a loop.
        Action.Call(seq, Reset, Action.Sequence(Grp));
    }

	void Update () 
    {
        Grp.Update(Time.smoothDeltaTime);
	}
}
