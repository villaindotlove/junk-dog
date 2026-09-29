

using System.Collections.Generic;

public interface IBehaviourState<T>
{
    string Name { get; }
    void Update(T stateObject, double delta);
    void Enter(T stateObject, IBehaviourState<T> previousState);
    void Exit(T stateObject);
    List<StateTransition<T>> GetTransitions();
}