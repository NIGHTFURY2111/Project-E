using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        ctx.moveDirectionX = 0f;
        ctx.moveDirectionY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {//move jump
        
        //move
        if (ctx.moveInput.IsInProgress())
        {
            SwitchState(factory.Move());
        }

        //jump
        if (ctx.jumpInput.WasPressedThisFrame())
        {
            SwitchState(factory.Jump());
        }

        //dash
        if (ctx.dashInput.WasPressedThisFrame())
        {
            SwitchState(factory.Dash());
        }

    }
}
