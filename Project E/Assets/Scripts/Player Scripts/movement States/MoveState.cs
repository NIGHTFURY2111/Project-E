using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    Vector2 outputVector;
    public MoveState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }


    public override void EnterState()
    {
    }
    public override void UpdateState()
    {

        outputVector = ctx.playerInput * ctx.playerMoveSpeed;
        ctx.moveDirectionX = outputVector.x;
        //CheckSwitchState();
    }

    public override void ExitState()
    {

    }

    
    public override bool SwitchCondintion()
    {
        return ctx.isGrounded && ctx.moveInput.IsInProgress();
    }
}
