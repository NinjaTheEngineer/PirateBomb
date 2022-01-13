using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected Core core;

    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    protected float startTime;
    protected float lastRotateTime;

    protected string animBoolName;

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName) //State contructor
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        core = entity.Core;
    }

    public virtual void Enter() //Like a state Start() method
    {
        startTime = Time.time;
        lastRotateTime = startTime;
        entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }

    public virtual void Exit() //Exit method 
    {
        entity.anim.SetBool(animBoolName, false);
    }
    public virtual void LogicUpdate() //Update method, logic update
    {

    }
    public virtual void PhysicsUpdate() //Physics update
    {
        DoChecks();
    }

    public virtual void DoChecks() //Do Physics checks
    {

    }
}
