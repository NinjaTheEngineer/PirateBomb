using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BP_ChaseState : ChaseState
{
    private Enemy_Bald_Pirate enemy;
    public E_BP_ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChaseState stateData, Enemy_Bald_Pirate enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        //Debug.Log("E_BP: ENTER Chase State");
        base.Enter();
    }

    public override void Exit()
    {
        //Debug.Log("E_BP: EXIT Chase State");
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (performBombCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (isChaseTimeOver)
        {
            if (isPlayerInMinAgroRange && isBombInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
        else if (!isPlayerInMaxAgroRange || (isDetectingWall && !isDetectingUpperLedge))
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();

    }
}
