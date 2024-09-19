using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class stateholder
{
    [SerializeReferenceDropdown]
    [SerializeReference]
    public BaseState state;
    public exitStates stateEnum;
      
}
[Serializable]
public class StateFactory
{
    [SerializeField]PlayerStateMachine _context;
    [SerializeField] List<stateholder> stateholders = new();
    [SerializeField] exitStates initial;
    [SerializeField] BaseState currentState;
    
    public void update()
    {
        currentState.UpdateState();
        switchCheck(currentState._next);
        Debug.Log(currentState);
    }

    public void fixedUpdate() 
    {
        currentState.FixedUpdate();
    }
    protected void switchCheck(exitStates next)
    {

        foreach (var holder in stateholders)
        {
            if (next.HasFlag(holder.stateEnum) && holder.state.SwitchCondintion())
            {
                currentState.SwitchState(holder.state);
                return;
            }
        }

    }
    protected BaseState fetch(exitStates exitStates)
    {
        foreach(var holder in stateholders)
        {
            if (exitStates.HasFlag(holder.stateEnum))
            {
                return holder.state;
            }
        }
        return null;
    }

    public void setPrerequisites()
    {
        foreach (stateholder holder in stateholders)
        {
            holder.state.prerequisites(_context,this);
        }
        currentState = fetch(initial);
    }

    public BaseState _currentState { get => currentState; set => currentState = value; }
}
