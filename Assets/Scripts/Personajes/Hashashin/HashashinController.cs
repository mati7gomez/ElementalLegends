using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HashashinController : BasePlayerController
{
    private protected override void Start()
    {
        base.Start();
    }
    private protected override void Update()
    {
        base.Update();
    }
    private protected override void FixedUpdate()
    {
        PlayerAtk();
        BaseFixedUpdate();
    }
    private void PlayerAtk()
    {
        AirAttack();
        Atk1();
        Atk2();
        Atk3();
        UltimateAtk();
    }
    private void AirAttack()
    {
        if (!playerManager.IsBusy && m_attack1Pressed && playerManager.IsJumping && !playerManager.IsGrounded && m_canAirAttack)
        {
            AirAttackAnimationAndState();
            m_rb.velocity = Vector2.zero;
            m_rb.isKinematic = true;
        }
    }
    private void AirAttackAnimationAndState()
    {
        playerAnimatorController.ChangeAnimationState("air_atk");
        playerManager.SetAttacking(true);
        playerManager.SetBusy(true);
        playerManager.SetJumping(false);
        m_canAirAttack = false;
    }

    private void AtkAnimationAndState(string attack)
    {
        playerAnimatorController.ChangeAnimationState(attack);
        playerManager.SetAttacking(true);
        playerManager.SetBusy(true);
        PlayerSetActionPerformed(true);

    }
    private void Atk1()
    {
        if (m_attack1Pressed && !playerManager.IsBusy && playerManager.IsGrounded && m_canAtk1)
        {
            AtkAnimationAndState("atk1");
            m_rb.velocity = Vector2.zero;
            m_rb.isKinematic = true;
            m_canCombo = true;
            m_comboNumber = 1;
            m_canAtk1 = false;
            Invoke("Atk1Cooldown", m_atk1Cooldown);
        }
    }
    private void Atk1Cooldown()
    {
        m_canAtk1 = true;
    }
    private void Atk2()
    {
        if (m_attack2Pressed && !playerManager.IsBusy && playerManager.IsGrounded && m_canAtk2 && m_canCombo && m_comboNumber == 1)
        {
            AtkAnimationAndState("atk2");
            m_rb.velocity = Vector2.zero;
            m_rb.isKinematic = true;
            m_canCombo = true;
            m_comboNumber = 2;
            m_canAtk2 = false;
            Invoke("Atk2Cooldown", m_atk2Cooldown);
        }
    }
    private void Atk2Cooldown()
    {
        m_canAtk2 = true;
    }
    private void Atk3()
    {
        if (m_attack3Pressed && !playerManager.IsBusy && playerManager.IsGrounded && m_canAtk3 && m_canCombo && m_comboNumber == 2)
        {
            AtkAnimationAndState("atk3");
            m_rb.velocity = Vector2.zero;
            m_rb.isKinematic = true;
            m_canCombo = true;
            m_comboNumber = 3;
            m_canAtk3 = false;
            Invoke("Atk3Cooldown", m_atk3Cooldown);
        }
    }
    private void Atk3Cooldown()
    {
        m_canAtk3 = true;
    }
    private void UltimateAtk()
    {
        if (m_attack3Pressed && !playerManager.IsBusy && playerManager.Energy >= 120f)
        {
            AtkAnimationAndState("ultimate");
            playerManager.SetJumping(false);
            m_rb.velocity = Vector2.zero;
            m_rb.isKinematic = true;
            DisableCombo();
            ResetEnergy();
        }
    }
    private void UltimateTeleport()
    {
        AttackCollidersHashashin ultimate = transform.Find("Ultimate").GetComponent<AttackCollidersHashashin>();
        if (ultimate.GetCanUltimatePlayer())
        {
            Transform enemyPos = ultimate.GetTarget();
            Debug.Log("Me muevo al enemigo nache");
            transform.position = enemyPos.position;
            ultimate.ResetTarget();
        }
    }

}
