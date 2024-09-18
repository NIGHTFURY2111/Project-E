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
        ctx.canAttack = true;
    }
    public override void UpdateState()
    {
        //CheckSwitchState();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchState()
    {//move jump //dash //fall
        
        
        //fall
        if (!ctx.isGrounded)
        {
            SwitchState(factory.Fall());
        }

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

    public override bool SwitchCondintion()
    {
       return ctx.isGrounded && !ctx.moveInput.IsInProgress();
    }
}
