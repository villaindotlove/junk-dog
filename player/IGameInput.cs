using Godot;

public interface IGameInput
{
    bool IsActionPressed(string actionName);

    bool IsActionJustPressed(string actionName);

    float GetAxis(string positiveAction, string negativeAction);
}