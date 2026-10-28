using System.Collections.Generic;
using System.Linq;
using Godot;

public abstract class StateMachine<T> where T : IStateObject<T>
{
    public IBehaviourState<T> OriginState { get; init; }
    protected Dictionary<StateHandle, IBehaviourState<T>> _states = [];
    protected Dictionary<IBehaviourState<T>, List<StateTransition<T>>> _transitions = [];
    protected IStateBus _eventBus;
    private int _frame = 0;

    public void LinkStateBus(IStateBus stateBus)
    {
	_eventBus = stateBus;
    }

    public void AddState(StateHandle stateHandle, IBehaviourState<T> state)
    {
        _states[stateHandle] = state;
        _transitions[state] = state.Transitions;
    }

    public void Update(T stateObject, double delta)
    {
        _frame+=1;
        stateObject.CurrentStateDuration += delta;
        MakeTransition(stateObject);
        stateObject.CurrentState.Update(stateObject, delta);
    }

    public void MakeTransition(T stateObject)
    {
        var transitions = _transitions[stateObject.CurrentState];

        IBehaviourState<T>? outState = null;

        foreach (var transition in transitions.OrderBy(t => t.Priority))
        {
            if(transition.TransitionFunction(stateObject))
            {
                outState = _states[transition.EndState];
            }
        }

        if(outState != null)
        {
            // GD.Print($"[{_frame}][SM:{typeof(T)}] {stateObject.CurrentState.Handle} -> {outState.Handle}");
            stateObject.CurrentState.Exit(stateObject);
	    GD.Print($"Exited state {stateObject.CurrentState.Handle}");
	    _eventBus.ExitState(stateObject.CurrentState.Handle);

            stateObject.PreviousState = stateObject.CurrentState;
            stateObject.CurrentState = outState;
            stateObject.CurrentState.Enter(stateObject, stateObject.PreviousState);
	    GD.Print($"Entered state {stateObject.CurrentState.Handle}");
	    _eventBus.EnterState(stateObject.CurrentState.Handle);

            stateObject.CurrentStateDuration = 0.0;
        }
    }
}
