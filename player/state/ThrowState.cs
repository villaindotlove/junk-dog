using System.Collections.Generic;

public class ThrowState : IPlayerState
{
    public string Name => throw new System.NotImplementedException();

    public void Enter(PlayerState state)
    {
        throw new System.NotImplementedException();
    }

    public void Exit(PlayerState state)
    {
        throw new System.NotImplementedException();
    }

    public List<PlayerStateTransition> GetTransitions()
    {
        throw new System.NotImplementedException();
    }

    public void Update(PlayerState state, double delta)
    {
        throw new System.NotImplementedException();
    }
}