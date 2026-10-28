
using Godot;

public static class PlayerProperties
{
    public readonly static StateHandle[] ValidCoyoteStates = [StateHandle.MOVE, StateHandle.IDLE];
    public readonly static Vector2 GRABBOX_OFFSET = new(-7.0f, -5f);
    public readonly static Vector2 HELD_ITEM_OFFSET = new(0.0f, -30.0f);
    public const float SPEED = 250.0f;
    public const float AIRDRIFT = 250.0f;
    public const float DRAG = 1000.0f;
    public const float AIR_DRAG = 50.0f;
    public const float JUMP_VELOCITY = 300.0f;
    public const float WALLJUMP_VELOCITY = 150.0f;
    public const float WALLJUMP_BOOST = 500.0f;
    public const double WALLJUMP_LOCKOUT = 0.25;
    public const float WALLSLIDE_GRAVITY_MODIFIER = 0.25f;
    public const double HANG_TIME = 0.2;
    public const float GRAVITY = 980.0f;
    public const float FRICTION = 1000.0f;
    public const float SLAM_SPEED = 800.0f;
    public const float SLIDE_SPEED = 400.0f;
    public const float ACCEL_TIME = 0.6f;
    public const float ACCEL_CURVE = 0.4f;
    public const float INITIAL_SPEED = 0.3f;
    public const float MASS = 45.0f;
    public const float VELOCITY_CAP = 800.0f;
    public const double COYOTE_TIME = 0.1;
    public const double GRAB_DURATION = 0.3;
    public const double THROW_DURATION = 0.3;
}
