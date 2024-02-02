using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator m_animator;
    public string m_currentAnimation;

    float m_yVelocity;
    Rigidbody2D m_rb;
    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        m_yVelocity = m_rb.velocity.y;
        m_animator.SetFloat("VelY", m_yVelocity);
    }


    public void ChangeAnimationState(string animation)
    {
        if (m_currentAnimation == animation) return;

        m_currentAnimation = animation;
        m_animator.Play(animation);
    }

    public void SetAnimationState(string animation)
    {
        m_currentAnimation = animation;
    }


}
