using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected bool isKnockableActive;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isKnockableActive = core.Combat.isKnockbackActive;
    }

    public override void Enter()
    {
        isKnockableActive = true;
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
