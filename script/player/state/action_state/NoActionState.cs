using System.Collections.Generic;

public class NoActionState : IBehaviourState<PlayerActionStateObject>
{
    public StateHandle Handle => StateHandle.NO_ACTION;
    public List<StateTransition<PlayerActionStateObject>> Transitions { get; }
    
    public NoActionState()
    {
        Transitions = [
            new (Handle, StateHandle.GRAB, GrabTransition, priority: 0),
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

    private bool GrabTransition(PlayerActionStateObject stateObject)
    {
        return !stateObject.HasItem && stateObject.Input.GrabActuationBuffer.Last();
    }
}
