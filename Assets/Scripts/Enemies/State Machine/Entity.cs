using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    
    public GameObject visualGO { get; private set; }

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;

    [SerializeField]
    private Transform playerCheck;

    [SerializeField]
    private Transform interrogationEffects;

    private Vector3 interrogationEffectsStartingPosition;
    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        interrogationEffectsStartingPosition = interrogationEffects.localPosition;
        SetDetectingTargetEffects(false);

        facingDirection = 1;

        visualGO = transform.Find("Visual").gameObject;
        rb = visualGO.GetComponent<Rigidbody2D>();
        anim = visualGO.GetComponent<Animator>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
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
        return Physics2D.Raycast(wallCheck.position, transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, visualGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
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

    public virtual void Flip()
    {
        facingDirection *= -1;
        visualGO.transform.Rotate(0f, 180f, 0f);
        HandleEffectsPositionOnFlip();
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
    }

    private void HandleEffectsPositionOnFlip()
    {
        interrogationEffects.Rotate(0f, 180f, 0f);
        interrogationEffects.localPosition = new Vector3(-interrogationEffects.localPosition.x, interrogationEffects.localPosition.y, 0f);
    }

}
