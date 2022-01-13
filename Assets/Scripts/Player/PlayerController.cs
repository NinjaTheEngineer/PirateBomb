using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour, IKnockbackable, IDamageable, IDefaultKnockback
{
    #region Variables

    public TextMeshProUGUI healthText;

    private int amountOfJumpsLeft;
    private int amountOfBombsLeft;
    private int healthAmount;

    private float bombsRefreshTimeLeft;
    private float holdDownStartTime;
    private float holdDownTime = 0f;
    private float walkTime = 0f;
    private float walkTimeStarted;
    private float walkEffectCd = 0.5f;
    private float knockbackStartTime;
    private float movementInputDirection;

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
    private bool isAtExitDoor = false;
    private bool enteredLevel = false;
    private bool exitingLevel = false;

    private Vector2 rbVelocity;
    private Animator exitDoor;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps;
    public int amountOfBombs;

    public int healthAmountMax = 3;

    public float bombsRefreshTime = 1.25f;
    public float minHoldDownTime = 0.25f;
    public float maxHoldDownTime = 2.5f;
    public float maxBombForce = 2f;

    public float movementSpeed = 5.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius = 0.125f;
    public float exitDoorCheckRadius = 0.05f;
    public float variableJumpHeightMultiplier = 0.5f;

    public float knockbackDuration = 0.75f;
    public float xKnockbackMultiplier = 1f;
    public float yKnockbackMultiplier = 1f;

    public Transform groundCheck;
    public Transform exitDoorCheck;
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
    public LayerMask whatIsExitDoor;
    public Vector2 defaultKnockbackAngle = Vector2.zero;

    public EventManager eventManager;
    public Core Core { get; private set; }
    #endregion

    private void Awake()
    {
        Core = GetComponentInChildren<Core>(); //Initialize Core
    }

    void Start() //Initialize default settings and fetch objects
    {
        exitDoor = GameObject.FindGameObjectWithTag("ExitDoor").GetComponent<Animator>();
        bombsRefreshTimeLeft = bombsRefreshTime;
        amountOfBombsLeft = amountOfBombs;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isHidden", true);
        amountOfJumpsLeft = amountOfJumps;
        healthAmount = healthAmountMax;
    }

    void Update() //Handles all the logic
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        RefreshBombsAmount();
        enteredLevel = GameManager.Instance.GameStarted;
    }

    private void FixedUpdate() //Handles all physics and detections
    {
        ApplyMovement();
        CheckSurroundings();
        CheckKnockback();
    }

    private void CheckSurroundings() //Checks if player is grounded or near the exit door
    {
        if(rb.velocity.y <= 0.01f)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
            isAtExitDoor = Physics2D.OverlapCircle(exitDoorCheck.position, exitDoorCheckRadius, whatIsExitDoor);
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
    private void CheckIfCanJump() //Checks if the player can jump
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

    private void RefreshBombsAmount() //Refreshs the amounts of bombs available, updates it with the UIManager
    {
        if(amountOfBombsLeft < amountOfBombs)
        {
            bombsRefreshTimeLeft -= Time.deltaTime;
            UIManager.Instance.UpdateBombReloadTime((bombsRefreshTime - bombsRefreshTimeLeft) / bombsRefreshTime);
            if (bombsRefreshTimeLeft < 0 && amountOfBombsLeft < amountOfBombs)
            {
                bombsRefreshTimeLeft = bombsRefreshTime;
                amountOfBombsLeft++;
                UIManager.Instance.UpdateBombAmount(amountOfBombsLeft);
            }
        }
        else
        {
            UIManager.Instance.UpdateBombReloadTime(1f);
        }
    }
    private void CheckInput() //Handles all input detection and functions
    {
        if (Input.GetKey(KeybindManager.Instance.Keybinds["Pause"]))
        {
            GameManager.Instance.OpenOptionsMenu();
        }
        if (isAlive && canMove && enteredLevel && !exitingLevel)
        {
            if (Input.GetKey(KeybindManager.Instance.Keybinds["Left"]))
            {
                movementInputDirection = -1;
            }
            else if (Input.GetKey(KeybindManager.Instance.Keybinds["Right"]))
            {
                movementInputDirection = 1;
            }
            else
            {
                movementInputDirection = 0;
            }

            if (Input.GetKeyDown(KeybindManager.Instance.Keybinds["Jump"]))
            {
                SoundManager.Instance.PlayPlayerJump();
                Jump();
            }

            if (Input.GetKeyUp(KeybindManager.Instance.Keybinds["Jump"]))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }

            if (Input.GetKeyDown(KeybindManager.Instance.Keybinds["Bomb"]))
            {
                if (isAtExitDoor)
                {
                    anim.SetTrigger("doorIn");
                    exitDoor.SetTrigger("open");
                    exitingLevel = true;
                    rb.velocity = Vector2.zero;
                    SoundManager.Instance.PlayDoorOpen();
                    Invoke("ExitLevel", 0.75f);
                    return;
                }
                if (amountOfBombsLeft > 0) { 
                    holdDownStartTime = Time.time;
                    holdingBomb = true;
                }
            }

            if (Input.GetKeyUp(KeybindManager.Instance.Keybinds["Bomb"]))
            {
                if (holdingBomb){
                    PlaceBomb(holdDownTime);
                    holdingBomb = false;
                }
            }
        }
        
    }
    private void ExitLevel() //Loads next scene when player enters a door
    {
        LevelLoader.Instance.LoadNextScene();
    }

    private void CheckMovementDirection() //Handles the movement direction and effects
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
    private void ApplyMovement() //Applies the movement for when the player can move
    {
        if (!isAlive && isGrounded && !isKnockbackActive || exitingLevel)
        {
            rb.velocity = Vector2.zero;
            if (exitingLevel)
            {
                return;
            }
            SoundManager.Instance.PlayPlayerDead();
            return;
        }

        if (canMove && isAlive)
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }

    private void PlaceBomb(float holdDownTime) //Place or throws the bomb
    {
        if(amountOfBombsLeft > 0)
        {
            BombController bombPlaced = Instantiate(bombPrefab, bombPlacement.position, Quaternion.identity).GetComponent<BombController>();
            amountOfBombsLeft--;
            UIManager.Instance.UpdateBombAmount(amountOfBombsLeft);

            if (holdDownTime >= minHoldDownTime)
            {
                holdBarObject.BombReleased();
                bombPlaced.LaunchBomb(CalculateHoldDownForce(holdDownTime), isFacingRight ? 1 : -1);
            }

        }
    }

    private float CalculateHoldDownForce(float holdTime) //Calculates throw force with the amount or time the throw button was held
    {
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxHoldDownTime);
        Debug.Log("CalculateHoldDownForce -> " + holdTimeNormalized);
        return holdTimeNormalized * maxBombForce;
    }

    private void Flip() //Flips the player
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180f, 0f);
    }

    private void Jump() //Vertical jump
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

    private void UpdateAnimations() //Updates all the animations
    {
        anim.SetBool("walking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isKnockback", isKnockbackActive);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isAlive", isAlive);
        anim.SetBool("canMove", canMove);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isHidden", !enteredLevel);
        HandleBombBarAnimations();
        HandleRunningEffect();
    }
    private void HandleBombBarAnimations() //Handles the bomb force bar animation
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
    private void HandleRunningEffect() //Handles the running animation
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
    private void OnDrawGizmos() //For debugging purposes
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    public void Knockback(float knockbackStrength, Vector2 knockbackOriginPosition) //Handles the knockback applied to the player, either from bombs or pirate
    {
        if (!isKnockbackActive && !exitingLevel)
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
    public bool IsAlive() //Returns if the player is alive
    {
        return isAlive;
    }
    public void ApplyKnockback() //Applies the knockback to the rigidbody
    {
        Debug.Log("KNOCKBACK Angle : " + rbVelocity);
        rb.velocity = Vector2.zero;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
        rb.velocity = rbVelocity;
        canMove = false;
    }
    public void Damage(int amount) //Damage the player
    {
        SoundManager.Instance.PlayPlayerHit();
        isHit = true;
        healthAmount -= amount;
        UIManager.Instance.UpdatePlayerHealth(healthAmount);
        Debug.Log("HEALTH: " + healthAmount);
        if (healthAmount <= 0f)
        {
            GameManager.Instance.GameOver();
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
        if (!isKnockbackActive && !exitingLevel)
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
