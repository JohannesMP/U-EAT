using UnityEngine;
using System.Collections;
using ActionSystem;
public class ClampToBoundsOnEvent : OnEvent
{
    [CustomNames(new string[] { }, true, EditorNameFlags.UseTitle)]
    public Boolean3 ClampXYZ = new Boolean3(true, true, true);
    public Vector2 XBounds = new Vector2(-1000, 1000);
    public Vector2 YBounds = new Vector2(-1000, 1000);
    public Vector2 ZBounds = new Vector2(-1000, 1000);

    public override void OnEventFunc(EventData data)
    {
        var newPos = transform.position;
        if(ClampXYZ.x)
        {
            newPos.x = Mathf.Clamp(newPos.x, XBounds.x, XBounds.y);
        }
        if(ClampXYZ.y)
        {
            newPos.y = Mathf.Clamp(newPos.y, YBounds.x, YBounds.y);
        }
        if(ClampXYZ.z)
        {
            newPos.z = Mathf.Clamp(newPos.z, ZBounds.x, ZBounds.y);
        }

        transform.position = newPos;
    }
}
