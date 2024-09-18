using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : BaseState
{
    float normalClampValue;
    public WallSlideState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        normalClampValue = ctx.setGravityClamp ;
        ctx.setGravityClamp = ctx.wallGravityClamp;
    }

    public override void UpdateState()
    {
        //CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx.setGravityClamp = normalClampValue;
    }

    public override void CheckSwitchState()
    {
        //idle
        if (ctx.isGrounded)
        {
            SwitchState(factory.Idle());
        }

        //fall
        if (!ctx.touchingWall)
        {
            SwitchState(factory.Fall());
        }

        ////wallJump
        //if (ctx.jumpInput.WasPressedThisFrame())
        //{

        //}
    }

    public override bool SwitchCondintion()
    {
        return !ctx.isGrounded && ctx.touchingWall;
    }
}
