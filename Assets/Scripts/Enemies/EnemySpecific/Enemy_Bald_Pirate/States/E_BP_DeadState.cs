using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BP_DeadState : DeadState
{
    private Enemy_Bald_Pirate enemy;

    public E_BP_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Enemy_Bald_Pirate enemy) : base(entity, stateMachine, animBoolName)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
