using System;

public class PlayerStateTransition
{
    public string BeginStateName { get; init; }
    public string EndStateName { get; init; }
    public Func<PlayerState, bool> TransitionFunction { get; init; }
    public int Priority { get; init; }

    public PlayerStateTransition(
        string beginStateName,
        string endStateName,
        Func<PlayerState, bool> transitionFunction,
        int priority)
    {
        BeginStateName = beginStateName;
        EndStateName = endStateName;
        TransitionFunction = transitionFunction;
        Priority = priority;
    }
}