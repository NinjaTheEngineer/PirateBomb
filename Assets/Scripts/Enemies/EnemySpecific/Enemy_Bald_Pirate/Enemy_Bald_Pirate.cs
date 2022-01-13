using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bald_Pirate : Entity
{
    public E_BP_TutorialState tutorialState { get; private set; }
    public E_BP_IdleState idleState { get; private set; }
    public E_BP_MoveState moveState { get; private set; }

    public E_BP_PlayerDetectedState playerDetectedState { get; private set; }

    public E_BP_ChaseState ChaseState { get; private set; }

    public E_BP_LookForPlayerState lookForPlayerState { get; private set; }

    public E_BP_MeleeAttackState meleeAttackState { get; private set; }
    public E_BP_StunState stunState { get; private set; }
    public E_BP_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;

    [SerializeField]
    private D_MoveState moveStateData;

    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;

    [SerializeField]
    private D_ChaseState ChaseStateData;

    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;

    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;

    [SerializeField]
    private Transform meleeAttackPosition;


    public override void Start()
    {
        base.Start();

        tutorialState = new E_BP_TutorialState(this, stateMachine, "tutorial", this);
        moveState = new E_BP_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E_BP_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E_BP_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        ChaseState = new E_BP_ChaseState(this, stateMachine, "charge", ChaseStateData, this);
        lookForPlayerState = new E_BP_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E_BP_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new E_BP_StunState(this, stateMachine, this,"knockback");
        deadState = new E_BP_DeadState(this, stateMachine, "dead", this);

        if (GameManager.Instance.IsTutorial())
        {
            stateMachine.Initialize(tutorialState);
            Flip();
        }
        else
        {
            stateMachine.Initialize(idleState);
        }

    }

    public void ChangeStateToIdleState()
    {
        stateMachine.ChangeState(idleState);
    }

    public override void Knockback(float knockbackStrength, Vector2 bombPosition)
    {
        base.Knockback(knockbackStrength, bombPosition);
        stateMachine.ChangeState(stunState);
    }

    public override void Damage(int amount)
    {
        base.Damage(amount);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
