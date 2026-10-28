using Godot;

public partial class PlayerController : CharacterBody2D
{
    private PlayerInput _playerInput;
    private PlayerMovementStateObject _movementStateObject;
    private PlayerMovementStateMachine _movementStateMachine;
    private PlayerActionStateObject _actionStateObject;
    private PlayerActionStateMachine _actionStateMachine;
    private Node2D _heldItemSlot;
    private Area2D _grabHitbox;
    private CollisionShape2D _grabCollider;
    private Label _movementStateLabel;
    private Label _actionStateLabel;
    private PlayerAnimationController _playerAnimationController;
    private PlayerEventBus _eventBus;

    public override void _Ready()
    {
	LinkEventBus();
        _playerInput = new PlayerInput(new PhysicalGameInput());
        _movementStateMachine = new PlayerMovementStateMachine();
	_movementStateMachine.LinkStateBus(Global.PlayerEventBus(this));
        _movementStateObject = new PlayerMovementStateObject(_playerInput)
        {
            CurrentState = _movementStateMachine.OriginState,
            PreviousState = _movementStateMachine.OriginState,
        };

        _actionStateMachine = new PlayerActionStateMachine();
	_actionStateMachine.LinkStateBus(Global.PlayerEventBus(this));
        _actionStateObject = new PlayerActionStateObject(_playerInput)
        {
            CurrentState = _actionStateMachine.OriginState,
            PreviousState = _actionStateMachine.OriginState,
        };

        _heldItemSlot = new Node2D
        {
            Name = "HeldItemSlot",
            Position = PlayerProperties.HELD_ITEM_OFFSET,
        };

        _movementStateLabel = new Label
        {
            Position = new Vector2(0, -25)
        };

        _actionStateLabel = new Label
        {
            Position = new Vector2(0, 0)
        };

        AddChild(_heldItemSlot);
        AddChild(_movementStateLabel);
        AddChild(_actionStateLabel);
        AddChild(CreateGrabHitbox());
        DisableGrabHitbox();
        _playerAnimationController = GetNode<PlayerAnimationController>("AnimationController");
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateStateObject(delta);
        UpdateStateMachines(delta);
        ProcessActiveMovementState(delta);
        ProcessActiveActionState(delta);
        ProcessFlip(_movementStateObject.Velocity.X);
        Velocity = _movementStateObject.Velocity;
        MoveAndSlide();
    }

    private void UpdateStateObject(double delta)
    {
        _playerInput.Update(delta);
        _movementStateObject.IsOnFloor = IsOnFloor();
        _movementStateObject.IsOnWall = IsOnWall();
        if(_movementStateObject.IsOnWall)
        {
            _movementStateObject.WallNormal = GetWallNormal();
        }
        else
        {
            _movementStateObject.WallNormal = null;
        }
    }

    private void UpdateStateMachines(double delta)
    {
        _movementStateMachine.Update(_movementStateObject, delta);
        _actionStateMachine.Update(_actionStateObject, delta);
        _movementStateLabel.Text = _movementStateObject.CurrentState.Handle.ToString();
        _actionStateLabel.Text = _actionStateObject.CurrentState.Handle.ToString();
    }

    private void OnStateEnter(string stateName)
    {
	var handle = StateHandleExtensions.StateHandleFromString(stateName);
	switch(handle)
	{
	    case StateHandle.GRAB:
		EnableGrabHitbox();
		break;
	    case StateHandle.THROW:
		ThrowHeldItem();
		_actionStateObject.HeldItem = null;
		_actionStateObject.HasItem = false;
		break;
	    default:
		break;
	}
    }

    private void OnStateExit(string stateName)
    {
	var handle = StateHandleExtensions.StateHandleFromString(stateName);
	switch(handle)
	{
	    case StateHandle.GRAB:
		DisableGrabHitbox();
		break;
	    default:
		break;
	}
    }

    private void ProcessActiveActionState(double delta)
    {
        switch (_actionStateObject.CurrentState.Handle)
        {
            default:
                break;
        }
    }

    private void ProcessActiveMovementState(double delta)
    {
        switch(_movementStateObject.CurrentState.Handle)
        {
            case StateHandle.MOVE:
                ProcessMovementState(delta);
                break;
            case StateHandle.IDLE:
                ProcessIdleState(delta);
                break;
            case StateHandle.FALL:
                ProcessFallState(delta);
                break;
            case StateHandle.JUMP:
                ProcessJumpState(delta);
                break;
            case StateHandle.WALLSLIDE:
                ProcessWallslideState(delta);
                break;
            case StateHandle.WALLJUMP:
                ProcessWalljumpState(delta);
                break;
            default:
                break;
        }
    }

