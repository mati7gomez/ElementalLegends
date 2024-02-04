using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerStatsInfoUI : MonoBehaviour
{
    private PlayerManager playerManagerP1;

    [SerializeField] private Color m_colorRed;
    [SerializeField] private Color m_colorGreen;

    private TextMeshProUGUI[] m_texts;

    private void Start()
    {
        playerManagerP1 = GameObject.Find("Hashashin").GetComponent<PlayerManager>();
        SetTextsObjects();
        SetP1EventsSuscription();
    }
    private void SetTextsObjects()
    {
        m_texts = new TextMeshProUGUI[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_texts[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }
    private void SetP1EventsSuscription()
    {
        // Suscribir los métodos de P1 a los eventos correspondientes
        playerManagerP1.GroundedState += UpdateGroundedState;
        playerManagerP1.AttackState += UpdateAttackState;
        playerManagerP1.JumpState += UpdateJumpState;
        playerManagerP1.ParryState += UpdateParryState;
        playerManagerP1.DieState += UpdateDieState;
        playerManagerP1.MoveState += UpdateMoveState;
        playerManagerP1.RollState += UpdateRollState;
        playerManagerP1.BusyState += UpdateBusyState;
        playerManagerP1.FlipState += UpdateFlipState;
        playerManagerP1.LandState += UpdateLandState;
        playerManagerP1.FinishParryState += UpdateFinishParryState;
        playerManagerP1.GetHurtState += UpdateGetHurtState;
        playerManagerP1.HealthState += UpdateHealthState;
        playerManagerP1.FinishAttackState += UpdateFinishAttackState;
        playerManagerP1.EnergyState += UpdateEnergyState;
    }



    // Métodos de manejo de eventos generados para cada suscripción
    private void UpdateStatesColorUI(int i, bool value)
    {
        if (value)
            m_texts[i].color = m_colorGreen;
        else
            m_texts[i].color = m_colorRed;
    }
    private void UpdateHealthUI(float health)
    {
        m_texts[13].text = "Health: " + health.ToString();
    }
    private void UpdateEnergyUI(float energy)
    {
        m_texts[15].text = "Energy: " + energy.ToString();
    }

    //--------------------------------------------------------//

    private void UpdateGroundedState(bool value)
    {
        UpdateStatesColorUI(1, value);
    }
    private void UpdateAttackState(bool value)
    {
        UpdateStatesColorUI(2, value);
    }
    private void UpdateJumpState(bool value)
    {
        UpdateStatesColorUI(3, value);
    }
    private void UpdateParryState(bool value)
    {
        UpdateStatesColorUI(4, value);
    }
    private void UpdateDieState(bool value)
    {
        UpdateStatesColorUI(5, value);
    }
    private void UpdateMoveState(bool value)
    {
        UpdateStatesColorUI(6, value);
    }
    private void UpdateRollState(bool value)
    {
        UpdateStatesColorUI(7, value);
    }
    private void UpdateBusyState(bool value)
    {
        UpdateStatesColorUI(8, value);
    }
    private void UpdateFlipState(bool value)
    {
        UpdateStatesColorUI(9, value);
    }
    private void UpdateLandState(bool value)
    {
        UpdateStatesColorUI(10, value);
    }
    private void UpdateFinishParryState(bool value)
    {
        UpdateStatesColorUI(11, value);
    }
    private void UpdateGetHurtState(bool value)
    {
        UpdateStatesColorUI(12, value);
    }
    private void UpdateHealthState(float health)
    {
        UpdateHealthUI(health);
    }
    private void UpdateFinishAttackState(bool value)
    {
        UpdateStatesColorUI(14, value);
    }
    private void UpdateEnergyState(float energy) // 15
    {
        UpdateEnergyUI(energy);
    }

}
