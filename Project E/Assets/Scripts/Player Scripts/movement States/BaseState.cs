using System;
using UnityEngine;

public abstract class BaseState
{
    protected PlayerStateMachine ctx;
    protected StateFactory factory;
    [SerializeField]exitStates next;
    public BaseState(PlayerStateMachine ctx, StateFactory factory)
    {
       
    }
    public abstract bool SwitchCondintion();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
   

    public virtual void LateUpdateState() { }

    public virtual void FixedUpdate() { }

    public void SwitchState(BaseState next)
    {
        factory._currentState.ExitState();
        factory._currentState = next;
        factory._currentState.EnterState();
    }

    public void  prerequisites(PlayerStateMachine ctx,StateFactory factory) 
    {
        this.ctx = ctx;
        this.factory = factory;
    }

    public exitStates _next { get => next; }
    


}
