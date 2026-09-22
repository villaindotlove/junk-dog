using System.Collections.Generic;
using System.Linq;
using Godot;

public class PlayerStateMachine
{
    public IPlayerState CurrentState { get; private set; }

    private Dictionary<string, IPlayerState> _states = [];
    private Dictionary<IPlayerState, List<PlayerStateTransition>> _transitions = [];

    public PlayerStateMachine()
    {
        AddState(StateHandle.IDLE, new IdleState());
        AddState(StateHandle.JUMP, new JumpState());
        AddState(StateHandle.FALL, new FallState());
        AddState(StateHandle.MOVE, new MoveState());

        CurrentState = _states[StateHandle.IDLE];
    }

    public void AddState(string stateHandle, IPlayerState state)
    {
        _states[stateHandle] = state;
        _transitions[state] = state.GetTransitions();
    }

    public void Update(PlayerState playerState, double delta)
    {
        CurrentState.Update(playerState, delta);
        MakeTransition(playerState);
    }

    public void MakeTransition(PlayerState playerState)
    {
        var transitions = _transitions[CurrentState];

        IPlayerState? outState = null;

        foreach (var transition in transitions.OrderBy(t => t.Priority))
        {
            if(transition.TransitionFunction(playerState))
            {
                outState = _states[transition.EndStateName];
            }
        }

        if(outState != null)
        {
            CurrentState.Exit(playerState);
            CurrentState = outState;
            outState.Enter(playerState);
        }
    }
}