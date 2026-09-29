using System;

public class StateTransition<T>
{
    public string BeginStateName { get; init; }
    public string EndStateName { get; init; }
    public Func<T, bool> TransitionFunction { get; init; }
    public int Priority { get; init; }

    public StateTransition(
        string beginStateName,
        string endStateName,
        Func<T, bool> transitionFunction,
        int priority)
    {
        BeginStateName = beginStateName;
        EndStateName = endStateName;
        TransitionFunction = transitionFunction;
        Priority = priority;
    }
}