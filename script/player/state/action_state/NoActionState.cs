
using System.Collections.Generic;
using Godot;

public class NoActionState : IBehaviourState<PlayerActionStateObject>
{
    public string Name => StateHandle.NO_ACTION;

    public void Enter(PlayerActionStateObject stateObject, IBehaviourState<PlayerActionStateObject> previousState)
    {
    }

    public void Exit(PlayerActionStateObject stateObject)
    {
    }

    public void Update(PlayerActionStateObject stateObject, double delta)
    {
    }

    public List<StateTransition<PlayerActionStateObject>> GetTransitions()
    {
        return [
            new (Name, StateHandle.GRAB, GrabTransition, priority: 0),
        ];
    }

    private bool GrabTransition(PlayerActionStateObject stateObject)
    {
        return !stateObject.HasItem && stateObject.Input.GrabActuationBuffer.Last();
    }
}