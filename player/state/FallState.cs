using System.Collections.Generic;
using Godot;

public class FallState : IPlayerState
{
    private const double COYOTE_TIME = 0.2;
    public string Name => StateHandle.FALL;
    private double _timeInState = 0.0;

    public void Enter(PlayerState state)
    {
        _timeInState = 0.0;
    }

    public void Exit(PlayerState state)
    {
    }

    public List<PlayerStateTransition> GetTransitions()
    {
        return [
            new (Name, StateHandle.IDLE, IdleTransition, priority: 5),
        ];
    }

    public void Update(PlayerState state, double delta)
    {
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
        state.Velocity += new Vector2(0, (float)(PlayerStats.GRAVITY * delta));
    }

    private bool IdleTransition(PlayerState state)
    {
        return state.IsOnFloor;
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