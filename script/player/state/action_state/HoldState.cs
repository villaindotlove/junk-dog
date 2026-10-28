using System.Collections.Generic;

public class HoldState : IBehaviourState<PlayerActionStateObject>
{
    public StateHandle Handle => StateHandle.HOLD;
    public List<StateTransition<PlayerActionStateObject>> Transitions { get; }

    public HoldState()
    {
	Transitions = [
            new(Handle, StateHandle.THROW, ThrowTransition, 0)
        ];
    }

    public void Enter(PlayerActionStateObject stateObject, IBehaviourState<PlayerActionStateObject> previousState)
    {
    }

    public void Exit(PlayerActionStateObject stateObject)
    {
    }

    public void Update(PlayerActionStateObject stateObject, double delta)
    {
    }

    private bool ThrowTransition(PlayerActionStateObject state)
    {
        return state.Input.GrabActuationBuffer.Last();
    }
}
