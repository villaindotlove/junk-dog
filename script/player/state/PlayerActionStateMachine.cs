
public class PlayerActionStateMachine : StateMachine<PlayerActionStateObject>
{
    public PlayerActionStateMachine()
    {
        OriginState = new NoActionState();

        AddState(StateHandle.NO_ACTION, OriginState);
        AddState(StateHandle.GRAB, new GrabState());
        AddState(StateHandle.HOLD, new HoldState());
        AddState(StateHandle.THROW, new ThrowState());
    }
}