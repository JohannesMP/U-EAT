using UnityEngine;
using System.Collections;
using ActionSystem;

public class OrientToMouse2DOnEvent : OnEvent
{

    // Use this for initialization
    public float Offset = 0.0f;
    public float LerpSpeed = 100f;
    [CustomNames(new string[]{"UseTimeScale", "UsePaused"}, false, EditorNameFlags.None)]
    public Boolean2 UseTimeScaleOrPaused = new Boolean2(true, true);

	// Update is called once per frame
	public override void OnEventFunc (EventData data)
    {
        if(UseTimeScaleOrPaused.y && Game.Paused)
        {
            return;
        }
        var aimVec = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimVec = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(aimVec.y, aimVec.x) * 180 / Mathf.PI + Offset);
        var speed = LerpSpeed * Time.smoothDeltaTime;
        if (UseTimeScaleOrPaused.x)
        {
            speed *= Game.GameTimeScale;
        }
        aimVec.z = Mathf.LerpAngle(transform.eulerAngles.z, aimVec.z, speed);

        transform.eulerAngles = aimVec;
    }
}


