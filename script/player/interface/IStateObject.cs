public interface IStateObject<T> 
{
    public double CurrentStateDuration { get; set; }
    public IBehaviourState<T> CurrentState { get; set;}
    public IBehaviourState<T> PreviousState { get; set;}
}