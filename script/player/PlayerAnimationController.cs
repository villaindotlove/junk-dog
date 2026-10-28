using Godot;

public partial class PlayerAnimationController : Node2D
{
    private AnimationPlayer _upperBodyPlayer;
    private AnimationPlayer _lowerBodyPlayer;
    private Sprite2D _upperBodySprite;
    private Sprite2D _lowerBodySprite;
    private PlayerEventBus _eventBus;
    private StateHandle _currentMovementState;

    public override void _Ready()
    {
        _upperBodyPlayer = GetNode<AnimationPlayer>("UpperBodyPlayer");
        _lowerBodyPlayer = GetNode<AnimationPlayer>("LowerBodyPlayer");
        _upperBodySprite = GetNode<Sprite2D>("UpperBodySprite");
        _lowerBodySprite = GetNode<Sprite2D>("LowerBodySprite");

	LinkEventBus();
    }

    public void Flip()
    {
        _upperBodySprite.FlipH = !_upperBodySprite.FlipH;
        _lowerBodySprite.FlipH = !_lowerBodySprite.FlipH;
    }

    private void PlayAnimation(string animationName)
    {
	if(!IsPlayingAction(_upperBodyPlayer))
	{
	    _upperBodyPlayer.Play(animationName);
	}

	_lowerBodyPlayer.Play(animationName);
    }

    private void PlayAction(string actionName)
    {
	_upperBodyPlayer.Play(actionName);
	if(_lowerBodyPlayer.AssignedAnimation == AnimationName.IDLE
		&& _lowerBodyPlayer.HasAnimation(actionName))
	{
		_lowerBodyPlayer.Play(actionName);
	}
    }

    private void Resume()
    {
	StateChange(_currentMovementState.ToString());
    }

    private void StateChange(string newState)
    {
	var stateHandle = StateHandleExtensions.StateHandleFromString(newState);
	switch(stateHandle)
	{
	    case StateHandle.MOVE:
		_currentMovementState = stateHandle;
		PlayAnimation(AnimationName.WALK);
		break;
	    case StateHandle.IDLE:
		_currentMovementState = stateHandle;
		PlayAnimation(AnimationName.IDLE);
		break;
	    case StateHandle.JUMP:
	    case StateHandle.FALL:
	    case StateHandle.WALLSLIDE:
	    case StateHandle.WALLJUMP:
		_currentMovementState = stateHandle;
		PlayAnimation(AnimationName.JUMP);
		break;
	    case StateHandle.THROW:
		break;
	    case StateHandle.GRAB:
		PlayAction(AnimationName.GRAB);
		break;
	    case StateHandle.HOLD:
		break;
	    case StateHandle.NO_ACTION:
		Resume();
		break;
	    default:
		break;
	}
    }

    private void LinkEventBus()
    {
	_eventBus = Global.PlayerEventBus(this);
	_eventBus.StateEntered += StateChange;
    }

    private bool IsAction(string animationName)
    {
	foreach(var a in AnimationName.ACTIONS)
	{
	    if(string.Equals(a, animationName))
	    {
		return true;
	    }
	}

	return false;
    }

    private bool IsPlayingAction(AnimationPlayer player)
    {
	return IsAction(player.CurrentAnimation) && player.IsPlaying();
    }
}
