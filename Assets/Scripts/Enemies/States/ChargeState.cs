﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    protected D_ChaseState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isBombInMinAgroRange;
    protected bool isDetectingGround;
    protected bool isDetectingWall;
    protected bool isDetectingUpperLedge;
    protected bool canJumpLedge;
    protected bool isDetectingFloorLedge;
    protected bool isRotating;
    protected bool isInTheAir;


    protected bool performCloseRangeAction;
    protected bool performBombCloseRangeAction;
    protected bool playerJumpAboveCheck; 

    protected bool isJumping;

    protected bool isChaseTimeOver;

    public ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChaseState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();
        isRotating = false;
        isChaseTimeOver = false;
        core.Movement.SetVelocity(stateData.chaseSpeed, core.Movement.FacingDirection);
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
            isChaseTimeOver = true;
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

        if (canJumpLedge)
        {
            Jump();
        }

        if (isDetectingGround)
        {
            core.Movement.SetVelocity(stateData.chaseSpeed, core.Movement.FacingDirection);
        }
    }

    public void Jump()
    {
        core.Movement.SetVelocity(stateData.jumpSpeed, stateData.jumpAngle, core.Movement.FacingDirection);
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingWall = core.CollisionSenses.WallFront;
        isDetectingGround = core.CollisionSenses.Ground;
        isDetectingFloorLedge = core.CollisionSenses.FloorLedge;
        canJumpLedge = core.CollisionSenses.CanJumpLedge;
        isDetectingUpperLedge = core.CollisionSenses.UpperLedge;
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isBombInMinAgroRange = entity.CheckBombInMinAgroRange();
        playerJumpAboveCheck = entity.CheckPlayerJumpAbove();
        isInTheAir = entity.CheckIfInTheAir();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        performBombCloseRangeAction = entity.CheckBombInCloseRangeAction();

        entity.anim.SetFloat("yVelocity", entity.rb.velocity.y);
        entity.anim.SetBool("isGrounded", isDetectingGround && !isInTheAir);
    }
}
