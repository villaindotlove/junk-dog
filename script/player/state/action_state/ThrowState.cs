using System.Collections.Generic;

public class ThrowState : IBehaviourState<PlayerActionStateObject>
{
    public string Name => StateHandle.THROW;

    public void Enter(PlayerActionStateObject stateObject, IBehaviourState<PlayerActionStateObject> previousState)
    {
        stateObject.ThrowHeldItem = true;
    }

    public void Exit(PlayerActionStateObject stateObject)
    {
    }

    public List<StateTransition<PlayerActionStateObject>> GetTransitions()
    {
        return [
            new(Name, StateHandle.NO_ACTION, NoActionTransition, 0),
        ];
    }

    public void Update(PlayerActionStateObject stateObject, double delta)
    {
    }
    
    public bool NoActionTransition(PlayerActionStateObject stateObject)
    {
        return stateObject.CurrentStateDuration > PlayerProperties.THROW_DURATION;
    }
}