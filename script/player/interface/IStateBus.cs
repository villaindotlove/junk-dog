public interface IStateBus
{
    void EnterState(StateHandle state);
    void ExitState(StateHandle state);
}
