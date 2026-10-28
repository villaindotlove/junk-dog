using System.Collections.Generic;

public class MoveState : IBehaviourState<PlayerMovementStateObject>
{
    public StateHandle Handle => StateHandle.MOVE;
    public List<StateTransition<PlayerMovementStateObject>> Transitions { get; }

    public MoveState()
    {
	Transitions = [
            new (Handle, StateHandle.FALL, FallTransition, 5),
            new (Handle, StateHandle.IDLE, IdleTransition, 1),
            new (Handle, StateHandle.JUMP, JumpTransition, 0)
	];
    }

    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
    }

    public void Exit(PlayerMovementStateObject state)
    {
    }

    public void Update(PlayerMovementStateObject stateObject, double delta)
    {
    }

    private bool IdleTransition(PlayerMovementStateObject state)
    {
        return state.Input.MovementBuffer.Last().X == 0.0;
    }

    private bool FallTransition(PlayerMovementStateObject state)
    {
        return !state.IsOnFloor;
    }

    private bool JumpTransition(PlayerMovementStateObject state)
    {
        return state.Input.JumpActuationBuffer.Contains(true);
    }
}
