using System.Collections.Generic;

public class WalljumpState : IBehaviourState<PlayerMovementStateObject>
{
    public StateHandle Handle => StateHandle.WALLJUMP;
    public List<StateTransition<PlayerMovementStateObject>> Transitions { get; }

    public WalljumpState()
    {
        Transitions = [
            new(Handle, StateHandle.FALL, FallTransition, 4),
            new(Handle, StateHandle.WALLSLIDE, WallslideTransition, 5),
        ];
    }

    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
        state.Jumped = true;
        state.Hang = true;
    }

    public void Exit(PlayerMovementStateObject state)
    {
        state.Hang = false;
    }

    public void Update(PlayerMovementStateObject state, double delta)
    {
        if(!state.Input.JumpBuffer.Last() || state.CurrentStateDuration > PlayerProperties.HANG_TIME)
        {
            state.Hang = false;
        }
    }

    private bool FallTransition(PlayerMovementStateObject state)
    {
        return !state.Hang;
    }

    private bool WallslideTransition(PlayerMovementStateObject state)
    {
        return state.IsOnWall;
    }
}
