using System.Collections.Generic;

public class GrabState : IBehaviourState<PlayerActionStateObject>
{
    public StateHandle Handle => StateHandle.GRAB;
    public List<StateTransition<PlayerActionStateObject>> Transitions { get; }

    public GrabState()
    {
	Transitions = [
            new (Handle, StateHandle.HOLD, HoldTransition, priority: 1),
            new (Handle, StateHandle.NO_ACTION, NoActionTransition, priority: 0)
	];
    }

    public void Enter(PlayerActionStateObject state, IBehaviourState<PlayerActionStateObject> previousState)
    {
    }

    public void Exit(PlayerActionStateObject state)
    {
    }

    public void Update(PlayerActionStateObject state, double delta)
    {
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
