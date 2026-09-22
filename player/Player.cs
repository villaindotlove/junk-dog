using Godot;

public partial class Player : CharacterBody2D
{
    private PlayerInput _playerInput;
    private PlayerState _playerState;
    private PlayerStateMachine _stateMachine;
    private Label _stateLabel;

    public override void _Ready()
    {
        _playerInput = new PlayerInput(new PhysicalGameInput());
        _playerState = new PlayerState(_playerInput);
        _stateMachine = new PlayerStateMachine();
        _stateLabel = new Label();
        AddChild(_stateLabel);
    }

    public override void _PhysicsProcess(double delta)
    {
        Update(delta);
        Velocity = _playerState.Velocity;
        MoveAndSlide();
        base._PhysicsProcess(delta);
    }

    public void Update(double delta)
    {
        _playerInput.Update(delta);
        _playerState.IsOnFloor = IsOnFloor();
        _stateMachine.Update(_playerState, delta);
        _stateLabel.Text = _stateMachine.CurrentState.Name;
    }

    public IPlayerState GetCurrentState()
    {
        return _stateMachine.CurrentState;
    }
}