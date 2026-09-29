using Godot;

public class PlayerMovementStateObject : IStateObject<PlayerMovementStateObject>
{
    private const int RIGHT = 1;
    private const int LEFT = -1;
    public PlayerInput Input { get; init; }
    public Vector2 Velocity { get; set; }
    public bool IsOnFloor { get; set; }
    public bool IsOnWall { get; set; }
    public Vector2? WallNormal { get; set; }
    public float Gravity { get; set; }
    public bool Jumped { get; set; }
    public bool Hang { get; set; }
    public double CurrentStateDuration { get; set; }
    public IBehaviourState<PlayerMovementStateObject> CurrentState { get; set; }
    public IBehaviourState<PlayerMovementStateObject> PreviousState { get; set; }
    public int Facing { get; set; }

    public PlayerMovementStateObject(PlayerInput input)
    {
        Input = input;
        Velocity = Vector2.Zero;
        IsOnFloor = false;
        Gravity = PlayerProperties.GRAVITY;
        Facing = LEFT;
    }
}