using Godot;

public class PlayerState
{
    public PlayerInput Input { get; init; }
    public Vector2 Velocity { get; set; }
    public bool HasItem { get; set; }
    public bool IsOnFloor { get; set; }

    public PlayerState(PlayerInput input)
    {
        Input = input;
        Velocity = Vector2.Zero;
        HasItem = false;
        IsOnFloor = false;
    }
}