    private void ProcessMovementState(double delta)
    {
        var xInput = _movementStateObject.Input.MovementBuffer.Last().X;
        if(xInput == 0.0)
        {
            _movementStateObject.Velocity = ApplyGroundDrag(_movementStateObject.Velocity, delta);
        }
        else
        {
            _movementStateObject.Velocity = ApplyGroundMovement(_movementStateObject.Velocity, xInput, _movementStateObject.CurrentStateDuration);
        }
    }

    private void ProcessIdleState(double delta)
    {
        _movementStateObject.Velocity = ApplyGroundDrag(_movementStateObject.Velocity, delta);
    }

    private void ProcessFallState(double delta)
    {
        _movementStateObject.Velocity = ApplyAirMovement(_movementStateObject.Velocity, delta);
        _movementStateObject.Velocity = ApplyGravity(_movementStateObject.Velocity, delta);
    }

    private void ProcessWallslideState(double delta)
    {
        _movementStateObject.Velocity = ApplyWallslide(_movementStateObject.Velocity, delta);
    }

    private void ProcessJumpState(double delta)
    {
        if(_movementStateObject.Jumped)
        {
            _movementStateObject.Velocity = ApplyJump(_movementStateObject.Velocity);
            _movementStateObject.Jumped = false;
        }

        _movementStateObject.Velocity = ApplyAirMovement(_movementStateObject.Velocity, delta);
    }

    private void ProcessWalljumpState(double delta)
    {
        if(_movementStateObject.Jumped)
        {
            _movementStateObject.Velocity = ApplyWalljump(_movementStateObject.Velocity);
            _movementStateObject.Jumped = false;
        }

        if(_movementStateObject.CurrentStateDuration >= PlayerProperties.WALLJUMP_LOCKOUT)
        {
            _movementStateObject.Velocity = ApplyAirMovement(_movementStateObject.Velocity, delta);
        }
    }

    private Vector2 ApplyGroundMovement(Vector2 velocity, float xInput, double timeInState)
    {
        var preX = velocity.X;
        var step = xInput * PlayerProperties.SPEED;
        var easing = Mathf.Ease(PlayerProperties.INITIAL_SPEED + timeInState / PlayerProperties.ACCEL_TIME, PlayerProperties.ACCEL_CURVE);
        var easedStep = step * easing;

        if (Mathf.Sign(preX) == Mathf.Sign(xInput))
        {
            var sign = Mathf.Sign(preX);
            easedStep = sign * Mathf.Max(sign * preX, sign * easedStep);
            velocity = new ((float)easedStep, _movementStateObject.Velocity.Y);
        }

        return new((float)easedStep, velocity.Y);
    }

    private Vector2 ApplyGroundDrag(Vector2 velocity, double delta)
    {
        var preX = velocity.X;
        var postX = preX - Mathf.Sign(velocity.X) * PlayerProperties.DRAG * delta;
        if(Mathf.Sign(preX) != Mathf.Sign(postX))
        {
            return new (0.0f, velocity.Y);
        }
        else
        {
            return new ((float)postX, velocity.Y);
        }
    }

    private Vector2 ApplyAirMovement(Vector2 velocity, double delta)
    {
        if(_movementStateObject.Input.MovementBuffer.Last().X == 0)
        {
            return ApplyAirDrag(velocity, delta);
        }
        else
        {
            return ApplyAirDrift(velocity, delta);
        }
    }

    private Vector2 ApplyAirDrift(Vector2 velocity, double delta)
    {
        var fasterThanCap = velocity.X > PlayerProperties.AIRDRIFT || velocity.X < -PlayerProperties.AIRDRIFT;
        if(fasterThanCap)
        {
            return ApplyAirDrag(velocity, delta);
        }
        else
        {
            var newX = velocity.X;
            newX += (float)(_movementStateObject.Input.MovementBuffer.Last().X * PlayerProperties.AIRDRIFT * 10.0 * delta);
            return new (Mathf.Clamp(newX, -PlayerProperties.AIRDRIFT, PlayerProperties.AIRDRIFT), velocity.Y);
        }
    }

