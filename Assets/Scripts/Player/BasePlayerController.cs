using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class BasePlayerController : MonoBehaviour, IDamagable //Clase base de los personajes del juego, algunas variables generales y metodos generales que comparten
{
    private protected PlayerAnimatorController playerAnimatorController; //Script donde se manejan las animaciones de los personajes
    private protected PlayerManager playerManager; //Script donde se encuentran los estados (Stats) del personaje

    private protected Rigidbody2D m_rb; //RigidBody2D del personaje
    private protected float m_playerSpeed = 6f; //Velocidad del jugador (puede cambiar dependiendo el personaje)
    private protected float m_xMovement; //Valor para el input horizontal
    private protected float m_yMovement; //Valor para guardar la velocidad vertical actual
    private protected bool m_movementPerformed; //Bool para controlar si el jugador se movio horizontalmente para que no se meta en bucle el cambio de animacion

    private protected BoxCollider2D m_groundCheckerBoxColl; //Box collider 2D para detectar si el jugador esta en el suelo
    private protected ContactFilter2D m_groundCheckerContactFilter; //Contact filter del GroundChecker, se establece su layermask a "Ground"
    private protected float m_jumpForce = 20f; //Fuerza de salto (puede cambiar dependiendo el personaje)

    private protected float m_rollForce = 10f; //Fuerza del roll (puede cambiar dependiendo el personaje)
    private protected float m_rollCooldown = 1f; //Tiempo del cooldown para relizar un roll (puede cambiar dependiendo el personaje)
    private protected bool m_canRoll = true;

    private protected float m_parryCooldown = 1f; //Tiempo del cooldown para relizar el parry (puede cambiar dependiendo el personaje)
    private protected bool m_canParry = true;

    private protected float m_hurtForce = 5f; //Fuerza con la que sera empujado el jugador al ser golpeado (puede cambiar dependiendo el personaje)

    private protected bool m_jumpPressed; //(Cross) Booleanos para saber que boton esta siendo apretado 
    private protected bool m_parryPressed1; //(L1)
    private protected bool m_parryPressed2; //(R1)
    private protected bool m_attack1Pressed; //(Square)
    private protected bool m_attack2Pressed; //(Circle)
    private protected bool m_attack3Pressed; //(Triangle)
    private protected bool m_rollPressed1; //(L2)
    private protected bool m_rollPressed2; //(R2)


    private protected virtual void Start()
    {
        playerAnimatorController = GetComponent<PlayerAnimatorController>(); //Obtenemos el componente PlayerAnimatorController
        playerManager = GetComponent<PlayerManager>(); //Obtenemos el componente PlayerManager
        m_rb = GetComponent<Rigidbody2D>(); //Obtenemos el componente RigidBody2D
        m_groundCheckerBoxColl = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>(); //Obtenemos el componente BoxCollider2D desde el primer objeto hijo del personaje
        SetGroundCheckerContactFilter(); //Establecemos la layer mask del ContactFilter del GroundChecker
    }

    private protected virtual void Update()
    {
        ManagePressedInputs(true);
    }

    private protected virtual void FixedUpdate()
    {
        PlayerJump();
        PlayerRoll();
        PlayerParry();
        PlayerHorizontalMovement();
        PlayerFlipSprite();

        DetectPlayerGrounded();
        
        ManagePressedInputs(false);
    }

    private void PlayerHorizontalMovement()
    {
        m_xMovement = Input.GetAxis("L3xP" + playerManager.PlayerNumber); //Valor del input horizontal (Left Stick del joystick)
        
        if (!playerManager.IsBusy)
        {
            if (m_xMovement != 0)
            {
                if (!m_movementPerformed)
                {
                    playerManager.SetMoving(true);
                    m_movementPerformed = true;
                    PlayerHorizontalMovementAnimation("run");
                }

                m_xMovement = Mathf.Sign(m_xMovement) * Mathf.CeilToInt(Mathf.Abs(m_xMovement));
                m_yMovement = m_rb.velocity.y;
                m_rb.velocity = new Vector2(m_xMovement * m_playerSpeed, m_yMovement);
            }
            else
            {
                if (m_movementPerformed)
                {
                    playerManager.SetMoving(false);
                    m_movementPerformed = false;
                    PlayerHorizontalMovementAnimation("idle");
                }

                m_yMovement = m_rb.velocity.y;
                m_rb.velocity = new Vector2(0, m_yMovement);
            }
        }
        
        
    }
    private void PlayerHorizontalMovementAnimation(string animation)
    {
        if (!playerManager.IsBusy && playerManager.IsGrounded && !playerManager.IsJumping)
            playerAnimatorController.ChangeAnimationState(animation);
    }
    private void PlayerFlipSprite()
    {
        if(!playerManager.IsFlipped && m_rb.velocity.x < 0 && !playerManager.IsBusy)
        {
            PlayerFlip("left");
        }
        else if (playerManager.IsFlipped && m_rb.velocity.x > 0 && !playerManager.IsBusy)
        {
            PlayerFlip("right");
        }
    }
    private void PlayerFlip(string side)
    {
        if (side == "left")
        {
            Debug.Log("Giro loco izquierda");
            playerManager.SetFlipped(true);
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y);
        }
        else if (side == "right")
        {
            Debug.Log("Giro loco derecha");
            playerManager.SetFlipped(false);
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
    private void PlayerJump()
    {
        if (m_jumpPressed && playerManager.IsGrounded && !playerManager.IsBusy)
        {
            playerManager.SetJumping(true);
            playerManager.SetLanding(false);
            m_xMovement = m_rb.velocity.x;
            m_rb.velocity = new Vector2(m_xMovement, m_jumpForce);
            PlayerJumpAnimation();
            m_groundCheckerBoxColl.enabled = false;
            Invoke("ActivateGroundChecker", 0.1f);
        }
    }
    private void PlayerJumpAnimation()
    {
        playerAnimatorController.ChangeAnimationState("jump_up");
    }
    private void SetGroundCheckerContactFilter()
    {
        m_groundCheckerContactFilter.SetLayerMask(LayerMask.GetMask("Ground")); 
    }
    private void ActivateGroundChecker()
    {
        m_groundCheckerBoxColl.enabled = true;
    }
    private void DetectPlayerGrounded()
    {
        if (Physics2D.OverlapCollider(m_groundCheckerBoxColl, m_groundCheckerContactFilter, new Collider2D[1]) > 0)
        {
            playerManager.SetGrounded(true);
            playerManager.SetJumping(false);
            if (!playerManager.IsBusy)
            {
                if (playerManager.IsMoving)
                {
                    playerAnimatorController.ChangeAnimationState("run");
                    Debug.Log("Bucle2");
                }
                else
                {
                    if (!playerManager.IsFinishingParry)
                    {
                        playerAnimatorController.ChangeAnimationState("idle");
                        Debug.Log("Bucle3");
                    }
                    
                }       
            }
        }
        else
        {
            playerManager.SetGrounded(false);
            if (!playerManager.IsBusy && !playerManager.IsGrounded && !playerManager.IsJumping)
            {
                Debug.Log("Bucle1");
                PlayerJumpAnimation();
            }   
        }
            
    }
    private void PlayerRoll()
    {
        if ((m_rollPressed1 || m_rollPressed2) && !playerManager.IsBusy && m_canRoll)
        {
            PlayerRollAnimation();
            playerManager.SetBusy(true);
            playerManager.SetRolling(true);

            m_canRoll = false;
            Invoke("PlayerRollCooldown", m_rollCooldown);

            m_rb.velocity = Vector2.zero;
            Vector2 rollDir = Vector2.zero;
            if (m_rollPressed1)
            {
                rollDir = new Vector2(-m_rollForce, 0f);
                PlayerFlip("left");
            }
            else if (m_rollPressed2)
            {
                rollDir = new Vector2(m_rollForce, 0f);
                PlayerFlip("right");
            }
            

            m_rb.AddForce(rollDir, ForceMode2D.Impulse);
        }
    }
    private void PlayerRollAnimation()
    {
        playerAnimatorController.ChangeAnimationState("roll");
    }
    private void PlayerRollCooldown()
    {
        m_canRoll = true;
    }
    private void PlayerParry()
    {
        if ((m_parryPressed1 || m_parryPressed2) && !playerManager.IsBusy && playerManager.IsGrounded && m_canParry)
        {
            PlayerParryAnimation();
            playerManager.SetBusy(true);
            playerManager.SetParrying(true);

            m_canParry = false;
            Invoke("PlayerParryCooldown", m_parryCooldown);

            m_rb.velocity = Vector2.zero;

            if (m_parryPressed1) 
                PlayerFlip("left");
            else if (m_parryPressed2)
                PlayerFlip("right");
        }
    }
    private void PlayerParryAnimation()
    {
        playerAnimatorController.ChangeAnimationState("parry");
        //playerAnimatorController.SetAnimationState("holisxddxd"); //Esto se puede solucionar mediante un cooldown en el parry
    }
    private void PlayerParryCooldown()
    {
        m_canParry = true;
    }
    public void ReceiveDamage(Transform enemyTransform, float damage)
    {
        float distance = transform.position.x - enemyTransform.position.x;
        sbyte i = 0;
        bool receiveDamage = false;
        if (distance >= 0)
        {
            i = 1;
            if (!playerManager.IsParrying) { receiveDamage = true; }
            else if (playerManager.IsParrying && !playerManager.IsFlipped) { receiveDamage = true; }
        }
        else if (distance < 0) 
        {
            i = -1;
            if (!playerManager.IsParrying) { receiveDamage = true; }
            else if (playerManager.IsParrying && playerManager.IsFlipped) { receiveDamage = true; }
        } 

        if (!playerManager.IsRolling && receiveDamage)
        {
            
            playerManager.SetHealth(playerManager.Health - damage);
            if (playerManager.Health <= 0)
            {
                m_rb.velocity = Vector2.zero;
                PlayerDeath();
            }
            else
            {
                PlayerHurtAnimation();
                playerManager.SetGettingHurt(true);
                playerManager.SetBusy(true);

                if (i == 1) PlayerFlip("left");
                else if (i == -1) PlayerFlip("right");

                m_rb.velocity = Vector2.zero;
                Vector2 moveDir = new Vector2(m_hurtForce * i, 0f);
                m_rb.AddForce(moveDir, ForceMode2D.Impulse);
            }
        }
    }
    private void PlayerHurtAnimation()
    {
        playerAnimatorController.ChangeAnimationState("hurt");
        playerAnimatorController.SetAnimationState("");
    }
    private void PlayerDeath()
    {
        playerManager.SetBusy(true);
        PlayerDeathAnimation();
    }
    private void PlayerDeathAnimation()
    {
        playerAnimatorController.ChangeAnimationState("death");
        this.enabled = false;
    }
    
    









    /// <summary>
    /// Cambia el estado del jugador.
    /// </summary>
    /// <param name="state">El estado del jugador. escribir en minuscula.</param>
    /// <param name="value">El valor que determina el nuevo estado.</param>
    public void ChangePlayerState(string state, bool value) //Este metodo es para usarse desde los StateMachines (se puede usar, pero por ahora solo para SM)
    {
        switch (state)
        {
            case "grounded":
                playerManager.SetGrounded(value);
                break;
            case "attacking":
                playerManager.SetAttacking(value);
                break;
            case "jumping":
                playerManager.SetJumping(value);
                break;
            case "parrying":
                playerManager.SetParrying(value);
                break;
            case "dying":
                playerManager.SetDying(value);
                break;
            case "moving":
                playerManager.SetMoving(value);
                break;
            case "rolling":
                playerManager.SetRolling(value);
                break;
            case "busy":
                playerManager.SetBusy(value);
                break;
            case "flipped":
                playerManager.SetFlipped(value);
                break;
            case "landing":
                playerManager.SetLanding(value);
                break;
            case "finishingparry":
                playerManager.SetFinishingParry(value);
                break;
            case "gettinghurt":
                playerManager.SetGettingHurt(value);
                break;

        }
    }

    private void ManagePressedInputs(bool entry)
    {
        if (entry)
        {
            if (Input.GetButton("CrossP" + playerManager.PlayerNumber)) m_jumpPressed = true;
            if (Input.GetButton("L1P" + playerManager.PlayerNumber)) m_parryPressed1 = true;
            if (Input.GetButton("R1P" + playerManager.PlayerNumber)) m_parryPressed2 = true;
            if (Input.GetButton("SquareP" + playerManager.PlayerNumber)) m_attack1Pressed = true;
            if (Input.GetButton("CircleP" + playerManager.PlayerNumber)) m_attack2Pressed = true;
            if (Input.GetButton("TriangleP" + playerManager.PlayerNumber)) m_attack3Pressed = true;
            if (Input.GetButton("L2P" + playerManager.PlayerNumber)) m_rollPressed1 = true;
            if (Input.GetButton("R2P" + playerManager.PlayerNumber)) m_rollPressed2 = true;


        }
        else
        {
            m_jumpPressed = false;
            m_parryPressed1 = false;
            m_parryPressed2 = false;
            m_attack1Pressed = false;
            m_attack2Pressed = false;
            m_attack3Pressed = false;
            m_rollPressed1 = false;
            m_rollPressed2 = false;
        }
    }

}
