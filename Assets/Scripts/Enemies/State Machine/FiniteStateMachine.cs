using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public State currentState { get; private set; } //global get, private set

    public void Initialize(State startingState) //Initialize state machine
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(State newState) //Change any state
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
