using System.Collections.Generic;
using Godot;

public class IdleState : IPlayerState
{
    public string Name => StateHandle.IDLE;
    private double _timeInState = 0.0;

    public void Enter(PlayerState state)
    {
        _timeInState = 0.0;
        state.Velocity = new (state.Velocity.X, 0.0f);
    }

    public void Exit(PlayerState state)
    {
    }

    public void Update(PlayerState state, double delta)
    {
        _timeInState += delta;
        ApplyDrag(state, delta);
    }

    public List<PlayerStateTransition> GetTransitions()
    {
        return [
            new (Name, StateHandle.JUMP, JumpTransition, priority: 5),
            new (Name, StateHandle.MOVE, MoveTransition, priority: 1),
            new (Name, StateHandle.FALL, FallTransition, priority: 0),
        ];
    }
    
    private void ApplyDrag(PlayerState state, double delta)
    {
        var preX = state.Velocity.X;
        var postX = preX - Mathf.Sign(state.Velocity.X) * PlayerStats.DRAG * delta;
        if(Mathf.Sign(preX) != Mathf.Sign(postX))
        {
            state.Velocity = new (0.0f, state.Velocity.Y);
        }
        else
        {
            state.Velocity = new ((float)postX, state.Velocity.Y);
        }
    }

    private bool JumpTransition(PlayerState state)
    {
        return state.Input.JumpActuationBuffer.Contains(true);
    }

    private bool GrabTransition(PlayerState state)
    {
        return (state.Input.GrabBuffer.Contains(true) && !state.HasItem);
    }

    private bool ThrowTransition(PlayerState state)
    {
        return (state.Input.GrabBuffer.Contains(true) && state.HasItem);
    }

    private bool MoveTransition(PlayerState state)
    {
        return state.Input.MovementBuffer.Last().X != 0.0;
    }

    private bool FallTransition(PlayerState state)
    {
        return !state.IsOnFloor;
    }
}