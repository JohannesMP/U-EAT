/****************************************************************************/
/*!
\file   InputManager.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of the input manager class
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Mouse
{
    LEFT,
    RIGHT,
    MIDDLE,
    NONE,
}

public enum Buttons
{
    BUTTON_X,
    BUTTON_Y,
    BUTTON_A,
    BUTTON_B,
    BUTTON_DPAD_UP,
    BUTTON_DPAD_DOWN,
    BUTTON_DPAD_LEFT,
    BUTTON_DPAD_RIGHT,
    BUTTON_LEFT_SHOULDER,
    BUTTON_RIGHT_SHOULDER,
    BUTTON_START,
    BUTTON_BACK,
    BUTTON_LEFT_STICK,
    BUTTON_RIGHT_STICK,
    BUTTON_TOTAL
};

public struct GamepadStickValues
{
    public float XPos;
    public float YPos;
}

public struct GamepadTriggerValues
{
    public float LeftTrigger;

    public float RightTrigger;
}

//public class KeyboardEvent : EventData
//{
//    public KeyCode KeyCode;

//    public KeyboardEvent(KeyCode keyCode)
//    {
//        KeyCode = keyCode;
//    }
//}

//public class MouseEvent : EventData
//{
//    public Mouse Button;
//    public Vector2 Position;

//    public MouseEvent(Mouse button, Vector2 position)
//    {
//        Button = button;
//        Position = position;
//    }
//}

public class ScrollEvent : EventData
{
    public enum ScrollDirection
    {
        Up,
        Down
    }
    public ScrollDirection Direction;
    public ScrollEvent(ScrollDirection direction)
    {
        Direction = direction;
    }
}

//public class DragEvent : MouseEvent
//{
//    public Vector2 Delta;
//    public DragEvent(Mouse button, Vector2 position, Vector2 delta) : base(button, position)
//    {
//        Delta = delta;
//    }
//}

public static class InputManager
{
    //// list of Input States so we can determine different states.



#if UNITY_EDITOR
    static public Vector2 MousePosition
    {
        get
        {
            if(Application.isPlaying)
            {
                return Input.mousePosition;
            }
            return MousePositionProp;
        }
        set
        {
            if (Application.isPlaying)
            {
                MousePositionProp = value;
            }
        }
    }
    static public Vector3 MouseWorldMainPosition
    {
        get
        {
            if (Application.isPlaying)
            {
                return Camera.main.ScreenToWorldPoint(MousePosition);
            }
            return MouseWorldPositionProp;
        }
        set
        {
            if (Application.isPlaying)
            {
                MouseWorldPositionProp = value;
            }
        }
    }
    static public Vector2 MousePositionProp = new Vector3();
    static public Vector3 MouseWorldPositionProp = new Vector3();
    static public bool MouseScrolling { get; set; }
    static public bool MouseDragging = false;
    static readonly int InputSize = 512;
    static public bool[] LastInputState = new bool[InputSize];
    static public bool[] CurrentInputState = new bool[InputSize];
#else
    static public Vector2 MousePosition { get { return Input.mousePosition; } }
    static public Vector3 MouseWorldMainPosition { get { return Camera.main.ScreenToWorldPoint(MousePosition); } } //= new Vector3();
    //static KeyboardEvent KeyboardData = new KeyboardEvent(KeyCode.A);
    //static MouseEvent MouseData = new MouseEvent(Mouse.LEFT, new Vector2(0,0));
#endif

    static public Vector3 MouseWorldPosition(Camera cam = null)
    {
        if (!cam) cam = Camera.main;
        return cam.ScreenToWorldPoint(MousePosition);
    }

    static InputManager()
    {

    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    //static void Start()
    //{

    //}
    
    //static void OnLogicUpdate()
    //{

    //}

    /*********************************************************************
     Keyboard wrappers 
     *********************************************************************/
    static public bool IsKeyTriggered(KeyCode key)
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            return CurrentInputState[(int)key] && !LastInputState[(int)key];
        }
#endif
        return Input.GetKeyDown(key);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsKeyDown(KeyCode key)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return CurrentInputState[(int)key] && LastInputState[(int)key];
        }
#endif
        return Input.GetKey(key);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsKeyReleased(KeyCode key)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return !CurrentInputState[(int)key] && LastInputState[(int)key];
        }
#endif
        return Input.GetKeyUp(key);
    }

    /*********************************************************************
    Input Wrappers 
    *********************************************************************/

    /*************************************************************************/
    /*!
      \brief
        uses the gloabal list of input to check for actions
    */
    /*************************************************************************/
    static public bool IsInputTriggered(List<InputCodes> inputCodes)
    {
        if(inputCodes == null)
        {
            return false;
        }
        foreach(var i in inputCodes)
        {
            switch (i.InputType)
            {
                case InputTypes.Controller:
                {
                     //if(IsButtonTriggered((Button)i.Value))
                     //{
                     //       return true;
                     //}
                     break;
                }
                case InputTypes.Keyboard:
                {
                     if(IsKeyTriggered((KeyCode)i.Value))
                    {
                        return true;
                    }
                    break;
                }
                case InputTypes.Mouse:
                {
                     if( IsMouseTriggered((Mouse)i.Value))
                    {
                        return true;
                    }
                    break;
                }
                    
            }

        }

        return false;
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsInputDown(List<InputCodes> inputCodes)
    {
        foreach (var i in inputCodes)
        {
            switch (i.InputType)
            {
                case InputTypes.Controller:
                    {
                        //if (IsButtonDown((Button)i.Value))
                        //{
                        //    return true;
                        //}
                        break;
                    }
                case InputTypes.Keyboard:
                    {
                        if (IsKeyDown((KeyCode)i.Value))
                        {
                            return true;
                        }
                        break;
                    }
                case InputTypes.Mouse:
                    {
                        if (IsMouseDown((Mouse)i.Value))
                        {
                            return true;
                        }
                        break;
                    }

            }

        }

        return false;
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsInputReleased(List<InputCodes> inputCodes)
    {
        foreach (var i in inputCodes)
        {
            switch (i.InputType)
            {
                case InputTypes.Controller:
                    {
                        //if (IsButtonReleased((Button)i.Value))
                        //{
                        //    return true;
                        //}
                        break;
                    }
                case InputTypes.Keyboard:
                    {
                        if (IsKeyReleased((KeyCode)i.Value))
                        {
                            return true;
                        }
                        break;
                    }
                case InputTypes.Mouse:
                    {
                        if (IsMouseReleased((Mouse)i.Value))
                        {
                            return true;
                        }
                        break;
                    }

            }

        }

        return false;
    }

    /*********************************************************************
    Keyboard wrappers 
    *********************************************************************/
    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsMouseDown(Mouse button)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return CurrentInputState[(int)button] && LastInputState[(int)button];
        }
#endif
        return Input.GetMouseButton((int)button);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsMouseReleased(Mouse button)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return !CurrentInputState[(int)button] && LastInputState[(int)button];
        }
#endif
        return Input.GetMouseButtonUp((int)button);

    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsMouseTriggered(Mouse button)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            //Debug.Log(CurrentInputState[(int)button] + " : " + LastInputState[(int)button]);
            return CurrentInputState[(int)button] && !LastInputState[(int)button];
        }
#endif
        return Input.GetMouseButtonDown((int)button);
    }
}
