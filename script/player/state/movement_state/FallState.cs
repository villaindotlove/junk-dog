using System.Collections.Generic;
using System.Linq;

public class FallState : IBehaviourState<PlayerMovementStateObject>
{
    public StateHandle Handle => StateHandle.FALL;

    public List<StateTransition<PlayerMovementStateObject>> Transitions { get; }

    public FallState()
    {
        Transitions = [
            new (Handle, StateHandle.JUMP, JumpTransition, priority: 5),
            new (Handle, StateHandle.IDLE, IdleTransition, priority: 4),
            new (Handle, StateHandle.MOVE, MoveTransition, priority: 4),
            new (Handle, StateHandle.WALLSLIDE, WallTransition, priority: 3),
        ];
    }

    private bool IdleTransition(PlayerMovementStateObject state)
    {
        return state.IsOnFloor && (state.Input.MovementBuffer.Last().X == 0.0f);
    }

    private bool MoveTransition(PlayerMovementStateObject state)
    {
        return state.IsOnFloor && (state.Input.MovementBuffer.Last().X != 0.0f);
    }

    private bool JumpTransition(PlayerMovementStateObject state)
    {
        return 
            state.Input.JumpActuationBuffer.Contains(true)
            && state.CurrentStateDuration < PlayerProperties.COYOTE_TIME
            && PlayerProperties.ValidCoyoteStates.Contains(state.PreviousState.Handle);
    }

    private bool WallTransition(PlayerMovementStateObject state)
    {
        return state.IsOnWall;
    }

    public void Update(PlayerMovementStateObject stateObject, double delta)
    {
    }

    public void Enter(PlayerMovementStateObject stateObject, IBehaviourState<PlayerMovementStateObject> previousState)
    {
    }

    public void Exit(PlayerMovementStateObject stateObject)
    {
    }
}
