using System.Collections.Generic;
using Godot;

public class MoveState : IPlayerState
{
    public string Name => StateHandle.MOVE;
    private double _timeInState = 0.0;

    public void Enter(PlayerState state)
    {
        _timeInState = 0.0;
        state.Velocity = new (state.Velocity.X, 0.0f);
    }

    public void Exit(PlayerState state)
    {
    }

    public List<PlayerStateTransition> GetTransitions()
    {
        return [
            new (Name, StateHandle.FALL, FallTransition, 5),
            new (Name, StateHandle.IDLE, IdleTransition, 1),
            new (Name, StateHandle.JUMP, JumpTransition, 0),
        ];
    }

    public void Update(PlayerState state, double delta)
    {
        _timeInState += delta;

        var xInput = state.Input.MovementBuffer.Last().X;
        if(xInput == 0.0)
        {
            ApplyDrag(state, delta);
        }
        else
        {
            ApplyMovement(state, xInput);
        }
    }

    private void ApplyMovement(PlayerState state, float xInput)
    {
        var preX = state.Velocity.X;
        var step = xInput * PlayerStats.SPEED;
        var easing = Mathf.Ease(PlayerStats.INITIAL_SPEED + _timeInState / PlayerStats.ACCEL_TIME, PlayerStats.ACCEL_CURVE);
        var easedStep = step * easing;

        if (Mathf.Sign(preX) == Mathf.Sign(xInput))
        {
            var sign = Mathf.Sign(preX);
            easedStep = sign * Mathf.Max(sign * preX, sign * easedStep);
            state.Velocity = new ((float)easedStep, state.Velocity.Y);
        }

        state.Velocity = new((float)easedStep, state.Velocity.Y);
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

    private bool IdleTransition(PlayerState state)
    {
        return state.Input.MovementBuffer.Last().X == 0.0;
    }

    private bool FallTransition(PlayerState state)
    {
        return !state.IsOnFloor;
    }

    private bool JumpTransition(PlayerState state)
    {
        return state.Input.JumpActuationBuffer.Contains(true);
    }
}