using System.Collections.Generic;

public interface IPlayerState
{
    string Name { get; }

    void Update(PlayerState state, double delta);

    void Enter(PlayerState state);

    void Exit(PlayerState state);

    List<PlayerStateTransition> GetTransitions();
}