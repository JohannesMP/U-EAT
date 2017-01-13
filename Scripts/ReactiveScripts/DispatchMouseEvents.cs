using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DispatchMouseEvents : MonoBehaviour
{
    public Camera MouseCamera;
    GameObject BlockingObject = null;
    bool MouseOver = false;
	// Use this for initialization
	void Awake ()
    {
        MouseCamera = Camera.main;
        Update();
	}
	
	// Update is called once per frame
	void Update ()
    {
        BlockingObject = null;
        var ray = MouseCamera.ScreenPointToRay(InputManager.MousePosition);
        var hits = Physics.RaycastAll(ray);
        foreach(var i in hits)
        {
            if(i.transform.GetComponent<Blocking>())
            {
                if(i.transform.position.z < transform.position.z)
                {
                    if (MouseOver)
                    {
                        OnMouseExit();
                    }
                    BlockingObject = i.transform.gameObject;
                }
                
                break;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (BlockingObject) return;
        MouseOver = true;
        gameObject.DispatchEvent(Events.MouseEnter);
    }

    private void OnMouseExit()
    {
        if (BlockingObject) return;
        MouseOver = false;
        gameObject.DispatchEvent(Events.MouseExit);
    }

    private void OnMouseDown()
    {
        if (BlockingObject) return;
        gameObject.DispatchEvent(Events.MouseDown);
    }

    private void OnMouseUp()
    {
        if (BlockingObject) return;
        gameObject.DispatchEvent(Events.MouseUp);
    }

    private void OnMouseUpAsButton()
    {
        if (BlockingObject) return;
        gameObject.DispatchEvent(Events.MouseUpAsButton);
    }

    private void OnMouseOver()
    {
        if (BlockingObject) return;
        gameObject.DispatchEvent(Events.MouseOver);
    }

    private void OnMouseDrag()
    {
        if (BlockingObject) return;
        gameObject.DispatchEvent(Events.MouseDrag);
    }
}
