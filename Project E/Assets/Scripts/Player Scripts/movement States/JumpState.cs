using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }


    public override void EnterState()
    {
        ctx.moveDirectionY = 0;
        ctx.gravity = 0;
        ctx.moveDirectionY += ctx.playerJumpingForce;
        ctx.gravity = ctx.jumpGravity;
        ctx.canAttack = false;

    }

    public override void UpdateState()
    {
        //dynamic height for jumps
        ctx.gravity = (ctx.jumpInput.IsInProgress()) ? ctx.jumpGravity : ctx.normalGravity;

        //CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx.gravity = ctx.normalGravity;
    }

    public override void CheckSwitchState()
    {//fall

        //fall
        if (ctx.moveDirectionY <= 0)
        {
            SwitchState(factory.Fall());
        }
        //jump into another jump
        //if (ctx.jumpInput.WasPressedThisFrame())
        //{
        //    SwitchState(factory.Jump());
        //}

        //dash
        if (ctx.dashInput.WasPressedThisFrame())
        {
            SwitchState(factory.Dash());
        }

    }

    public override bool SwitchCondintion()
    {
        return ctx.isGrounded && ctx.jumpInput.WasPressedThisFrame();
    }
}
