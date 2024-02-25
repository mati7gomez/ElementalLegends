using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
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
    public const string UpDown = "Up";
}


public class JoystickDebug : MonoBehaviour
{
    [SerializeField] private Byte m_playerNumber; //Numero del jugador
    private bool
        m_squarePressed, m_crossPressed, m_circlePressed, m_trianglePressed,
        m_l1Pressed, m_r1Pressed, m_l2Pressed, m_r2Pressed,
        m_sharePressed, m_optionsPressed,
        m_l3Pressed, m_r3Pressed,
        m_psPressed, m_padPressed,
        m_leftPressed, m_rightPressed, m_upPressed, m_downPressed;

    private float
        m_l3xAxis, m_l3yAxis,
        m_l2Axis, m_r2Axis,
        m_lefRightAxis, m_upDownAxis;


    [SerializeField] private GameObject[] m_buttons; //Botones del joystick
    //0 = Square
    //1 = Cross
    //2 = Circle
    //3 = Triangle
    //4 = L1
    //5 = R1
    //6 = L2
    //7 = R2
    //8 = Share
    //9 = Options
    //10 = L3
    //11 = R3
    //12 = PS
    //13 = pad
    //14 = Left
    //15 = Right
    //16 = Up
    //17 = Down

    private Vector2 m_L3initialPos; //Posicion inicial del boton L3 del joystick
    private Vector2 m_R3initialPos; //Posicion inicial del boton R3 del joystick

    //-----------------//


    void Start()
    {
        DebugJoysticksActiveInGame();
        SetAnalogsInitialPosition();
    }


    //-----------------//


    private void Update()
    {
        m_squarePressed = GetJoystickButtonInfo(JoystickButtons.Square, (int)JoystickButtonsID.Square);
        m_crossPressed = GetJoystickButtonInfo(JoystickButtons.Cross, (int)JoystickButtonsID.Cross);
        m_circlePressed = GetJoystickButtonInfo(JoystickButtons.Circle, (int)JoystickButtonsID.Circle);
        m_trianglePressed = GetJoystickButtonInfo(JoystickButtons.Triangle, (int)JoystickButtonsID.Triangle);
        m_l1Pressed = GetJoystickButtonInfo(JoystickButtons.L1, (int)JoystickButtonsID.L1);
        m_r1Pressed = GetJoystickButtonInfo(JoystickButtons.R1, (int)JoystickButtonsID.R1);
        m_l2Pressed = GetJoystickButtonInfo(JoystickButtons.L2, (int)JoystickButtonsID.L2);
        m_r2Pressed = GetJoystickButtonInfo(JoystickButtons.R2, (int)JoystickButtonsID.R2);
        m_sharePressed = GetJoystickButtonInfo(JoystickButtons.Share, (int)JoystickButtonsID.Share);
        m_optionsPressed = GetJoystickButtonInfo(JoystickButtons.Options, (int)JoystickButtonsID.Options);
        m_l3Pressed = GetJoystickButtonInfo(JoystickButtons.L3, (int)JoystickButtonsID.L3);
        m_r3Pressed = GetJoystickButtonInfo(JoystickButtons.R3, (int)JoystickButtonsID.R3);
        m_psPressed = GetJoystickButtonInfo(JoystickButtons.PS, (int)JoystickButtonsID.PS);
        m_padPressed = GetJoystickButtonInfo(JoystickButtons.Pad, (int)JoystickButtonsID.Pad);
    }
    //-----------------//

    private void DebugJoysticksActiveInGame()
    {
        string[] joystickNames = Input.GetJoystickNames();

        for (int i = 0; i < joystickNames.Length; i++)
        {
            Debug.Log("Joystick " + i + ": " + joystickNames[i]);
        }
    }
    private void SetAnalogsInitialPosition()
    {
        m_L3initialPos = m_buttons[10].transform.position;
        m_R3initialPos = m_buttons[11].transform.position;
    }

    //-----------------//
    private bool GetJoystickButtonInfo(string buttonName, int buttomID)
    {
        bool pressed = Input.GetButton(buttonName + "P" + m_playerNumber);

        DebugJoystickButton(pressed, buttomID);
            
        return pressed;
    }
    private void DebugJoystickButton(bool pressedValue, int buttonID)
    {
        if (pressedValue)
            m_buttons[buttonID].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[buttonID].GetComponent<SpriteRenderer>().enabled = false;
    }
    private float GetJoystickAxisInfo(string buttonName, int buttonID, string axisXorY = "")
    {
        float axis = 0;
        if (axisXorY != "")
            axis = Input.GetAxis(buttonName + axisXorY.ToLower() + "P" + m_playerNumber);
        else
            axis = Input.GetAxis(buttonName + "P" + m_playerNumber);

        DebugJoystickAxis(0f);
        return axis;
    }
    private void DebugJoystickAxis(float axisValue)
    {

    }
    private void GetJoystickL3ButtonInfo()
    {
        bool pressed = Input.GetButton(JoystickButtons.L3 + "P" + m_playerNumber);
        float xInput = Input.GetAxis(JoystickButtons.L3 + "xP" + m_playerNumber);
        float yInput = Input.GetAxis(JoystickButtons.L3 + "yP" + m_playerNumber);

        DebugJoystickL3Movement(pressed, xInput, yInput);
        DebugJoystickL3Axis(xInput, yInput);
    }
    private void DebugJoystickL3Movement(bool pressed, float x, float y)
    {
        Vector2 joystickDir = new Vector2(x, y);
        Debug.DrawRay(m_buttons[10].transform.position, joystickDir);
        m_buttons[10].transform.position = m_L3initialPos + (joystickDir / 2);


        if (x != 0 || y != 0 || pressed)
            m_buttons[10].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[10].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void DebugJoystickL3Axis(float x, float y)
    {
        /*if (x < 0f) //si x es positivo
            m_mask[0].fillAmount = MathF.Abs(x);
        else if (x > 0) // si x es negativo
            m_mask[1].fillAmount = MathF.Abs(x);
        else
        {
            m_mask[0].fillAmount = MathF.Abs(x);
            m_mask[1].fillAmount = MathF.Abs(x);
        }*/
    }
    private void GetJoystickR3ButtonInfo()
    {
        bool pressed;
        float xInput;
        float yInput;

        xInput = Input.GetAxis(JoystickButtons.R3 + "xP" + m_playerNumber);
        yInput = Input.GetAxis(JoystickButtons.R3 + "yP" + m_playerNumber);
        pressed = Input.GetButton(JoystickButtons.R3 + "P" + m_playerNumber);

        DebugJoystickR3Movement(pressed, xInput, yInput);
    }
    private void DebugJoystickR3Movement(bool pressed, float x, float y)
    {
        Vector2 joystickDir = new Vector2(x, y);
        Debug.DrawRay(m_buttons[11].transform.position, joystickDir);
        m_buttons[11].transform.position = m_R3initialPos + (joystickDir / 2);


        if (x != 0 || y != 0 || pressed)
            m_buttons[11].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[11].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickPSButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.PS + "P" + m_playerNumber))
            m_buttons[12].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[12].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickPadButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.Pad + "P" + m_playerNumber))
            m_buttons[13].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[13].GetComponent<SpriteRenderer>().enabled = false;
    }


}
