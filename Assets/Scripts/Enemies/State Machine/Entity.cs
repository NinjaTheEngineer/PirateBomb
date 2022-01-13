using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IKnockbackable, IDamageable //Class for any enemy
{
    public FiniteStateMachine stateMachine;
    public Core Core { get; private set; }

    public D_Entity entityData;

    public int currentHealth;
    public int facingDirection { get; private set; }
    public bool isRotating { get;  set; }

    public bool isPlayerAlive { get; set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject visualGO { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform floorLedgeCheck;

    [SerializeField]
    private Transform upperLedgeCheck;

    [SerializeField]
    private Transform playerCheck;

    [SerializeField]
    private Transform playerJumpAboveCheck;

    [SerializeField]
    private Transform interrogationEffects;

    private bool isInTheAir;

    private Vector3 interrogationEffectsStartingPosition;
    private Vector2 velocityWorkspace;

    public virtual void Awake() //Initialize Core and FSM
    {
        Core = GetComponentInChildren<Core>();
        stateMachine = new FiniteStateMachine();
    }
    public virtual void Start() //Initialize variables and effects
    {
        isPlayerAlive = true;
        EventManager.OnPlayerDeath += PlayerIsDead;
        interrogationEffectsStartingPosition = interrogationEffects.localPosition;
        SetDetectingTargetEffects(false);

        isRotating = false;

        currentHealth = entityData.maxHealthAmount;
        visualGO = transform.Find("Visual").gameObject;
        rb = GetComponent<Rigidbody2D>();
        anim = visualGO.GetComponent<Animator>();
        atsm = visualGO.GetComponent<AnimationToStateMachine>();

    }

    public virtual void Update() //Update Logic
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
        UpdateAnimations();
    }

    private void UpdateAnimations() //Updates animations 
    {
        isInTheAir = CheckIfInTheAir();
        anim.SetBool("isGrounded", Core.CollisionSenses.Ground && !isInTheAir);
        anim.SetBool("isPlayerAlive", isPlayerAlive);
    }

    public virtual void FixedUpdate() //Physics update
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity) //Sets velocity to the entity
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckPlayerJumpAbove() //Check if the player jumped above
    {
        return Physics2D.Raycast(playerJumpAboveCheck.position, Vector2.up, entityData.playerJumpAboveDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMinAgroRange() //Check if player in min distance to do something
    {
        if(Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer) && isPlayerAlive)
        {
            float distance = Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer).distance;
            return !Core.CollisionSenses.WallBeforePlayer(distance);
        }
        else
        {
            return false;
        }
    }
    public virtual bool CheckPlayerInMaxAgroRange() //Check if palyer in max distance to do something
    {
        return (Physics2D.CircleCast(playerCheck.position, entityData.maxAgroDistance, visualGO.transform.right, entityData.whatIsPlayer) && isPlayerAlive);
    }
    public virtual bool CheckBombInMinAgroRange() //Check if bomb in min distance to do something
    {
        return (Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.minAgroDistance, entityData.whatIsBomb));
    }
    public virtual bool CheckBombInMaxAgroRange() //Check if bomb in max distance to do something
    {
        return (Physics2D.CircleCast(playerCheck.position, entityData.bombMaxAgroRange, visualGO.transform.right, entityData.whatIsBomb));
    }

    public virtual bool CheckPlayerInCloseRangeAction() //Check if player in range for close action
    {
        return Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckBombInCloseRangeAction() //Check if bomb in range for close action
    {
        return Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsBomb);
    }

    public virtual bool CheckIfInTheAir() //Check if isn't grounded
    {
        return !(Core.Movement.CurrentVelocity.y <= 0.005f && Core.Movement.CurrentVelocity.y >= -0.005f);
    }

    public virtual void SetDetectingTargetEffects(bool active) //Set detecting target effects
    {
        if (active)
        {
            SoundManager.Instance.PlayPirateHm();
        }
        interrogationEffects.gameObject.SetActive(active);

        if (Core.Movement.FacingDirection.Equals(1))
        {
            interrogationEffects.localPosition = interrogationEffectsStartingPosition;
        }
        else
        {
            interrogationEffects.localPosition = new Vector3(-interrogationEffectsStartingPosition.x, interrogationEffectsStartingPosition.y, 0f);
        }
    }

    public virtual void Jump() //Jump
    {
        rb.velocity = new Vector2(rb.velocity.x, 6.25f);
    }

    public virtual void Flip() //Flip character
    {
        Core.Movement.Flip();
        HandleEffectsPositionOnFlip();
    }

    public virtual void RotateEnemy(float chaseSpeed) //Rotate enemy
    {
        StartCoroutine(IERotateEnemy(chaseSpeed));
    }
    IEnumerator IERotateEnemy(float chaseSpeed) //Rotate after time
    {
        yield return new WaitForSecondsRealtime(0.35f);
        Debug.Log("Rotate Enemy");
        Flip();
        isRotating = false;
        Core.Movement.SetVelocity(chaseSpeed, facingDirection);
    }

    private void HandleEffectsPositionOnFlip() //Handle the position of the detecting effect
    {
        interrogationEffects.localPosition = new Vector3(-interrogationEffects.localPosition.x, interrogationEffects.localPosition.y, 0f);
        interrogationEffects.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos() //Debug purpose
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3)(Vector2.down * entityData.groundCheckDistance));
        
        Gizmos.DrawLine(upperLedgeCheck.position, upperLedgeCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.upperLedgeCheckDistance));
        
        Gizmos.DrawLine(floorLedgeCheck.position, floorLedgeCheck.position + (Vector3)(Vector2.down * entityData.floorLedgeCheckDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right), entityData.maxAgroDistance);
    }

    public virtual void Damage(int amount) //Damge entity
    {
        currentHealth -= amount;
    }
    public virtual void Knockback(float knockbackStrength, Vector2 bombPosition) //Handle entity knockback
    {
        Core.Combat.Knockback(knockbackStrength, bombPosition);
    }

    public void PlayerIsDead() //Set player dead
    {
        isPlayerAlive = false;
        EventManager.OnPlayerDeath -= PlayerIsDead;
    }
}
