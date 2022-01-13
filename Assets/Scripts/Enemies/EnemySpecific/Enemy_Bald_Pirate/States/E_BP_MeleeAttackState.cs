using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BP_MeleeAttackState : MeleeAttackState
{
    private Enemy_Bald_Pirate enemy;

    public E_BP_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Enemy_Bald_Pirate enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        if (isPlayerInMaxAgroRange || isBombInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.ChaseState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (!enemy.isPlayerAlive)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
                return;
            }
            if (isPlayerInMinAgroRange && isBombInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        SoundManager.Instance.PlayPirateKick();
        base.TriggerAttack();
    }
}
