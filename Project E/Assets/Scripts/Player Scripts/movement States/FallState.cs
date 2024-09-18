using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BaseState
{
    public FallState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }


    public override void EnterState()
    {
        ctx.canAttack = false;

    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        //CheckSwitchState();
    }
    public override void CheckSwitchState()
    {
        if (ctx.isGrounded)
        {
            SwitchState(factory.Idle());
        }

        //if (ctx.jumpInput.WasPressedThisFrame())
        //{
        //    SwitchState(factory.Jump());
        //}

        //dash
        if (ctx.dashInput.WasPressedThisFrame())
        {
            SwitchState(factory.Dash());
        }


        if (ctx.touchingWall)
        {
            SwitchState(factory.WallSlide());
            
        }

    }

    public override bool SwitchCondintion()
    {
        return !ctx.isGrounded && ctx.moveDirectionY <= 0;
    }
}
