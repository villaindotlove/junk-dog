using System.Collections.Generic;

public class IdleState : IBehaviourState<PlayerMovementStateObject>
{
    public StateHandle Handle => StateHandle.IDLE;

    public List<StateTransition<PlayerMovementStateObject>> Transitions { get; }

    public IdleState()
    {
	Transitions = [
            new (Handle, StateHandle.JUMP, JumpTransition, priority: 5),
            new (Handle, StateHandle.MOVE, MoveTransition, priority: 1),
            new (Handle, StateHandle.FALL, FallTransition, priority: 0),
        ];

    }

    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
    }

    public void Exit(PlayerMovementStateObject state)
    {
    }

    public void Update(PlayerMovementStateObject state, double delta)
    {
    }

    private bool JumpTransition(PlayerMovementStateObject state)
    {
        return state.Input.JumpActuationBuffer.Contains(true);
    }

    private bool MoveTransition(PlayerMovementStateObject state)
    {
        return state.Input.MovementBuffer.Last().X != 0.0;
    }

    private bool FallTransition(PlayerMovementStateObject state)
    {
        return !state.IsOnFloor;
    }
}
