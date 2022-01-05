using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BP_LookForPlayerState : LookForPlayerState
{
    Enemy_Bald_Pirate enemy;
    public E_BP_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, Enemy_Bald_Pirate enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!enemy.isPlayerAlive)
        {
            stateMachine.ChangeState(enemy.moveState);
            return;
        }
        if (isPlayerInMinAgroRange || isBombInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.moveState);
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
