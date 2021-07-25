﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BP_PlayerDetectedState : PlayerDetectedState
{
    private Enemy_Bald_Pirate enemy;

    private bool isPlayerDetectedOver;
    public E_BP_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, Enemy_Bald_Pirate enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        Debug.Log("E_BP: 'ENTER' PlayerDetected State");
        isPlayerDetectedOver = false;
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("E_BP: 'EXIT' PlayerDetected State");
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }

        //TODO: Transition to Attack
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
