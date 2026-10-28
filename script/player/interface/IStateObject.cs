public interface IStateObject<T> 
{
    double CurrentStateDuration { get; set; }
    IBehaviourState<T> CurrentState { get; set;}
    IBehaviourState<T> PreviousState { get; set;}
    string ToString();
}
