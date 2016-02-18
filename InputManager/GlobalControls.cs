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
    public static List<InputCodes> FireCandyControls = new List<InputCodes> { Mouse.LEFT };
    public static List<InputCodes> Forward = new List<InputCodes> { KeyCode.W, KeyCode.UpArrow };
    public static List<InputCodes> Backwards = new List<InputCodes> { KeyCode.S, KeyCode.DownArrow };
    public static List<InputCodes> TurnLeft = new List<InputCodes> { KeyCode.A, KeyCode.LeftArrow };
    public static List<InputCodes> TurnRight = new List<InputCodes> { KeyCode.D, KeyCode.RightArrow };
    public static List<InputCodes> StrafeLeft = new List<InputCodes> { KeyCode.Q };
    public static List<InputCodes> StrafeRight = new List<InputCodes> { KeyCode.E };
    public static List<InputCodes> Jump = new List<InputCodes> { KeyCode.Space };
    public static List<InputCodes> Run = new List<InputCodes> { KeyCode.LeftShift };
    public static List<InputCodes> ExitGame = new List<InputCodes> { KeyCode.Escape };
    public static List<InputCodes> DebugCursor = new List<InputCodes> { KeyCode.L };

    public static List<KeyCode> AlphabeticCodes
    {
        get
        {
            var codes = new List<KeyCode>();
            for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; ++i)
            {
                codes.Add((KeyCode)i);
            }
            return codes;
        }
    }

    public static List<KeyCode> NumericCodes = new List<KeyCode>() { KeyCode.Alpha0, KeyCode.Alpha1,
                                                                     KeyCode.Alpha2, KeyCode.Alpha3,
                                                                     KeyCode.Alpha4, KeyCode.Alpha5,
                                                                     KeyCode.Alpha6, KeyCode.Alpha7,
                                                                     KeyCode.Alpha8, KeyCode.Alpha9,
                                                                     KeyCode.Keypad0, KeyCode.Keypad1,
                                                                     KeyCode.Keypad2, KeyCode.Keypad3,
                                                                     KeyCode.Keypad4, KeyCode.Keypad5,
                                                                     KeyCode.Keypad6, KeyCode.Keypad7,
                                                                     KeyCode.Keypad8, KeyCode.Keypad9};

    public static List<KeyCode> SpecialCharacterCodes = new List<KeyCode>() { KeyCode.At, KeyCode.Ampersand,
                                                                              KeyCode.Asterisk, KeyCode.BackQuote,
                                                                              KeyCode.Backslash, KeyCode.Caret,
                                                                              KeyCode.Colon, KeyCode.Comma,
                                                                              KeyCode.Dollar, KeyCode.DoubleQuote,
                                                                              KeyCode.Equals, KeyCode.Exclaim,
                                                                              KeyCode.Greater, KeyCode.Hash,
                                                                              KeyCode.KeypadDivide, KeyCode.KeypadEquals,
                                                                              KeyCode.KeypadMinus, KeyCode.KeypadMultiply,
                                                                              KeyCode.KeypadPeriod, KeyCode.KeypadPlus,
                                                                              KeyCode.Space};

    public static List<KeyCode> AlphaNumericCodes
    {
        get
        {
            var codes = AlphabeticCodes;
            codes.AddRange(NumericCodes);
            return codes;
        }
    }
    public static List<KeyCode> AlphaNumericSpecialCodes
    {
        get
        {
            var codes = AlphabeticCodes;
            codes.AddRange(NumericCodes);
            codes.AddRange(SpecialCharacterCodes);
            return codes;
        }
    }
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



