using System.Collections.Generic;

public class HoldState : IBehaviourState<PlayerActionStateObject>
{
    public string Name => StateHandle.HOLD;

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
            new(Name, StateHandle.THROW, ThrowTransition, 0)
        ];
    }

    private bool ThrowTransition(PlayerActionStateObject state)
    {
        return state.Input.GrabActuationBuffer.Last();
    }
}