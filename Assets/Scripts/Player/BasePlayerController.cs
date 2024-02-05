using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class BasePlayerController : MonoBehaviour, IDamagable //Clase base de los personajes del juego, algunas variables generales y metodos generales que comparten
{
    #region Variables
    private protected PlayerAnimatorController playerAnimatorController; //Script donde se manejan las animaciones de los personajes
    private protected PlayerManager playerManager; //Script donde se encuentran los estados (Stats) del personaje

    private protected Rigidbody2D m_rb; //RigidBody2D del personaje
    private protected float m_playerSpeed = 6f; //Velocidad del jugador (puede cambiar dependiendo el personaje)
    private protected float m_xMovement; //Valor para el input horizontal
    private protected float m_yMovement; //Valor para guardar la velocidad vertical actual
    private protected bool m_actionPerformed; //Bool para controlar si el jugador realizo alguna accion antes (parry, roll, atk)

    private protected BoxCollider2D m_groundCheckerBoxColl; //Box collider 2D para detectar si el jugador esta en el suelo
    private protected ContactFilter2D m_groundCheckerContactFilter; //Contact filter del GroundChecker, se establece su layermask a "Ground"
    private protected float m_jumpForce = 20f; //Fuerza de salto (puede cambiar dependiendo el personaje)

    private protected float m_rollForce = 10f; //Fuerza del roll (puede cambiar dependiendo el personaje)
    private protected float m_rollCooldown = 1f; //Tiempo del cooldown para relizar un roll (puede cambiar dependiendo el personaje)
    private protected bool m_canRoll = true; //Bool para saber si puede hacer un roll

    private protected float m_parryCooldown = 2f; //Tiempo del cooldown para relizar el parry (puede cambiar dependiendo el personaje)
    private protected bool m_canParry = true; //Bool para saber si puede hacer el parry

    private protected float m_hurtForce = 5f; //Fuerza con la que sera empujado el jugador al ser golpeado (puede cambiar dependiendo el personaje)

    private protected bool m_canAirAttack = true; //Variable para saber si puede atacar en el aire (uno por vez que ataque en el aire)

    private protected bool m_canAtk1 = true;
    private protected float m_atk1Cooldown = 1.5f;

    private protected bool m_canAtk2 = true;
    private protected float m_atk2Cooldown = 2f;

    private protected bool m_canAtk3 = true;
    private protected float m_atk3Cooldown = 2f;

    private protected bool m_canCombo;
    private protected Byte m_comboNumber;

    private protected bool m_jumpPressed; //(Cross) Booleanos para saber que boton esta siendo apretado 
    private protected bool m_parryPressed1; //(L1)
    private protected bool m_parryPressed2; //(R1)
    private protected bool m_attack1Pressed; //(Square)
    private protected bool m_attack2Pressed; //(Circle)
    private protected bool m_attack3Pressed; //(Triangle)
    private protected bool m_rollPressed1; //(L2)
    private protected bool m_rollPressed2; //(R2)

    #endregion

    private protected virtual void Start() //Start
    {
        playerAnimatorController = GetComponent<PlayerAnimatorController>(); //Obtenemos el componente PlayerAnimatorController
        playerManager = GetComponent<PlayerManager>(); //Obtenemos el componente PlayerManager
        m_rb = GetComponent<Rigidbody2D>(); //Obtenemos el componente RigidBody2D
        m_groundCheckerBoxColl = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>(); //Obtenemos el componente BoxCollider2D desde el primer objeto hijo del personaje
        SetGroundCheckerContactFilter(); //Establecemos la layer mask del ContactFilter del GroundChecker
    }

    private protected virtual void Update() //Update
    {
        ManagePressedInputs(true);
    }

    private protected virtual void FixedUpdate() //FixedUpdate
    {
        PlayerJump();
        PlayerRoll();
        PlayerParry();
        PlayerHorizontalMovement();
        PlayerFlipSprite();
        PlayerGroundChecker();
        
        ManagePressedInputs(false);
    }
    #region Horizontal movement
    private void PlayerHorizontalMovement() //Manejamos el movimiento horizontal del jugador
    {
        m_xMovement = Input.GetAxis("L3xP" + playerManager.PlayerNumber); //Valor del input horizontal (Left Stick del joystick)
        
        if (!playerManager.IsBusy && !playerManager.IsFinishingAttack) //Si el jugador no esta realizando ninguna accion
        {
            if (m_xMovement != 0) //Si el movimiento del input es distinto de 0, nos estamos moviendo
            {
                if (!playerManager.IsMoving) //Si no nos estabamos moviendo
                {
                    StartMovement();
                }
                PlayerMove(2); //Se ejecutan los calculos de movimiento y el movimiento
            }
            else //Si no estamos moviendo al jugador
            {
                if (playerManager.IsMoving) //Si nos estabamos moviendo (esto se ejecuta al quedarnos quietos luego de correr)
                {
                    StopMovement(); //Metodo para detener al jugador al quedarnos quietos
                }
                else if (!playerManager.IsMoving && m_actionPerformed && !playerManager.IsFinishingParry && !playerManager.IsFinishingAttack) //Si no nos estamos moviendo e hicimos alguna accion y no estamos terminando el parry o el ataque
                {
                    StopMovement(); //Metodo para detener al jugador al quedarnos quietos
                }
            }
        }
    }
    private void SetHorizontalMovementAnimation(string animation) //Metodo para cambiar la animacion del movimiento en el suelo
    {
        if (playerManager.IsGrounded && !playerManager.IsJumping)
            playerAnimatorController.ChangeAnimationState(animation);
    }
    private void CalculateDirectHorizontalMovement() //Calculo del movimiento del jugador sin suavizado
    {
        m_xMovement = Mathf.Sign(m_xMovement) * 1f; //Multiplicamos el signo del valor del input 1
    }
    private void CalculateSoftHorizontalMovement() //Calculo del movimiento del jugador con suavizado
    {
        m_xMovement = Mathf.Sign(m_xMovement) * Mathf.Clamp(Mathf.Abs(m_xMovement), 0.5f, 1f); //multiplicamos el signo del valor del input por Mathf.Clamp del valor absoluto del input, y establece 0.5 como valor minimo y 1 como maximo del float
    }
    private void HandleHorizontalMovementPhysics() //Fisicas del movimiento horizontal del jugador
    {
        m_yMovement = m_rb.velocity.y; //Velocidad actual de la velocidad en Y
        m_rb.velocity = new Vector2(m_xMovement * m_playerSpeed, m_yMovement); //Nueva velocidad del rigidbody. xInput * playerSpeed
    }
    private void PlayerMove(Byte movementMode) //Metodo para ejecutar los calculos de movimiento y el movimiento
    {
        if (movementMode == 1)
        {
            //Movimiento sin suavizado
            CalculateDirectHorizontalMovement(); //Calculamos el valor del input para movernos directamente
        }
        else if (movementMode == 2)
        {
            //Movimiento con suavizado
            CalculateSoftHorizontalMovement(); //Calculamos el valor del input para movernos con suavizado
        }
        HandleHorizontalMovementPhysics(); //Manejamos las fisicas del movimiento horizontal del jugador
    }
    private void StartMovement() //Metodo para comenzar a movernos
    {
        playerManager.SetMoving(true); //Establecemos el estado moviendose del jugador
        SetHorizontalMovementAnimation("run"); //Establecemos la animacion del personaje a "run"
    }
    private void StopMovement() //Metodo para detener al jugador al quedarnos quietos
    {
        PlayerSetActionPerformed(false); //Establecemos que no se realizo ninguna accion anteriormente
        playerManager.SetMoving(false); //Establecemos el estado de movimiento del jugador
        SetHorizontalMovementAnimation("idle"); //Establecemos la animacion del personaje a "idle"
        HandleHorizontalMovementPhysics(); //Manejamos las fisicas del movimiento horizontal del jugador
    }
    #endregion

    #region Flip
    private void PlayerFlipSprite() //Rotamos el sprite del jugador al cambiar de direccion
    {
        if(!playerManager.IsFlipped && m_rb.velocity.x < 0 && !playerManager.IsBusy)
        {
            FlipSprite("left");
        }
        else if (playerManager.IsFlipped && m_rb.velocity.x > 0 && !playerManager.IsBusy)
        {
            FlipSprite("right");
        }
    }
    private void FlipSprite(string side) //Metodo para rotar el sprite
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
    #endregion

    #region Jump
    private void PlayerJump()
    {
        if (m_jumpPressed && playerManager.IsGrounded && !playerManager.IsBusy)
        {
            SetJumpAnimation(); //Establecemos la animacion de salto y los estados
            m_xMovement = m_rb.velocity.x;
            m_rb.velocity = new Vector2(m_xMovement, m_jumpForce);
            
            //m_groundCheckerBoxColl.enabled = false; //Desactivamos el groundChecker momentaneamente para q no se nos buguee
            //Invoke("ActivateGroundChecker", 0.1f); //Activamos el ground checker nuevamente
        }
    }
    private void SetJumpAnimation() //Establecemos la animacion de salto y los estados
    {
        playerAnimatorController.ChangeAnimationState("jump_up");
        playerManager.SetJumping(true);
        playerManager.SetLanding(false);
        playerManager.SetGrounded(false);
    }
    #endregion

    #region GroundChecker
    private void SetGroundCheckerContactFilter()
    {
        m_groundCheckerContactFilter.SetLayerMask(LayerMask.GetMask("Ground")); 
    }
    private void ActivateGroundChecker()
    {
        m_groundCheckerBoxColl.enabled = true;
    }
    private void PlayerGroundChecker() //metodo para controlar el estado del jugador sobre el suelo
    {
        if (Physics2D.OverlapCollider(m_groundCheckerBoxColl, m_groundCheckerContactFilter, new Collider2D[1]) > 0) //Si hay mas de un objeto con layerMask "Ground" colisionando con el groundCheckerBoxCollider es porque estamos en el suelo
        {
            if (!playerManager.IsBusy) //Si no estamos realizando ninguna accion (ataque, parry, roll. No incluye movimiento horizontal o salto)
            {
                if (playerManager.IsMoving && !playerManager.IsGrounded) //Si nos estamos moviendo y caemos en el suelo (cambio de animacion a correr al caer)
                {
                    PlayerOnLand("run"); //Metodo para cuando aterricemos en el suelo
                }
                else if (!playerManager.IsMoving && !playerManager.IsGrounded) //Si no nos estamos moviendo y caemos al suelo (cambio de animacion a idle al caer)
                {
                    PlayerOnLand("idle"); //Metodo para cuando aterricemos en el suelo
                }     
            }
        }
        else //Si el groundChecker no esta en el suelo
        {
            if (playerManager.IsGrounded) //Si estamos en el suelo
                playerManager.SetGrounded(false); //cambiamos la variable ya que dejamos de estar en el suelo
            else //Si no estamos en el suelo
            {
                if (!playerManager.IsBusy && !playerManager.IsJumping) //Si el jugador no esta realizando ninguna accion y sale del suelo y no esta saltando (cambio de animacion al caer del suelo)
                {
                    SetJumpAnimation();
                    playerManager.SetJumping(true);
                }
            }
        }  
    }
    private void PlayerOnLand(string animation) //Metodo para cuando aterricemos en el suelo
    {
        playerManager.SetJumping(false); //Establecemos la variable isJumping en falsa, ya no estamos saltando
        playerManager.SetGrounded(true); //Establecemos la variable isGrounded en verdadera, estamos en el suelo
        m_canAirAttack = true;
        SetHorizontalMovementAnimation(animation); //Metodo para cambiar la animacion del movimiento en el suelo
        
    }
    #endregion

    #region Roll
    private void PlayerRoll()
    {
        if ((m_rollPressed1 || m_rollPressed2) && !playerManager.IsBusy && m_canRoll)
        {
            PlayerRollAnimation(); //Cambiamos la animacion de roll y sus estados

            m_canRoll = false;
            Invoke("PlayerRollCooldown", m_rollCooldown);

            m_rb.velocity = Vector2.zero;
            Vector2 rollDir = Vector2.zero;
            if (m_rollPressed1)
            {
                rollDir = new Vector2(-m_rollForce, 0f);
                FlipSprite("left");
            }
            else if (m_rollPressed2)
            {
                rollDir = new Vector2(m_rollForce, 0f);
                FlipSprite("right");
            }
            
            m_rb.AddForce(rollDir, ForceMode2D.Impulse);
        }
    }
    private void PlayerRollAnimation() //Cambiamos la animacion de roll y sus estados
    {
        playerAnimatorController.ChangeAnimationState("roll");
        playerManager.SetBusy(true); 
        playerManager.SetRolling(true);
        PlayerSetActionPerformed(true);
    }
    private void PlayerRollCooldown()
    {
        m_canRoll = true;
    }
    #endregion

    #region Parry
    private void PlayerParry()
    {
        if ((m_parryPressed1 || m_parryPressed2) && !playerManager.IsBusy && playerManager.IsGrounded && m_canParry)
        {
            Debug.Log("parry");
            Debug.Log(playerAnimatorController.m_currentAnimation);
            PlayerParryAnimation(); //Cambiamos a la animacion del parry y sus estados
            
            m_canParry = false;
            Invoke("PlayerParryCooldown", m_parryCooldown);

            m_rb.velocity = Vector2.zero;
            if (m_parryPressed1) 
                FlipSprite("left");
            else if (m_parryPressed2)
                FlipSprite("right");
        }
    }
    private void PlayerParryAnimation() //Cambiamos a la animacion del parry y sus estados
    {
        playerAnimatorController.ChangeAnimationState("parry");
        playerManager.SetBusy(true);
        playerManager.SetParrying(true);
        PlayerSetActionPerformed(true);
    }
    private void PlayerParryCooldown()
    {
        m_canParry = true;
    }

    #endregion

    #region Damage and death
    public void ReceiveDamage(Transform enemyTransform, float damage, float energy, float forceX, float forceY)
    {   
        PlayerSetActionPerformed(true);
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
            AddEnergy(energy);
            playerManager.SetHealth(playerManager.Health - damage);
            if (playerManager.Health <= 0)
            {
                m_rb.velocity = Vector2.zero;
                PlayerDeath();
            }
            else
            {
                PlayerHurtAnimation(); //Establecemos la animacion de daño y los estados

                if (i == 1) FlipSprite("left");
                else if (i == -1) FlipSprite("right");

                m_rb.velocity = Vector2.zero;
                Vector2 moveDir = new Vector2(forceX * i, forceY);
                m_rb.AddForce(moveDir, ForceMode2D.Impulse);
            }
        }
    }
    private void PlayerHurtAnimation() //Establecemos la animacion de daño y los estados
    {
        playerAnimatorController.ChangeAnimationState("hurt");
        playerAnimatorController.SetAnimationState("");
        playerManager.SetGettingHurt(true);
        playerManager.SetBusy(true);
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

    #endregion

    private protected void PlayerSetActionPerformed(bool value)
    {
        playerManager.SetMoving(false); //Establecemos q nos dejamos de mover
        m_actionPerformed = value; //Establecemos el booleano de accion realizada, esto para no estar seteando todo el tiempo el estado o la animacion
    }
    private protected void BaseFixedUpdate() //Ejecutar al final del fixed update de cada personaje
    {
        PlayerJump();
        PlayerRoll();
        PlayerParry();
        PlayerHorizontalMovement();
        PlayerFlipSprite();
        PlayerGroundChecker();

        ManagePressedInputs(false);
    }
    public void SetRigidBodyDynamic()
    {
        m_rb.isKinematic = false;
    }
    public void DisableCombo()
    {
        if (!playerManager.IsAttacking)
        {
            m_canCombo = false;
            m_comboNumber = 0;
        }
        
    }
    public void ResetEnergy() 
    {
        playerManager.SetEnergy(0);
    }
    public void AddEnergy(float energyToAdd)
    {
        playerManager.SetEnergy(playerManager.Energy + energyToAdd);
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
            case "finishingattack":
                playerManager.SetFinishingAttack(value);
                break;

        }
    }

    private void ManagePressedInputs(bool entry)
    {
        if (entry)
        {
            if (Input.GetButtonDown("CrossP" + playerManager.PlayerNumber)) m_jumpPressed = true;
            if (Input.GetButtonDown("L1P" + playerManager.PlayerNumber)) m_parryPressed1 = true;
            if (Input.GetButtonDown("R1P" + playerManager.PlayerNumber)) m_parryPressed2 = true;
            if (Input.GetButtonDown("SquareP" + playerManager.PlayerNumber)) m_attack1Pressed = true;
            if (Input.GetButtonDown("CircleP" + playerManager.PlayerNumber)) m_attack2Pressed = true;
            if (Input.GetButtonDown("TriangleP" + playerManager.PlayerNumber)) m_attack3Pressed = true;
            if (Input.GetButtonDown("L2P" + playerManager.PlayerNumber)) m_rollPressed1 = true;
            if (Input.GetButtonDown("R2P" + playerManager.PlayerNumber)) m_rollPressed2 = true;


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
