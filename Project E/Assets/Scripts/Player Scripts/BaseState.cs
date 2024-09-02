public abstract class BaseState
{
    protected PlayerStateMachine ctx;
    protected StateFactory factory;
    public BaseState(PlayerStateMachine ctx, StateFactory factory)
    {
        this.ctx = ctx;
        this.factory = factory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();

    public virtual void LateUpdateState() { }

    public virtual void FixedUpdate() { }

    protected void SwitchState(BaseState next)
    {
        ctx.currentState.ExitState();
        ctx.currentState = next;
        ctx.currentState.EnterState();
    }

}
