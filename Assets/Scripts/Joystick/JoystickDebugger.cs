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

    private struct JoystickButtonsObj
    {
        GameObject Square;
        GameObject Cross;
    }
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
        DebugJoystickButton(JoystickButtons.Square, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Cross, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Circle, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Triangle, m_playerNumber);
        DebugJoystickButton(JoystickButtons.L1, m_playerNumber);
        DebugJoystickButton(JoystickButtons.R1, m_playerNumber);
        DebugJoystickButton(JoystickButtons.L2, m_playerNumber);
        DebugJoystickButton(JoystickButtons.R2, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Share, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Options, m_playerNumber);
        DebugJoystickButton(JoystickButtons.L3, m_playerNumber);
        DebugJoystickButton(JoystickButtons.R3, m_playerNumber);
        DebugJoystickButton(JoystickButtons.PS, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Pad, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Left, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Right, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Up, m_playerNumber);
        DebugJoystickButton(JoystickButtons.Down, m_playerNumber);


    }

    //-----------------//

    private void SetAnalogsInitialPosition()
    {
        m_L3initialPos = m_buttons[10].transform.position;
        m_R3initialPos = m_buttons[11].transform.position;
    }

    //-----------------//

    private void DebugJoystickButton(string buttonName, int playerNumber)
    {
        if (GetJoystickButtonHasAxis(buttonName))
        {
            if (GetJoystickButtonIsAnalog(buttonName))
            {
                bool buttonPressed = JoystickManager.GetJoystickButton(buttonName, playerNumber);
                int buttonID = GetJoystickButtonID(buttonName);
                int axisID = GetJoystickAxisID(buttonName);
                float xAxis = JoystickManager.GetJoystickAxis(buttonName, playerNumber, 'x');
                float yAxis = JoystickManager.GetJoystickAxis(buttonName, playerNumber, 'y');
                DebugJoystickAnalogs(buttonPressed, buttonID, xAxis, yAxis);
                DebugJoystickAxis(xAxis, 'x', axisID);
                DebugJoystickAxis(yAxis, 'y', axisID + 1);
            }
            else if (GetJoystickButtonIsTrigger(buttonName))
            {
                bool buttonPressed = JoystickManager.GetJoystickButton(buttonName, playerNumber);
                int buttonID = GetJoystickButtonID(buttonName);
                int axisID = GetJoystickAxisID(buttonName);
                float xAxis = JoystickManager.GetJoystickAxis(buttonName, playerNumber, 'x');
                DebugJoystickButtonOnly(buttonPressed, buttonID);
                DebugJoystickAxis(xAxis, 'x', axisID);
            }
            else
            {

            }
        }
        else
        {
            int buttonID = GetJoystickButtonID(buttonName);
            bool buttonPressed = JoystickManager.GetJoystickButton(buttonName, playerNumber);
            DebugJoystickButtonOnly(buttonPressed, buttonID);
        }
    }
    private void DebugJoystickButtonOnly(bool buttonPressed, int buttonID)
    {
        if (buttonPressed)
            ChangeButtonRenderState(buttonID, true);
        else
            ChangeButtonRenderState(buttonID, false);
    }
    private void DebugJoystickAnalogs(bool buttonPressed, int buttonID, float xAxis, float yAxis)
    {
        HandleAnalogsMovement(xAxis, yAxis, buttonID);

        if (xAxis != 0 || yAxis != 0 || buttonPressed)
            ChangeButtonRenderState(buttonID, true);
        else
            ChangeButtonRenderState(buttonID, false);
    }
    private void HandleAnalogsMovement(float xAxis, float yAxis, int buttonID)
    {
        Vector2 joystickDir = new Vector2(xAxis, yAxis);

        Debug.DrawRay(m_buttons[buttonID].transform.position, joystickDir);
        if (buttonID == (int)JoystickButtonsID.L3)
            m_buttons[buttonID].transform.position = m_L3initialPos + (joystickDir / 2);
        else if (buttonID == (int)JoystickButtonsID.R3)
            m_buttons[buttonID].transform.position = m_R3initialPos + (joystickDir / 2);

    }
    private void DebugJoystickArrows(int axisValue, int buttonID)
    {
        //if (axisValue !=)
    }
    private void DebugJoystickAxis(float axisValue, char axis, int axisID)
    {
        if (axisValue != 0)
            ChangeAxisRenderValue(axisID, axisValue, axis);
        else
            ChangeAxisRenderValue(axisID, axisValue, axis);
    }
    private void ChangeAxisRenderValue(int axisID, float axisValue, char axis)
    {
        if (axis == 'x')
            m_axis[axisID].transform.localScale = new Vector2(axisValue, 1);
        else if (axis == 'y')
            m_axis[axisID].transform.localScale = new Vector2(1, axisValue);
    }
    private void ChangeButtonRenderState(int buttonID, bool value)
    {
        m_buttons[buttonID].GetComponent<SpriteRenderer>().enabled = value;
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
    private int GetJoystickAxisID(string buttonName)
    {
        switch (buttonName)
        {
            case JoystickButtons.L3:
                return 0;
            case JoystickButtons.R3:
                return 2;
            case JoystickButtons.L2:
                return 4;
            case JoystickButtons.R2:
                return 5;

        }
        return 1;
    }
    private bool GetJoystickButtonHasAxis(string buttonName)
    {
        if (   buttonName == JoystickButtons.L2 || buttonName == JoystickButtons.R2
            || buttonName == JoystickButtons.L3 || buttonName == JoystickButtons.R3
            || buttonName == JoystickButtons.Left || buttonName == JoystickButtons.Right
            || buttonName == JoystickButtons.Up || buttonName == JoystickButtons.Down)
            return true;
        else 
            return false;
    }
    private bool GetJoystickButtonIsAnalog(string buttonName)
    {
        if (buttonName == JoystickButtons.L3 || buttonName == JoystickButtons.R3)
            return true;
        else
            return false;
    }
    private bool GetJoystickButtonIsTrigger(string buttonName)
    {
        if (buttonName == JoystickButtons.L2 || buttonName == JoystickButtons.R2)
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
