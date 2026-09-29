using Godot;

public partial class ThrowableItem : RigidBody2D, IThrowable
{
    public bool CanBeThrown { get; private set; } = true;
    private Vector2 _initialPos; 
    private Vector2 _initialRotation; 
    private Vector2 _initialVelocity; 
    private Vector2 _angularVelocity; 

    public override void _Ready()
    {

    }

    public void AttachPlayer()
    {
        CanBeThrown = false;
        SetDeferred(RigidBody2D.PropertyName.Freeze, true);
        SetDeferred(RigidBody2D.PropertyName.Rotation, 0);
        SetDeferred(RigidBody2D.PropertyName.Position, 0);
        SetDeferred(RigidBody2D.MethodName.SetProcess, false);
    }

    public void DetachPlayer()
    {
        CanBeThrown = true;
        Freeze = false;
        SetProcess(true);
    }

    public void Reset()
    {
        Freeze = false;
        Sleeping = false;
    }
}