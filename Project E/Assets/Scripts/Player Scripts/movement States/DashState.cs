using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashState : BaseState
{
    Vector2 finalVector;
    public DashState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    }

   
    public override void EnterState()
    {
        
        finalVector = ((ctx.moveInput.IsPressed())? ctx.playerInput : Vector2.right * Vector3.Dot(Vector2.right,ctx.transform.forward)) * ctx.dashSpeedMultiplier;
        ctx.StartCoroutine(Dash());
    }
    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }

    IEnumerator Dash()
    {
           
            
                //Vector2 direction = move.ReadValue<Vector2>();
                //Vector2 storing = rb.velocity;

                ctx.moveDirectionX = 0;
                ctx.moveDirectionY = 0;
                ctx.gravity = ctx.dashGravity;
                yield return new WaitForSecondsRealtime(0.05f);
                ctx.moveDirectionX = finalVector.x;
                ctx.moveDirectionY = finalVector.y;
                yield return new WaitForSecondsRealtime(ctx.dashTime);

                //rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, float.MinValue, vertMaxDash));
                ctx.gravity = ctx.normalGravity;
                
            
    }

    public override bool SwitchCondintion()
    {
      
        return ctx.dashInput.WasPressedThisFrame();
    }
}
