using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class stateholder
{
    [SerializeReferenceDropdown]
    [SerializeReference]
    public BaseState state;
    public exitStates stateEnum;
      
}
[Serializable]
public class StateFactory
{
    PlayerStateMachine _context;
    IdleState idleState;
    JumpState jumpState;
    MoveState moveState;
    FallState fallState;
    DashState dashState;
    WallSlideState wallSlideState;
    public List<stateholder> stateholders = new();
    public BaseState currentState;
    
    public StateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        //idleState = new IdleState(_context, this);
        //idleState.next = exitStates.move | exitStates.fall | exitStates.jump | exitStates.dash;
        //jumpState = new JumpState(_context, this);
        //jumpState.next = exitStates.fall | exitStates.dash;
        //moveState = new MoveState(_context, this);
        //moveState.next = exitStates.fall | exitStates.dash | exitStates.move | exitStates.jump | exitStates.idle;
        //fallState = new FallState(_context, this);
        //fallState.next = exitStates.wallslide | exitStates.dash | exitStates.move | exitStates.idle;
        //dashState = new DashState(_context, this);
        //dashState.next = exitStates.idle;
        //wallSlideState = new WallSlideState(_context, this);
        //wallSlideState.next = exitStates.idle;
        

        //statedick.Add(exitStates.idle, idleState);
        //statedick.Add(exitStates.move, moveState);
        //statedick.Add(exitStates.dash, dashState);
        //statedick.Add(exitStates.jump, jumpState);
        //statedick.Add(exitStates.fall, fallState);
        //statedick.Add(exitStates.wallslide, wallSlideState);



    }

    public BaseState Idle()
    {
        return idleState;
    }
    public BaseState Jump()
    {
        return jumpState;
    }
    public BaseState Move()
    {
        return moveState;
    }
    public BaseState Dash()
    {
        return dashState;
    }
    public BaseState Fall()
    {
        return fallState;
    }

    public BaseState WallSlide()
    {
        return wallSlideState;
    }

    public void update()
    {
        currentState.UpdateState();
        switchCheck(currentState.next);
        Debug.Log(currentState);
    }

    public void fixedUpdate() 
    {
        currentState.FixedUpdate();
    }
    protected void switchCheck(exitStates next)
    {

        foreach (var holder in stateholders)
        {
            if (next.HasFlag(holder.stateEnum) && holder.state.SwitchCondintion())
            {
                currentState.SwitchState(holder.state);
                return;
            }
        }

    }
    protected BaseState fetch(exitStates exitStates)
    {
        foreach(var holder in stateholders)
        {
            if (exitStates.HasFlag(holder.stateEnum))
            {
                return holder.state;
            }
        }
        return null;
    }


}
