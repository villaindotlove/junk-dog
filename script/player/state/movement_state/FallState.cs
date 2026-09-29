using System.Collections.Generic;
using System.Linq;
using Godot;

public class FallState : IBehaviourState<PlayerMovementStateObject>
{
    public string Name => StateHandle.FALL;

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
            new (Name, StateHandle.JUMP, JumpTransition, priority: 5),
            new (Name, StateHandle.IDLE, IdleTransition, priority: 4),
            new (Name, StateHandle.MOVE, MoveTransition, priority: 4),
            new (Name, StateHandle.WALLSLIDE, WallTransition, priority: 3),
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
            && PlayerProperties.ValidCoyoteStates.Contains(state.PreviousState.Name);
    }

    private bool WallTransition(PlayerMovementStateObject state)
    {
        return state.IsOnWall;
    }
}