using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public Core Core { get; private set; }

    public D_Entity entityData;

    public int facingDirection { get; private set; }
    public bool isRotating { get;  set; }
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

    [SerializeField]
    private bool bUpperLedge;

    [SerializeField]
    private bool bFloorLedge;

    [SerializeField]
    private bool bCheckWall;

    [SerializeField]
    private bool bCheckGround;


    private Vector3 interrogationEffectsStartingPosition;
    private Vector2 velocityWorkspace;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        stateMachine = new FiniteStateMachine();
    }
    public virtual void Start()
    {
        interrogationEffectsStartingPosition = interrogationEffects.localPosition;
        SetDetectingTargetEffects(false);

        facingDirection = 1;
        isRotating = false;

        visualGO = transform.Find("Visual").gameObject;
        rb = visualGO.GetComponent<Rigidbody2D>();
        anim = visualGO.GetComponent<Animator>();
        atsm = visualGO.GetComponent<AnimationToStateMachine>();

    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return bCheckWall = Physics2D.Raycast(wallCheck.position, transform.right * facingDirection, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckGround()
    {
        return bCheckGround = Physics2D.Raycast(groundCheck.position, Vector2.down, entityData.groundCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckUpperLedge()
    {
        return bUpperLedge = (!Physics2D.Raycast(upperLedgeCheck.position, transform.right * facingDirection, entityData.upperLedgeCheckDistance, entityData.whatIsGround) &&
            CheckWall() && CheckGround());
    }

    public virtual bool CheckFloorLedge()
    {
        return bFloorLedge = (!Physics2D.Raycast(floorLedgeCheck.position, Vector2.down, entityData.floorLedgeCheckDistance, entityData.whatIsGround) &&
            CheckGround());
    }
    public virtual bool CheckPlayerJumpAbove()
    {
        return Physics2D.Raycast(floorLedgeCheck.position, Vector2.up, entityData.playerJumpAboveDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckIfInTheAir()
    {
        return rb.velocity.y != 0;
    }

    public virtual void SetDetectingTargetEffects(bool active)
    {
        interrogationEffects.gameObject.SetActive(active);

        if (facingDirection.Equals(1))
        {
            interrogationEffects.localPosition = interrogationEffectsStartingPosition;
        }
        else
        {
            interrogationEffects.localPosition = new Vector3(-interrogationEffectsStartingPosition.x, interrogationEffectsStartingPosition.y, 0f);
        }
    }

    public virtual void Jump()
    {
            rb.velocity = new Vector2(rb.velocity.x, 6.25f);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        visualGO.transform.Rotate(0f, 180f, 0f);
        HandleEffectsPositionOnFlip();
    }

    public virtual void RotateEnemy(float chaseSpeed)
    {
        StartCoroutine(IERotateEnemy(chaseSpeed));
    }
    IEnumerator IERotateEnemy(float chaseSpeed)
    {
        yield return new WaitForSecondsRealtime(0.35f);
        Debug.Log("Rotate Enemy");
        Flip();
        isRotating = false;
        Core.Movement.SetVelocity(chaseSpeed, facingDirection);
    }

    private void HandleEffectsPositionOnFlip()
    {
        interrogationEffects.Rotate(0f, 180f, 0f);
        interrogationEffects.localPosition = new Vector3(-interrogationEffects.localPosition.x, interrogationEffects.localPosition.y, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3)(Vector2.down * entityData.groundCheckDistance));
        
        Gizmos.DrawLine(upperLedgeCheck.position, upperLedgeCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.upperLedgeCheckDistance));
        
        Gizmos.DrawLine(floorLedgeCheck.position, floorLedgeCheck.position + (Vector3)(Vector2.down * entityData.floorLedgeCheckDistance));

        //Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
        //Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
        //Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);
    }

}
