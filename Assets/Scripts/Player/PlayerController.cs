using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour, IKnockbackable, IDamageable, IDefaultKnockback
{
    #region Variables
    private float movementInputDirection;

    public TextMeshProUGUI healthText;

    private int amountOfJumpsLeft;
    private int amountOfBombsLeft;

    private float bombsRefreshTimeLeft;
    private float holdDownStartTime;
    private float holdDownTime = 0f;
    private float walkTime = 0f;
    private float walkTimeStarted;
    private float walkEffectCd = 0.5f;
    private float knockbackStartTime;

    private bool holdingBomb = false;
    private bool isFacingRight = true;
    private bool isWalking = false;
    private bool isGrounded = false;
    private bool isJumping = false; 
    private bool canJump = false;
    private bool isKnockbackActive = false;
    private bool canMove = true;
    private bool isHit = false;
    private bool isAlive = true;

    private Vector2 rbVelocity;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps;
    public int amountOfBombs;

    public float healthAmount = 5f;

    public float bombsRefreshTime = 2f;
    public float minHoldDownTime = 0.25f;
    public float maxHoldDownTime = 2.5f;
    public float maxBombForce = 2f;

    public float movementSpeed = 5.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float variableJumpHeightMultiplier = 0.5f;

    public float knockbackDuration = 0.75f;
    public float xKnockbackMultiplier = 1f;
    public float yKnockbackMultiplier = 1f;

    public Transform groundCheck;
    public Transform bombPlacement;
    public Transform jumpEffectPosition;
    public Transform fallEffectPosition;
    public Transform runEffectPosition;
    public HoldBarScript holdBarObject;

    public GameObject bombPrefab;
    public GameObject jumpEffectPrefab;
    public GameObject fallEffectPrefab;
    public GameObject runEffectPrefab;

    public LayerMask whatIsGround;
    public Vector2 defaultKnockbackAngle = Vector2.zero;

    public EventManager eventManager;
    public Core Core { get; private set; }
    #endregion

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
    }

    void Start()
    {
        bombsRefreshTimeLeft = bombsRefreshTime;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        RefreshBombsAmount();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
        CheckKnockback();
    }

    private void CheckSurroundings()
    {
        if(rb.velocity.y <= 0.01f)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
            if (isGrounded)
            {
                isHit = false;
            }
            else
            {
                isJumping = true;
            }
        }
    }
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
            if (isJumping)
            {
                Instantiate(fallEffectPrefab, fallEffectPosition.position, Quaternion.identity);
            }
            isJumping = false;
        }


        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void RefreshBombsAmount()
    {
        bombsRefreshTimeLeft -= Time.deltaTime;

        if(bombsRefreshTimeLeft < 0 && amountOfBombsLeft < amountOfBombs)
        {
            bombsRefreshTimeLeft = bombsRefreshTime;
            amountOfBombsLeft++;
        }
    }
    private void CheckInput()
    {
        if (isAlive && canMove)
        {
            movementInputDirection = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetButtonUp("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }

            if (Input.GetButtonDown("Bomb"))
            {
                if (amountOfBombsLeft > 0) { 
                    holdDownStartTime = Time.time;
                    holdingBomb = true;
                }
            }

            if (Input.GetButtonUp("Bomb"))
            {
                if(holdingBomb){
                    PlaceBomb(holdDownTime);
                    holdingBomb = false;
                }
            }
        }
        
    }
    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            if (!isWalking)
            {
                Instantiate(runEffectPrefab, runEffectPosition.position, transform.rotation);
                walkTimeStarted = Time.time;
            }

            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void ApplyMovement()
    {
        if (!isAlive && isGrounded && !isKnockbackActive)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (canMove && isAlive)
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }

    private void PlaceBomb(float holdDownTime)
    {
        if(amountOfBombsLeft > 0)
        {
            BombController bombPlaced = Instantiate(bombPrefab, bombPlacement.position, Quaternion.identity).GetComponent<BombController>();
            amountOfBombsLeft--;

            if (holdDownTime >= minHoldDownTime)
            {
                holdBarObject.BombReleased();
                bombPlaced.LaunchBomb(CalculateHoldDownForce(holdDownTime), isFacingRight ? 1 : -1);
            }

        }
    }

    private float CalculateHoldDownForce(float holdTime)
    {
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxHoldDownTime);
        Debug.Log("CalculateHoldDownForce -> " + holdTimeNormalized);
        return holdTimeNormalized * maxBombForce;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180f, 0f);
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            isJumping = true;
            amountOfJumpsLeft--;
            Instantiate(jumpEffectPrefab, jumpEffectPosition.position, Quaternion.identity);
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("walking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isKnockback", isKnockbackActive);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isAlive", isAlive);
        anim.SetBool("canMove", canMove);
        anim.SetFloat("yVelocity", rb.velocity.y);
        HandleBombBarAnimations();
        HandleRunningEffect();
    }
    private void HandleBombBarAnimations()
    {
        if (holdingBomb)
        {
            holdDownTime = Time.time - holdDownStartTime;
            if (holdDownTime >= minHoldDownTime)
            {
                holdBarObject.EnableEffect();
            }
        }
    }
    private void HandleRunningEffect()
    {
        if (isWalking && isGrounded && isAlive)
        {
            walkTime = Time.time - walkTimeStarted;
            if (walkTime >= walkEffectCd)
            {
                Instantiate(runEffectPrefab, runEffectPosition.position, transform.rotation);
                walkTimeStarted = Time.time;

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    public void Knockback(float knockbackStrength, Vector2 knockbackOriginPosition)
    {
        if (!isKnockbackActive)
        {

            Vector2 position = transform.position;
            Vector2 angle = new Vector2(position.x - knockbackOriginPosition.x, position.y - knockbackOriginPosition.y).normalized;

            if (!rb.bodyType.Equals(2))
            {

                Debug.Log("transform.position.x > bombPosition.x: " + (position.x > knockbackOriginPosition.x) + " _ transform.position.x - " + position.x + " _bombPosition.x - " + knockbackOriginPosition.x);
                if (TargetAndBombHaveSameHeight(position, knockbackOriginPosition))
                {
                    rbVelocity = new Vector2(angle.x * knockbackStrength * xKnockbackMultiplier,
                        angle.y * knockbackStrength * yKnockbackMultiplier);
                }
                else
                {
                    rbVelocity = new Vector2(angle.x * knockbackStrength * xKnockbackMultiplier,
                        angle.y * knockbackStrength);
                }
                rb.isKinematic = true;
                ApplyKnockback();
                rb.isKinematic = false;
            }
            holdBarObject.BombReleased();
        }
    }
    public bool IsAlive()
    {
        return isAlive;
    }
    public void ApplyKnockback()
    {
        Debug.Log("KNOCKBACK Angle : " + rbVelocity);
        rb.velocity = Vector2.zero;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
        rb.velocity = rbVelocity;
        canMove = false;
    }
    public void Damage(float amount)
    {
        isHit = true;
        healthAmount -= amount;
        healthText.SetText("HEALTH: " + healthAmount);
        Debug.Log("HEALTH: " + healthAmount);
        if (healthAmount <= 0f)
        {
            eventManager.BroadCastPlayerDeath();
            isAlive = false;
            canMove = false;
        }
        holdBarObject.BombReleased();
    }
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && isKnockbackActive && isGrounded)
        {
            isKnockbackActive = false;
            canMove = true;
        }
    }

    private bool TargetAndBombHaveSameHeight(Vector2 targetPosition, Vector2 originPosition)
    {
        return originPosition.y + 1 > targetPosition.y;
    }

    public void Knockback(int facingDirection)
    {
        if (!isKnockbackActive)
        {

            if (!rb.bodyType.Equals(2))
            {

                rbVelocity = new Vector2(defaultKnockbackAngle.x * facingDirection, defaultKnockbackAngle.y);
                rb.velocity = rbVelocity;

                rb.isKinematic = true;
                ApplyKnockback();
                rb.isKinematic = false;
            }
            holdBarObject.BombReleased();
        }
    }
}
