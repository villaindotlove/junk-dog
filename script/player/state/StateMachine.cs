
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;

public abstract class StateMachine<T> where T : IStateObject<T>
{
    public IBehaviourState<T> OriginState { get; init; }
    protected Dictionary<string, IBehaviourState<T>> _states = [];
    protected Dictionary<IBehaviourState<T>, List<StateTransition<T>>> _transitions = [];
    private int _frame = 0;

    public void AddState(string stateHandle, IBehaviourState<T> state)
    {
        _states[stateHandle] = state;
        _transitions[state] = state.GetTransitions();
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
                outState = _states[transition.EndStateName];
            }
        }

        if(outState != null)
        {
            GD.Print($"[{_frame}][SM:{typeof(T)}] {stateObject.CurrentState.Name} -> {outState.Name}");
            stateObject.CurrentState.Exit(stateObject);
            stateObject.PreviousState = stateObject.PreviousState;
            outState.Enter(stateObject, stateObject.PreviousState);
            stateObject.CurrentState = outState;
            stateObject.CurrentStateDuration = 0.0;
        }
    }
}