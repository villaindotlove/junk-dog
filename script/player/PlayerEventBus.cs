using Godot;

public partial class PlayerEventBus : Node, IStateBus
{
    public void EnterState(StateHandle state)
    {
	EmitSignal(SignalName.StateEntered, state.ToString());
    }

    public void ExitState(StateHandle state)
    {
	EmitSignal(SignalName.StateExit, state.ToString());
    }

    [Signal]
    public delegate void StateEnteredEventHandler(string state);

    [Signal]
    public delegate void StateExitEventHandler(string state);
}
