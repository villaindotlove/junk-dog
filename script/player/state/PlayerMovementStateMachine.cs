public class PlayerMovementStateMachine : StateMachine<PlayerMovementStateObject>
{
    public PlayerMovementStateMachine()
    {
        OriginState = new IdleState();

        AddState(StateHandle.IDLE, OriginState);
        AddState(StateHandle.JUMP, new JumpState());
        AddState(StateHandle.FALL, new FallState());
        AddState(StateHandle.MOVE, new MoveState());
        AddState(StateHandle.WALLSLIDE, new WallSlideState());
        AddState(StateHandle.WALLJUMP, new WalljumpState());
    }
}
