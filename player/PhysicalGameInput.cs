using Godot;

public class PhysicalGameInput : IGameInput
{
    public float GetAxis(string positiveAction, string negativeAction)
    {
        return Input.GetAxis(positiveAction, negativeAction);
    }

    public bool IsActionJustPressed(string actionName)
    {
        return Input.IsActionJustPressed(actionName);
    }

    public bool IsActionPressed(string actionName)
    {
        return Input.IsActionPressed(actionName);
    }
}