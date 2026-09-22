using Godot;

public class PlayerInput
{
    public const int INPUT_BUFFER_SIZE = 10;
    public const int TARGET_FRAMERATE = 60;
    public const double FRAME_LENGTH = 1.0 / TARGET_FRAMERATE;

    public InputBuffer<bool> JumpBuffer { get; init; }
    public InputBuffer<bool> JumpActuationBuffer { get; init; }
    public InputBuffer<bool> GrabBuffer { get; init; }
    public InputBuffer<bool> GrabActuationBuffer { get; init; }
    public InputBuffer<Vector2> MovementBuffer { get; init; }
    private double _frameTimer;
    private IGameInput _input;

    public PlayerInput(IGameInput input)
    {
        _input = input;
        JumpBuffer = new InputBuffer<bool>(INPUT_BUFFER_SIZE);
        JumpActuationBuffer = new InputBuffer<bool>(INPUT_BUFFER_SIZE);
        GrabBuffer = new InputBuffer<bool>(INPUT_BUFFER_SIZE);
        GrabActuationBuffer = new InputBuffer<bool>(INPUT_BUFFER_SIZE);
        MovementBuffer = new InputBuffer<Vector2>(INPUT_BUFFER_SIZE);

        _frameTimer = 0.0;
    }

    public void Update(double delta)
    {
        _frameTimer += delta;
        if(_frameTimer >= FRAME_LENGTH)
        {
            _frameTimer -= FRAME_LENGTH;
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        JumpBuffer.Add(_input.IsActionPressed(ActionName.JUMP));
        JumpActuationBuffer.Add(_input.IsActionJustPressed(ActionName.JUMP));

        GrabBuffer.Add(_input.IsActionPressed(ActionName.GRAB));
        GrabActuationBuffer.Add(_input.IsActionJustPressed(ActionName.GRAB));

        MovementBuffer.Add(
            new(
                _input.GetAxis(ActionName.LEFT, ActionName.RIGHT),
                _input.GetAxis(ActionName.UP, ActionName.DOWN)
            ));
    }
}