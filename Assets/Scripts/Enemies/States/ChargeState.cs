using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    protected D_ChaseState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingGround;
    protected bool isDetectingWall;
    protected bool isDetectingUpperLedge;
    protected bool isDetectingFloorLedge;
    protected bool isRotating;
    protected bool isInTheAir;

    protected bool performCloseRangeAction;
    protected bool playerJumpAboveCheck; 

    protected bool isJumping;

    protected bool ischaseTimeOver;

    public ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChaseState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();
        isRotating = false;
        ischaseTimeOver = false;
        core.Movement.SetVelocity(stateData.chaseSpeed, entity.facingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.chaseTime)
        {
            ischaseTimeOver = true;
        }

        if(playerJumpAboveCheck && !entity.isRotating)
        {
            entity.isRotating = true;
            Debug.Log("Player Jumped Above");
            entity.RotateEnemy(stateData.chaseSpeed);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isDetectingUpperLedge)
        {
            Jump();
        }

        if (isDetectingGround)
        {
            core.Movement.SetVelocity(stateData.chaseSpeed, entity.facingDirection);
        }
    }

    public void Jump()
    {
        core.Movement.SetVelocity(stateData.jumpSpeed, stateData.jumpAngle, entity.facingDirection);
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingWall = entity.CheckWall();
        isDetectingGround = entity.CheckGround();
        isDetectingFloorLedge = entity.CheckFloorLedge();
        isDetectingUpperLedge = entity.CheckUpperLedge();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        playerJumpAboveCheck = entity.CheckPlayerJumpAbove();
        isInTheAir = entity.CheckIfInTheAir();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();

        entity.anim.SetFloat("yVelocity", entity.rb.velocity.y);
        entity.anim.SetBool("isGrounded", isDetectingGround || !isInTheAir);
    }
}
