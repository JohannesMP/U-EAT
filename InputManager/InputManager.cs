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

public class KeyboardEvent : EventData
{
    public KeyCode KeyCode;

    public KeyboardEvent(KeyCode keyCode)
    {
        KeyCode = keyCode;
    }
}

public class MouseEvent : EventData
{
    public Mouse Button;
    public Vector2 Position;

    public MouseEvent(Mouse button, Vector2 position)
    {
        Button = button;
        Position = position;
    }
}

public static class InputManager
{
    //// list of Input States so we can determine different states.
    //static readonly int InputSize = 256;
    //static bool[] LastInputState = new bool[InputSize];
    //static bool[] CurrentInputState = new bool[InputSize];

    //static KeyboardEvent KeyboardData = new KeyboardEvent(KeyCode.A);
    //static MouseEvent MouseData = new MouseEvent(Mouse.LEFT, new Vector2(0,0));

    static InputManager()
    {

    }
    

    /*********************************************************************
     Keyboard wrappers 
     *********************************************************************/
    static public bool IsKeyTriggered(KeyCode Key)
    {
      return Input.GetKeyDown(Key);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsKeyDown(KeyCode Key)
    {
      return Input.GetKey(Key);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    static public bool IsKeyReleased(KeyCode Key)
    {
      return Input.GetKeyUp(Key);
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
        return Input.GetMouseButtonDown((int)button);
    }
}
