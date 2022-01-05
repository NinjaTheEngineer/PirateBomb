using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetectedState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isBombInMinAgroRange;
    protected bool isBombInMaxAgroRange;

    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool performBombCloseRangeAction;


    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        if (entity.isPlayerAlive)
        {
            performLongRangeAction = false;
            entity.SetDetectingTargetEffects(true);
        }
        core.Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
        entity.SetDetectingTargetEffects(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isBombInMinAgroRange = entity.CheckBombInMinAgroRange();
        isBombInMaxAgroRange = entity.CheckBombInMaxAgroRange();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        performBombCloseRangeAction = entity.CheckBombInCloseRangeAction();
    }
}
