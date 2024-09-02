public class StateFactory
{
    PlayerStateMachine _context;
    IdleState idleState;
    JumpState jumpState;
    MoveState moveState;
    FallState fallState;
    DashState dashState;
    //SlideState slideState;
    //WallSlideState wallSlideState;
    //WallJumpState wallJumpState;
    //WallRunState wallRunState;
    //WallRunJumpState wallRunJumpState;
    //GrappleStart grappleStart;
    //GrapplePull grapplePull;
    //GrappleSwing grappleSwing;
    public StateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        idleState = new IdleState(_context, this);
        jumpState = new JumpState(_context, this);
        moveState = new MoveState(_context, this);
        fallState = new FallState(_context, this);
        dashState = new DashState(_context, this);

        //slideState = new SlideState(_context, this);
        //wallJumpState = new WallJumpState(_context, this);
        //wallSlideState = new WallSlideState(_context, this);
        //wallRunState = new WallRunState(_context, this);
        //wallRunJumpState = new WallRunJumpState(_context, this);
        //grappleStart = new GrappleStart(_context, this);
        //grapplePull = new GrapplePull(_context, this);
        //grappleSwing = new GrappleSwing(_context, this);

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
    //public BaseState Slide()
    //{
    //    return slideState;
    //}
    //public BaseState WallSlide()
    //{
    //    return wallSlideState;
    //}
    //public BaseState WallJump()
    //{
    //    return wallJumpState;
    //}
    //public BaseState WallRun()
    //{
    //    return wallRunState;
    //}
    //public BaseState WallRunJumpState()
    //{
    //    return wallRunJumpState;
    //}
    //public BaseState GrappleStart()
    //{
    //    return grappleStart;
    //}
    //public BaseState GrapllePull()
    //{
    //    return grapplePull;
    //}

    //public BaseState GrappleSwing()
    //{
    //    return grappleSwing;
    //}
    //DO THE SAME FOR ALL CLASSES
}
