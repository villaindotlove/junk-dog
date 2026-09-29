using System.Collections.Generic;
using Godot;

public class GrabState : IBehaviourState<PlayerActionStateObject>
{
    public string Name => StateHandle.GRAB;

    public void Enter(PlayerActionStateObject state, IBehaviourState<PlayerActionStateObject> previousState)
    {
    }

    public void Exit(PlayerActionStateObject state)
    {
    }

    public void Update(PlayerActionStateObject state, double delta)
    {
    }

    public List<StateTransition<PlayerActionStateObject>> GetTransitions()
    {
        return [
            new (Name, StateHandle.HOLD, HoldTransition, priority: 1),
            new (Name, StateHandle.NO_ACTION, NoActionTransition, priority: 0),
        ];
    }

    private bool NoActionTransition(PlayerActionStateObject state)
    {
        return !state.GrabBoxActive;
    }

    private bool HoldTransition(PlayerActionStateObject state)
    {
        return state.HasItem;
    }
}