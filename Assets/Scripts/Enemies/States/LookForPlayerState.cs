﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayerState stateData;

    protected bool turnImmediately;

    protected bool isPlayerInMinAgroRange;
    protected bool isBombInMinAgroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;

    protected int amountOfTurnsDone;

    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        if (entity.isPlayerAlive)
        {
            base.Enter();
        
            entity.SetDetectingTargetEffects(true);

            isAllTurnsDone = false;
            isAllTurnsTimeDone = false;

            lastTurnTime = startTime;
            amountOfTurnsDone = 0;
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

        if (turnImmediately)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        else if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }

        if (amountOfTurnsDone >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }

        if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
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
        isBombInMinAgroRange = entity.CheckBombInMinAgroRange();
    }

    public void SetTurnImmediately(bool turn)
    {
        turnImmediately = turn; 
    }
}
