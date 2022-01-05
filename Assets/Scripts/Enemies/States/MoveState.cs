using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingGround;
    protected bool isPlayerInMinAgroRange;
    protected bool isBombInMinAgroRange;
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        core.Movement.SetVelocity(stateData.movementSpeed, core.Movement.FacingDirection);
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
    public override void DoChecks()
    {
        base.DoChecks();

        isDetectingWall = core.CollisionSenses.WallFront;
        isDetectingGround = core.CollisionSenses.Ground;
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isBombInMinAgroRange = entity.CheckBombInMinAgroRange();
    }
}
