using System.Collections.Generic;
using Godot;

public class IdleState : IBehaviourState<PlayerMovementStateObject>
{
    public string Name => StateHandle.IDLE;

    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
    }

    public void Exit(PlayerMovementStateObject state)
    {
    }

    public void Update(PlayerMovementStateObject state, double delta)
    {
    }

    public List<StateTransition<PlayerMovementStateObject>> GetTransitions()
    {
        return [
            new (Name, StateHandle.JUMP, JumpTransition, priority: 5),
            new (Name, StateHandle.MOVE, MoveTransition, priority: 1),
            new (Name, StateHandle.FALL, FallTransition, priority: 0),
        ];
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