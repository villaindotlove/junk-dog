using System.Collections.Generic;
using Godot;

public class WallSlideState : IBehaviourState<PlayerMovementStateObject>
{
    public StateHandle Handle => global::StateHandle.WALLSLIDE;
    public List<StateTransition<PlayerMovementStateObject>> Transitions { get; }

    public WallSlideState()
    {
        Transitions = [
            new (Handle, global::StateHandle.IDLE, IdleTransition, 2),
            new (Handle, global::StateHandle.WALLJUMP, WalljumpTransition, 1),
            new (Handle, global::StateHandle.FALL, FallTransition, 0),
        ];
    }


    public void Enter(PlayerMovementStateObject state, IBehaviourState<PlayerMovementStateObject> previousState)
    {
    }

    public void Update(PlayerMovementStateObject state, double delta)
    {
    }

    public void Exit(PlayerMovementStateObject state)
    {
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
