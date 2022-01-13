using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BP_StunState : StunState
{
    Enemy_Bald_Pirate enemy;
    public E_BP_StunState(Entity entity, FiniteStateMachine stateMachine, Enemy_Bald_Pirate enemy, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        SoundManager.Instance.PlayPirateHit();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (enemy.currentHealth <= 0f)
        {
            stateMachine.ChangeState(enemy.deadState);
        }
        else if (!isKnockableActive)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
