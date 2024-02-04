using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Byte m_playerNumber;
    public Byte PlayerNumber => m_playerNumber;

    private bool isGrounded;
    private bool isAttacking;
    private bool isJumping;
    private bool isParrying;
    private bool isDying;
    private bool isMoving;
    private bool isRolling;
    private bool isBusy;
    private bool isFlipped;
    private bool isLanding;
    private bool isFinishingParry;
    private bool isGettingHurt;
    private float health = 100f;
    private bool isFinishingAttack;
    private float energy;

    public bool IsGrounded => isGrounded;
    public bool IsAttacking => isAttacking;
    public bool IsJumping => isJumping;
    public bool IsParrying => isParrying;
    public bool IsDying => isDying;
    public bool IsMoving => isMoving;
    public bool IsRolling => isRolling;
    public bool IsBusy => isBusy;
    public bool IsFlipped => isFlipped;
    public bool IsLanding => isLanding;
    public bool IsFinishingParry => isFinishingParry;
    public bool IsGettingHurt => isGettingHurt;
    public float Health => health;
    public bool IsFinishingAttack => isFinishingAttack;
    public float Energy => energy;


    //public void SetGrounded(bool value) => isGrounded = value;
    //public void SetAttacking(bool value) => isAttacking = value;
    //public void SetJumping(bool value) => isJumping = value;
    //public void SetParrying(bool value) => isParrying = value;
    //public void SetDying(bool value) => isDying = value;
    //public void SetMoving(bool value) => isMoving = value;
    //public void SetRolling(bool value) => isRolling = value;
    //public void SetBusy(bool value) => isBusy = value;
    //public void SetFlipped(bool value) => isFlipped = value;
    //public void SetLanding(bool value) => isLanding = value;
    //public void SetFinishingParry(bool value) => isFinishingParry = value;
    public void SetGrounded(bool value)
    {
        isGrounded = value;
        GroundedState?.Invoke(value);
    }
    public void SetAttacking(bool value)
    {
        isAttacking = value;
        AttackState?.Invoke(value);
    }
    public void SetJumping(bool value)
    {
        isJumping = value;
        JumpState?.Invoke(value);
    }
    public void SetParrying(bool value)
    {
        isParrying = value;
        ParryState?.Invoke(value);
    }
    public void SetDying(bool value)
    {
        isDying = value;
        DieState?.Invoke(value);
    }
    public void SetMoving(bool value)
    {
        isMoving = value;
        MoveState?.Invoke(value);
    }
    public void SetRolling(bool value)
    {
        isRolling = value;
        RollState?.Invoke(value);
    }
    public void SetBusy(bool value)
    {
        isBusy = value;
        BusyState?.Invoke(value);
    }
    public void SetFlipped(bool value)
    {
        isFlipped = value;
        FlipState?.Invoke(value);
    }
    public void SetLanding(bool value)
    {
        isLanding = value;
        LandState?.Invoke(value);
    }
    public void SetFinishingParry(bool value)
    {
        isFinishingParry = value;
        FinishParryState?.Invoke(value);
    }
    public void SetGettingHurt(bool value)
    {
        isGettingHurt = value;
        GetHurtState?.Invoke(value);
    }
    public void SetHealth(float newHealth)
    {
        health = newHealth;
        HealthState?.Invoke(health);
    } 
    public void SetFinishingAttack(bool value)
    {
        isFinishingAttack = value;
        FinishAttackState?.Invoke(isFinishingAttack);
    }
    public void SetEnergy(float value)
    {
        energy = value;
        EnergyState?.Invoke(energy);
    }

    public event Action<bool> GroundedState;
    public event Action<bool> AttackState;
    public event Action<bool> JumpState;
    public event Action<bool> ParryState;
    public event Action<bool> DieState;
    public event Action<bool> MoveState;
    public event Action<bool> RollState;
    public event Action<bool> BusyState;
    public event Action<bool> FlipState;
    public event Action<bool> LandState;
    public event Action<bool> FinishParryState;
    public event Action<bool> GetHurtState;
    public event Action<float> HealthState;
    public event Action<bool> FinishAttackState;
    public event Action<float> EnergyState;

}
