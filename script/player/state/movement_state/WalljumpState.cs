
using System.Collections.Generic;

public class WalljumpState : IBehaviourState<PlayerMovementStateObject>
{
    public string Name => StateHandle.WALLJUMP;

    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
        state.Jumped = true;
        state.Hang = true;
    }

    public void Exit(PlayerMovementStateObject state)
    {
        state.Hang = false;
    }

    public List<StateTransition<PlayerMovementStateObject>> GetTransitions()
    {
        return [
            new(Name, StateHandle.FALL, FallTransition, 4),
            new(Name, StateHandle.WALLSLIDE, WallslideTransition, 5),
        ];
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