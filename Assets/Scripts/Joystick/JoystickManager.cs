using System;
using UnityEngine;
public enum JoystickButtonsID
{
    Square = 0,
    Cross = 1,
    Circle = 2,
    Triangle = 3,
    L1 = 4,
    R1 = 5,
    L2 = 6,
    R2 = 7,
    Share = 8,
    Options = 9,
    L3 = 10,
    R3 = 11,
    PS = 12,
    Pad = 13,
    Left = 14,
    Right = 15,
    Up = 16,
    Down = 17
}
public struct JoystickButtons
{
    public const string Square = "Square";
    public const string Cross = "Cross";
    public const string Circle = "Circle";
    public const string Triangle = "Triangle";
    public const string L1 = "L1";
    public const string R1 = "R1";
    public const string L2 = "L2";
    public const string R2 = "R2";
    public const string Share = "Share";
    public const string Options = "Options";
    public const string L3 = "L3";
    public const string R3 = "R3";
    public const string PS = "PS";
    public const string Pad = "Pad";
    public const string LeftRight = "LeftRight";
    public const string Right = "LeftRight";
    public const string UpDown = "UpDown";
    public const string Down = "UpDown";
}

public static class JoystickManager
{
    public static bool GetJoystickButton(string buttonName)
    {
        bool pressed = Input.GetButton(buttonName);
        return pressed;
    }
    public static bool GetJoystickButton(string buttonName, int playerNumber)
    {
        bool pressed = Input.GetButton(buttonName + "P" + playerNumber);
        return pressed;
    }
    public static float GetJoystickAxis(string buttonName, char axisXorY = 'x')
    {
        float axis = Input.GetAxis(buttonName + Char.ToLower(axisXorY));
        return axis;
    }
    public static float GetJoystickAxis(string buttonName, int playerNumber, char axisXorY = 'x')
    {
        float axis = Input.GetAxis(buttonName + axisXorY + "P" + playerNumber);
        return axis;
    }
}
