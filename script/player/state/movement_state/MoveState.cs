using System.Collections.Generic;

public class MoveState : IBehaviourState<PlayerMovementStateObject>
{
    public string Name => StateHandle.MOVE;

    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
    }

    public void Exit(PlayerMovementStateObject state)
    {
    }

    public void Update(PlayerMovementStateObject stateObject, double delta)
    {
    }

    public List<StateTransition<PlayerMovementStateObject>> GetTransitions()
    {
        return [
            new (Name, StateHandle.FALL, FallTransition, 5),
            new (Name, StateHandle.IDLE, IdleTransition, 1),
            new (Name, StateHandle.JUMP, JumpTransition, 0),
        ];
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