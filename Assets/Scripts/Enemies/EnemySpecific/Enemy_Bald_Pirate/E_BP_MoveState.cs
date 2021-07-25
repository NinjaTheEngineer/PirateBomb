using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BP_MoveState : MoveState
{
    private Enemy_Bald_Pirate enemy;
    private Timer timer;
    public E_BP_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enemy_Bald_Pirate enemy, Timer timer) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        this.timer = timer;
    }

    public override void Enter()
    {
        base.Enter();
        timer.StartCoroutine(Random.Range(2.0f, 8.0f));
    }

    public override void Exit()
    {
        base.Exit();
        timer.StopAllCoroutines();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if(isDetectingWall || !isDetectingLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
