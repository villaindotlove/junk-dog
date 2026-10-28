using Godot;

public static class Global
{
    public static PlayerEventBus PlayerEventBus(Node self) => self.GetNode<PlayerEventBus>($"/root/PlayerEventBus");
}
