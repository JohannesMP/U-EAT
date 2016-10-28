/****************************************************************************/
/*!
    \author Joshua Biggs  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GlobalControls
{
    //public static List<InputCodes> DefaultControls = new List<InputCodes> { };
    public static List<InputCodes> Charge = new List<InputCodes> { Mouse.LEFT };
    public static List<InputCodes> OpenDoors = new List<InputCodes> { KeyCode.P };
    public static List<InputCodes> Forward = new List<InputCodes> { KeyCode.W, KeyCode.UpArrow };
    public static List<InputCodes> Backwards = new List<InputCodes> { KeyCode.S, KeyCode.DownArrow };
    public static List<InputCodes> TurnLeft = new List<InputCodes> { KeyCode.A, KeyCode.LeftArrow };
    public static List<InputCodes> TurnRight = new List<InputCodes> { KeyCode.D, KeyCode.RightArrow };
    public static List<InputCodes> StrafeLeft = new List<InputCodes> { KeyCode.Q };
    public static List<InputCodes> StrafeRight = new List<InputCodes> { KeyCode.E };
    public static List<InputCodes> Jump = new List<InputCodes> { KeyCode.Space };
    public static List<InputCodes> Boost = new List<InputCodes> { KeyCode.Space };
    public static List<InputCodes> Run = new List<InputCodes> { KeyCode.LeftShift };
    public static List<InputCodes> ReleaseAim = new List<InputCodes> { Mouse.RIGHT };
    public static List<InputCodes> TogglePauseMenu = new List<InputCodes> { KeyCode.Escape };
}
public enum InputTypes
{
    Mouse,
    Keyboard,
    Controller
}

public struct InputCodes
{
    public InputTypes InputType;
    public int Value;

    public InputCodes(KeyCode key)
    {
        InputType = InputTypes.Keyboard;
        Value = (int)key;
    }

    public InputCodes(Buttons button)
    {
        InputType = InputTypes.Controller;
        Value = (int)button;
    }

    public InputCodes(Mouse button)
    {
        InputType = InputTypes.Mouse;
        Value = (int)button;
    }

    public static implicit operator InputCodes(KeyCode a)
    {
        return new InputCodes(a);
    }

    public static implicit operator InputCodes(Buttons a)
    {
        return new InputCodes(a);
    }

    public static implicit operator InputCodes(Mouse a)
    {
        
        return new InputCodes(a);
    }
}


