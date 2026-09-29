using System.Collections.Generic;
using Godot;

public class WallSlideState : IBehaviourState<PlayerMovementStateObject>
{
    public string Name => StateHandle.WALLSLIDE;

    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
    }

    public void Update(PlayerMovementStateObject state, double delta)
    {
    }

    public void Exit(PlayerMovementStateObject state)
    {
    }

    public List<StateTransition<PlayerMovementStateObject>> GetTransitions()
    {
        return [
            new (Name, StateHandle.IDLE, IdleTransition, 2),
            new (Name, StateHandle.WALLJUMP, WalljumpTransition, 1),
            new (Name, StateHandle.FALL, FallTransition, 0),
        ];
    }

    private bool WalljumpTransition(PlayerMovementStateObject state)
    {
        if(state.WallNormal == null)
        {
            return false;
        }

        var validFramesForWalljumpInput = PlayerInput.SecondsToFrames(state.CurrentStateDuration);
        if (validFramesForWalljumpInput > 0)
        {
            validFramesForWalljumpInput += 1;
        }

        validFramesForWalljumpInput = Mathf.Min(validFramesForWalljumpInput, PlayerInput.INPUT_BUFFER_SIZE - 1);

        return 
            state.Input.JumpActuationBuffer.RangeContains(true, validFramesForWalljumpInput)
            && state.Input.JumpActuationBuffer.Contains(true);
    }

    private bool IdleTransition(PlayerMovementStateObject state)
    {
        return state.IsOnFloor;
    }

    private bool FallTransition(PlayerMovementStateObject state)
    {
        if(state.WallNormal == null)
        {
            return true;
        }

        var xInput = state.Input.MovementBuffer.Last().X;
        return 
            xInput != 0.0f
            && Mathf.Sign(xInput) == Mathf.Sign(state.WallNormal.Value.X);
    }
}