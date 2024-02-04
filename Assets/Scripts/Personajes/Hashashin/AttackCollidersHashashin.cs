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
            case 1:
                m_damage = 5;
                m_hitForce = 2;
                break;
            case 2:
                m_damage = 10;
                m_hitForce = 3;
                break;
            case 3:
                m_damage = 15;
                m_hitForce = 5;
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
                collision.GetComponent<IDamagable>().ReceiveDamage(transform, m_damage, m_damage * 5f, m_hitForce);
            }  
            else
            {
                m_target = collision.transform;
                if (!m_ultimateCheck)
                {
                    m_ultimateCheck = true;
                    collision.GetComponent<IDamagable>().ReceiveDamage(transform, 0, 0, m_hitForce);
                }
                else
                {
                    collision.GetComponent<IDamagable>().ReceiveDamage(transform, m_damage, m_damage * 1.5f, m_hitForce);
                    ResetUltimateCheck();
                }
                    
            }
        }
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
