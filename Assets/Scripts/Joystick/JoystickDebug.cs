using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public struct JoystickButtons
{
    public static string Square = "Square";
    public static string Cross = "Cross";
    public static string Circle = "Circle";
    public static string Triangle = "Triangle";
    public static string L1 = "L1";
    public static string R1 = "R1";
    public static string L2 = "L2";
    public static string R2 = "R2";
    public static string Share = "Share";
    public static string Options = "Options";
    public static string L3 = "L3";
    public static string R3 = "R3";
    public static string PS = "PS";
    public static string Pad = "Pad";
}


public class JoystickDebug : MonoBehaviour
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

    private Vector2 m_L3initialPos; //Posicion inicial del boton L3 del joystick
    private Vector2 m_R3initialPos; //Posicion inicial del boton R3 del joystick

    //-----------------//


    void Start()
    {
        DebugJoysticksInGame();
        SetAnalogsInitialPosition();
    }


    //-----------------//


    private void Update()
    {
        GetJoystickSquareButtonInfo();
        GetJoystickCrossButtonInfo();
        GetJoystickCircleButtonInfo();
        GetJoystickTriangleButtonInfo();
        GetJoystickL1ButtonInfo();
        GetJoystickR1ButtonInfo();
        GetJoystickL2ButtonInfo();
        GetJoystickR2ButtonInfo();
        GetJoystickShareButtonInfo();
        GetJoystickOptionsButtonInfo();
        GetJoystickL3ButtonInfo();
        GetJoystickR3ButtonInfo();
        GetJoystickPSButtonInfo();
        GetJoystickPadButtonInfo();




    }

    //-----------------//

    private void DebugJoysticksInGame()
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

    private void GetJoystickSquareButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.Square + "P" + m_playerNumber))
            m_buttons[0].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[0].GetComponent<SpriteRenderer>().enabled = false;
    }

    private void GetJoystickCrossButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.Cross + "P" + m_playerNumber))
            m_buttons[1].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[1].GetComponent<SpriteRenderer>().enabled = false;
    }

    private void GetJoystickCircleButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.Circle + "P" + m_playerNumber))
            m_buttons[2].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[2].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickTriangleButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.Triangle + "P" + m_playerNumber))
            m_buttons[3].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[3].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickL1ButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.L1 + "P" + m_playerNumber))
            m_buttons[4].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[4].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickR1ButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.R1 + "P" + m_playerNumber))
            m_buttons[5].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[5].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickL2ButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.L2 + "P" + m_playerNumber))
            m_buttons[6].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[6].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickR2ButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.R2 + "P" + m_playerNumber))
            m_buttons[7].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[7].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickShareButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.Share + "P" + m_playerNumber))
            m_buttons[8].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[8].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickOptionsButtonInfo()
    {
        if (Input.GetButton(JoystickButtons.Options + "P" + m_playerNumber))
            m_buttons[9].GetComponent<SpriteRenderer>().enabled = true;
        else
            m_buttons[9].GetComponent<SpriteRenderer>().enabled = false;
    }
    private void GetJoystickL3ButtonInfo()
    {
        bool pressed;
        float xInput;
        float yInput;

        xInput = Input.GetAxis(JoystickButtons.L3 + "xP" + m_playerNumber);
        yInput = Input.GetAxis(JoystickButtons.L3 + "yP" + m_playerNumber);
        pressed = Input.GetButton(JoystickButtons.L3 + "P" + m_playerNumber);

        DebugJoystickL3Movement(pressed, xInput, yInput);
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
