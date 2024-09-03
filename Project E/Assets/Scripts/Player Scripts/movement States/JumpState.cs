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
        ctx.moveDirectionY += ctx.playerJumpingForce;
        ctx.gravity = ctx.jumpGravity;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
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
    }

}
