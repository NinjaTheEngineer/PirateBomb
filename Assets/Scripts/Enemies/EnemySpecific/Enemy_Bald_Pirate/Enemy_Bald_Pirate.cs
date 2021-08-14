using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bald_Pirate : Entity
{
    public E_BP_IdleState idleState { get; private set; }
    public E_BP_MoveState moveState { get; private set; }

    public E_BP_PlayerDetectedState playerDetectedState { get; private set; }

    public E_BP_ChaseState ChaseState { get; private set; }

    public E_BP_LookForPlayerState lookForPlayerState { get; private set; }

    public E_BP_MeleeAttackState meleeAttackState { get; private set; }

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

        moveState = new E_BP_MoveState(this, stateMachine, "move", moveStateData, this, GetComponent<Timer>());
        idleState = new E_BP_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E_BP_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        ChaseState = new E_BP_ChaseState(this, stateMachine, "charge", ChaseStateData, this);
        lookForPlayerState = new E_BP_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E_BP_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);

        stateMachine.Initialize(idleState);

    }

    public void ChangeStateToIdleState()
    {
        idleState.SetFlipAfterIdle(GetRandomChance());
        stateMachine.ChangeState(idleState);
    }

    private bool GetRandomChance()
    {
        int rand = Random.Range(0, 100);
        return rand < 50;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