    private Vector2 ApplyAirDrag(Vector2 velocity, double delta)
    {
        var drag = Mathf.Sign(velocity.X) * PlayerProperties.AIR_DRAG * delta;
        var newX = velocity.X - drag;
        if (Mathf.Sign(newX) != Mathf.Sign(velocity.X))
        {
            newX = 0.0;
        }
        return new ((float)newX, velocity.Y);
    }

    private Vector2 ApplyGravity(Vector2 velocity, double delta, float gravityModifier = 1.0f)
    {
        return velocity + new Vector2(0, (float)(gravityModifier * _movementStateObject.Gravity * delta));
    }

    private Vector2 ApplyJump(Vector2 velocity)
    {
        return new(velocity.X, -PlayerProperties.JUMP_VELOCITY);
    }

    private Vector2 ApplyWallslide(Vector2 velocity, double delta)
    {
        if(velocity.Y < 0)
        {
            return ApplyGravity(velocity, delta);
        }

        return ApplyGravity(velocity, delta, PlayerProperties.WALLSLIDE_GRAVITY_MODIFIER);
    }

    private Vector2 ApplyWalljump(Vector2 velocity)
    {
        var newX = velocity.X;
        newX += Mathf.Sign(_movementStateObject.WallNormal.Value.X) * PlayerProperties.WALLJUMP_BOOST;
        var newY = Mathf.Min(velocity.Y, 0) - PlayerProperties.WALLJUMP_VELOCITY;
        return new(newX, newY);
    }

    private Area2D CreateGrabHitbox()
    {
        _grabHitbox = new Area2D
        {
            Position = PlayerProperties.GRABBOX_OFFSET,
        };
        _grabCollider = new CollisionShape2D
        {
            Shape = (Shape2D)ResourceLoader.Load("res://script/player/res/grab_hitbox.tres")
        };

        _grabHitbox.AddChild(_grabCollider);

        _grabHitbox.BodyEntered += OnBodyEntered;
        return _grabHitbox;
    }

    private void OnBodyEntered(Node2D other)
    {
        if(other is IThrowable)
        {
            var item = other as IThrowable;
            if(item.CanBeThrown)
            {
                PickupItem(other);
            }
        }
    }

    private void PickupItem(Node2D item)
    {
        var throwableItem = item as IThrowable;
        _actionStateObject.HasItem = true;
        _actionStateObject.HeldItem = throwableItem;
        item.CallDeferred(Node.MethodName.Reparent, _heldItemSlot);
        throwableItem.AttachPlayer();
    }

    private void ThrowHeldItem()
    {
        var thrownItem = _actionStateObject.HeldItem;
        thrownItem.DetachPlayer();
        var item = thrownItem as RigidBody2D;
        item.Reparent(GetTree().Root);
        Vector2 throwForce;
        var lastInput = _movementStateObject.Input.MovementBuffer.Last();
        if(lastInput == Vector2.Zero)
        {
            throwForce = new Vector2(_movementStateObject.Facing, 0.0f) * 40_000.0f;
        }
        else
        {
            throwForce = lastInput.Normalized() * 40_000.0f;
        }
        if(lastInput.Y == 0.0f)
        {
            throwForce += new Vector2(0.0f, -10_000.0f);
        }

        item.ApplyForce(throwForce);
    }

    private void EnableGrabHitbox()
    {
        _grabHitbox.Monitoring = true;
        _grabHitbox.Monitorable = true;
        _actionStateObject.GrabBoxActive = true;
        _grabCollider.DebugColor = Color.Color8(128, 0, 128, 128);
    }

    private void DisableGrabHitbox()
    {
        _grabHitbox.Monitoring = false;
        _grabHitbox.Monitorable = false;
        _actionStateObject.GrabBoxActive = false;
        _grabCollider.DebugColor = Color.Color8(0, 128, 128, 128);
    }

    private void ProcessFlip(float vx)
    {
        var pFacing = _movementStateObject.Facing;
        if(vx != 0)
        {
            _movementStateObject.Facing = Mathf.Sign(vx);
        }
        if(pFacing != _movementStateObject.Facing)
        {
            _grabHitbox.Position *= new Vector2(-1, 1);
            _playerAnimationController.Flip();
        }
    }

    private void LinkEventBus()
    {
	_eventBus = Global.PlayerEventBus(this);
	_eventBus.StateEntered += OnStateEnter;
	_eventBus.StateExit += OnStateExit;
    }
}
