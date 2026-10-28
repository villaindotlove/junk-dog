using System;

public class StateTransition<T>
{
    public StateHandle BeginState { get; init; }
    public StateHandle EndState { get; init; }
    public Func<T, bool> TransitionFunction { get; init; }
    public int Priority { get; init; }

    public StateTransition(
        StateHandle beginState,
        StateHandle endState,
        Func<T, bool> transitionFunction,
        int priority)
    {
        BeginState = beginState;
        EndState = endState;
        TransitionFunction = transitionFunction;
        Priority = priority;
    }
}
