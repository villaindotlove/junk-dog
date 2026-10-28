using System.Collections.Generic;

public interface IBehaviourState<T>
{
    StateHandle Handle { get; }
    List<StateTransition<T>> Transitions { get; }
    void Update(T stateObject, double delta);
    void Enter(T stateObject, IBehaviourState<T> previousState);
    void Exit(T stateObject);
}
