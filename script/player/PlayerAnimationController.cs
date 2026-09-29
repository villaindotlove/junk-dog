using Godot;

public partial class PlayerAnimationController : Node2D
{
    private AnimationPlayer _upperBodyPlayer;
    private AnimationPlayer _lowerBodyPlayer;
    private Sprite2D _upperBodySprite;
    private Sprite2D _lowerBodySprite;

    public override void _Ready()
    {
        _upperBodyPlayer = GetNode<AnimationPlayer>("UpperBodyPlayer");
        _lowerBodyPlayer = GetNode<AnimationPlayer>("LowerBodyPlayer");
        _upperBodySprite = GetNode<Sprite2D>("UpperBodySprite");
        _lowerBodySprite = GetNode<Sprite2D>("LowerBodySprite");
    }

    public void PlayAnimation(string name)
    {
        Play(_upperBodyPlayer, name);
        Play(_lowerBodyPlayer, name);
    }

    public void PlayAction(string actionName)
    {
        _upperBodyPlayer.Play(actionName);
    }

    public void OverrideAnimation(string name)
    {
        _upperBodyPlayer.Play(name);
        _lowerBodyPlayer.Play(name);
    }

    public void UnlockAction()
    {
        var currentAnim = _lowerBodyPlayer.CurrentAnimation;
        _upperBodyPlayer.PlaySection(currentAnim, _lowerBodyPlayer.CurrentAnimationPosition);
    }

    public void Flip()
    {
        _upperBodySprite.FlipH = !_upperBodySprite.FlipH;
        _lowerBodySprite.FlipH = !_lowerBodySprite.FlipH;
    }

    private void Play(AnimationPlayer player, string name)
    {
        var currentAnim = player.AssignedAnimation;
        if(name != currentAnim)
        {
            if(player.HasAnimation(name))
            {
                player.Play(name);
            }
        }
    }
}