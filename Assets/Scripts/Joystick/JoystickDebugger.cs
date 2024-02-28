using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickDebugger : MonoBehaviour
{
    [SerializeField] private Byte m_playerNumber; //Numero del jugador

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
    [SerializeField] private GameObject[] m_axis;
    //0 = L3x
    //1 = L3y
    //2 = R3x
    //3 = R3y
    //4 = L2
    //5 = R2
    //6 = Left
    //7 = Right
    //8 = Up
    //9 = Down

    private Vector2 m_L3initialPos; //Posicion inicial del boton L3 del joystick
    private Vector2 m_R3initialPos; //Posicion inicial del boton R3 del joystick

    //-----------------//
    void Start()
    {
        DebugJoysticksActiveInGame();
        SetAnalogsInitialPosition();
    }
    private void Update()
    {
        DebugJoystickButton(JoystickButtons.Square);
        DebugJoystickButton(JoystickButtons.Left);
        DebugJoystickButton(JoystickButtons.L3);
    }

    //-----------------//

    private void SetAnalogsInitialPosition()
    {
        m_L3initialPos = m_buttons[10].transform.position;
        m_R3initialPos = m_buttons[11].transform.position;
    }

    //-----------------//

    private void DebugJoystickButton(string buttonName)
    {
        bool hasAxis = GetJoystickButtonHasAxis(buttonName);

        if (hasAxis)
        {
            if (buttonName == JoystickButtons.L3 || buttonName == JoystickButtons.R3)
            {
                DebugJoystickAnalogs(buttonName);
            }
            else if (buttonName == JoystickButtons.L2 || buttonName == JoystickButtons.R2)
            {
                bool pressed = JoystickManager.GetJoystickButton(buttonName);
                float axisValue = JoystickManager.GetJoystickAxis(buttonName, 'x');
            }
            else
            {

            }
        }
        else
        {
            int ID = GetJoystickButtonID(buttonName);
            bool pressed = JoystickManager.GetJoystickButton(buttonName);
            DebugJoystickButtonOnly(pressed, ID);
        }
    }
    private void DebugJoystickButtonOnly(bool buttonPressed, int buttonID)
    {
        if (buttonPressed)
            m_buttons[buttonID].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[buttonID].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void DebugJoystickAnalogs(string buttonName)
    {
        int ID = GetJoystickButtonID(buttonName);
        bool buttonPressed = JoystickManager.GetJoystickButton(buttonName);
        float xAxis = JoystickManager.GetJoystickAxis(buttonName, 'x');
        float yAxis = JoystickManager.GetJoystickAxis(buttonName, 'y');
        HandleAnalogsMovement(xAxis, yAxis, ID);

        if (xAxis != 0 || yAxis != 0 || buttonPressed)
            m_buttons[ID].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[ID].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void HandleAnalogsMovement(float x, float y, int buttonID)
    {
        Vector2 joystickDir = new Vector2(x, y);

        Debug.DrawRay(m_buttons[buttonID].transform.position, joystickDir);
        m_buttons[buttonID].transform.position = m_L3initialPos + (joystickDir / 2);
    }
    private void DebugJoystickAxis(float axisValue, int buttonID)
    {

    }



    //-----------------//
    private int GetJoystickButtonID(string buttonName)
    {
        int buttonID = -1;
        switch (buttonName)
        {
            case "Square":
                return (int)JoystickButtonsID.Square;
            case "Cross":
                return (int)JoystickButtonsID.Cross;
            case "Circle":
                return (int)JoystickButtonsID.Circle;
            case "Triangle":
                return (int)JoystickButtonsID.Triangle;
            case "L1":
                return (int)JoystickButtonsID.L1;
            case "R1":
                return (int)JoystickButtonsID.R1;
            case "L2":
                return (int)JoystickButtonsID.L2;
            case "R2":
                return (int)JoystickButtonsID.R2;
            case "Share":
                return (int)JoystickButtonsID.Share;
            case "Options":
                return (int)JoystickButtonsID.Options;
            case "L3":
                return (int)JoystickButtonsID.L3;
            case "R3":
                return (int)JoystickButtonsID.R3;
            case "PS":
                return (int)JoystickButtonsID.PS;
            case "Pad":
                return (int)JoystickButtonsID.Pad;
            case "LeftRight":
                return (int)JoystickButtonsID.Left;
            case "UpDown":
                return (int)JoystickButtonsID.Up;

        }
        return buttonID;
    }
    private bool GetJoystickButtonHasAxis(string buttonName)
    {
        int buttonID = GetJoystickButtonID(buttonName);

        if (   buttonID == (int)JoystickButtonsID.L2 || buttonID == (int)JoystickButtonsID.R2
            || buttonID == (int)JoystickButtonsID.L3 || buttonID == (int)JoystickButtonsID.R3
            || buttonID == (int)JoystickButtonsID.Left || buttonID == (int)JoystickButtonsID.Right
            || buttonID == (int)JoystickButtonsID.Up || buttonID == (int)JoystickButtonsID.Down)
            return true;
        else 
            return false;
    }
    private void DebugJoysticksActiveInGame()
    {
        string[] joystickNames = Input.GetJoystickNames();
        for (int i = 0; i < joystickNames.Length; i++)
        {
            Debug.Log("Joystick " + i + ": " + joystickNames[i]);
        }
    }
    //-----------------//
}
