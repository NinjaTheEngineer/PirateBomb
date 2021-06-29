using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bald_Pirate : Entity
{
    public E_BP_IdleState idleState { get; private set; }
    public E_BP_MoveState moveState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;

    [SerializeField]
    private D_MoveState moveStateData;

    public override void Start()
    {
        base.Start();

        moveState = new E_BP_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E_BP_IdleState(this, stateMachine, "idle", idleStateData, this);

        stateMachine.Initialize(idleState);
    }

}
