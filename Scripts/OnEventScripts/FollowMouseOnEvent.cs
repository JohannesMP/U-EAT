using UnityEngine;
using System.Collections;
using ActionSystem;
public class FollowMouseOnEvent : OnEvent
{
    public float LerpSpeed = 5;
    [CustomNames(new string[] { "UseTimeScale", "UseBounds" }, false, EditorNameFlags.None)]
    public Boolean2 UseTimeScaleOrBounds = new Boolean2(true, false);
    public bool UsePaused = true;
    public bool UsePhysics = false;
    public Vector2 XBounds = new Vector2(-1000, 1000);
    public Vector2 YBounds = new Vector2(-1000, 1000);
    // Use this for initialization
    Rigidbody2D Body;
    public override void Awake()
    {
        base.Awake();
        if (UsePhysics)
        {
            Body = this.GetOrAddComponent<Rigidbody2D>();
        }
    }

    public override void OnEventFunc(EventData data)
    {
        if (UsePaused && Game.Paused)
        {
            return;
        }
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        var speed = LerpSpeed * Time.smoothDeltaTime;
        if (!UseTimeScaleOrBounds.x)
        {
            speed /= Game.GameTimeScale;
        }

        if (UsePhysics)
        {
            var offset = mousePos - transform.position;
            Body.velocity = offset * LerpSpeed;
        }
        else
        {
            Vector3 newPos = Vector3.Lerp(transform.position, mousePos, Mathf.Clamp01(speed));
            if (UseTimeScaleOrBounds.y)
            {
                newPos.x = Mathf.Clamp(newPos.x, XBounds.x, XBounds.y);
                newPos.y = Mathf.Clamp(newPos.y, YBounds.x, YBounds.y);
            }

            transform.position = newPos;
        }
    }
}
