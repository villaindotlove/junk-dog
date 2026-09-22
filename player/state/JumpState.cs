using System.Collections.Generic;
using Godot;

public class JumpState : IPlayerState
{
    public string Name => StateHandle.JUMP;
    private bool _lastFrameJumped = false;
    private double _timeInState = 0.0;

    public void Enter(PlayerState state)
    {
        state.Velocity = new Vector2(state.Velocity.X, -PlayerStats.JUMP_VELOCITY);
        _lastFrameJumped = true;
        _timeInState = 0.0;
    }

    public void Exit(PlayerState state)
    {
    }

    public List<PlayerStateTransition> GetTransitions()
    {
        return [
            new(Name, StateHandle.FALL, FallTransition, 5),
        ];
    }

    public void Update(PlayerState state, double delta)
    {
        _timeInState += delta;
        double newX;
        if (state.Input.MovementBuffer.Last().X == 0.0)
        {
            newX = ProcessAirDrag(state.Velocity.X, delta);
        }
        else
        {
            newX = ProcessAirMovement(state, delta);
        }

        state.Velocity = new ((float)newX, state.Velocity.Y);
        if(!_lastFrameJumped)
        {
            state.Velocity += new Vector2(0, (float)(PlayerStats.GRAVITY * delta));
        }
        else if(!state.Input.JumpBuffer.Last() || _timeInState > PlayerStats.HANG_TIME)
        {
            _lastFrameJumped = false;
        }
    }

    private bool FallTransition(PlayerState state)
    {
        return state.Velocity.Y > 0;
    }

    private double ProcessAirMovement(PlayerState state, double delta)
    {
        var fasterThanCap = state.Velocity.X > PlayerStats.AIRDRIFT || state.Velocity.X < -PlayerStats.AIRDRIFT;
        if(fasterThanCap)
        {
            return ProcessAirDrag(state.Velocity.X, delta);
        }
        else
        {
            var newX = state.Velocity.X;
            newX += (float)(state.Input.MovementBuffer.Last().X * PlayerStats.AIRDRIFT * 10.0 * delta);
            return Mathf.Clamp(newX, -PlayerStats.AIRDRIFT, PlayerStats.AIRDRIFT);
        }
    }

    private double ProcessAirDrag(float currentVel, double delta)
    {
        var drag = Mathf.Sign(currentVel) * PlayerStats.AIR_DRAG * delta;
        var newX = currentVel - drag;
        if (Mathf.Sign(newX) != Mathf.Sign(currentVel))
        {
            newX = 0.0;
        }
        return newX;
    }
}