using System.Collections.Generic;

public class ThrowState : IBehaviourState<PlayerActionStateObject>
{
    public StateHandle Handle => StateHandle.THROW;
    public List<StateTransition<PlayerActionStateObject>> Transitions { get; }

    public ThrowState()
    {
	Transitions = [
            new(Handle, StateHandle.NO_ACTION, NoActionTransition, 0),
        ];
    }

    public void Enter(PlayerActionStateObject stateObject, IBehaviourState<PlayerActionStateObject> previousState)
    {
        stateObject.ThrowHeldItem = true;
    }

    public void Exit(PlayerActionStateObject stateObject)
    {
    }

    public void Update(PlayerActionStateObject stateObject, double delta)
    {
    }
    
    public bool NoActionTransition(PlayerActionStateObject stateObject)
    {
        return stateObject.CurrentStateDuration > PlayerProperties.THROW_DURATION;
    }
}
