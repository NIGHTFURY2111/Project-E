public abstract class BaseState
{
    protected PlayerStateMachine ctx;
    protected StateFactory factory;
    public exitStates next;
    public BaseState(PlayerStateMachine ctx, StateFactory factory)
    {
        this.ctx = ctx;
        this.factory = factory;
    }
    public abstract bool SwitchCondintion();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();

    public virtual void LateUpdateState() { }

    public virtual void FixedUpdate() { }

    public void SwitchState(BaseState next)
    {
        factory.currentState.ExitState();
        factory.currentState = next;
        factory.currentState.EnterState();
    }

}
