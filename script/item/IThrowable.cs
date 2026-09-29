
using Godot;

public interface IThrowable
{
    Vector2 Position { get; set; }
    bool CanBeThrown { get; }
    void AttachPlayer();
    void DetachPlayer();
}