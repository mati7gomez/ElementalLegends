using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollidersHashashin : MonoBehaviour
{
    [SerializeField] private Byte m_atkNumber;
    private int m_damage;
    private int m_hitForce;
    private bool m_ultimateCheck;
    private Transform m_target;

    private void Start()
    {
        switch (m_atkNumber)
        {
            case 0:
                m_damage = 5;
                m_hitForce = 4;
                break;
            case 1:
                m_damage = 5;
                m_hitForce = 0;
                break;
            case 2:
                m_damage = 10;
                m_hitForce = 1;
                break;
            case 3:
                m_damage = 15;
                m_hitForce = 8;
                break;
            case 4:
                m_damage = 10;
                m_hitForce = 0;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (m_atkNumber != 4)
            {
                switch (m_atkNumber)
                {
                    case 0:
                        HandleAttack(collision, m_hitForce, 0);
                        break;
                    case 1:
                        HandleAttack(collision, m_hitForce, 0);
                        break;
                    case 2:
                        HandleAttack(collision, m_hitForce, 0);
                        break;
                    case 3:
                        HandleAttack(collision, m_hitForce, 10);
                        break;
                    case 4:
                        HandleAttack(collision, m_hitForce, 0);
                        break;
                }
            }  
            else
            {
                m_target = collision.transform;
                if (!m_ultimateCheck)
                {
                    m_ultimateCheck = true;
                    collision.GetComponent<IDamagable>().ReceiveDamage(transform, 0, 0, 0, 0);
                }
                else
                {
                    collision.GetComponent<IDamagable>().ReceiveDamage(transform, m_damage, m_damage + 5, m_hitForce, m_hitForce);
                    ResetUltimateCheck();
                }
                    
            }
        }
    }
    private void HandleAttack(Collider2D collision, float forceX, float forceY)
    {
        collision.GetComponent<IDamagable>().ReceiveDamage(transform, m_damage, m_damage * 5f, forceX, forceY);
    }
    private void ResetUltimateCheck()
    {
        m_ultimateCheck = false;
    }
    public bool GetCanUltimatePlayer()
    {
        return m_ultimateCheck;
    }
    public Transform GetTarget()
    {
        return m_target;
    }
    public void ResetTarget()
    {
        m_target = null;
    }
}
