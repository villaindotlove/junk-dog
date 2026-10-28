using System.Reflection;

public class PlayerActionStateObject : IStateObject<PlayerActionStateObject>
{
    public PlayerInput Input { get; init; }
    public double CurrentStateDuration { get; set; }
    public IBehaviourState<PlayerActionStateObject> CurrentState { get; set; }
    public IBehaviourState<PlayerActionStateObject> PreviousState { get; set; }
    public bool HasItem { get; set; }
    public IThrowable? HeldItem { get; set; }
    public bool GrabBoxActive { get; set; }
    public bool ThrowHeldItem { get; set; }
    public ushort ComboCount { get; set; }

    public PlayerActionStateObject(PlayerInput input)
    {
        Input = input;
        HasItem = false;
        GrabBoxActive = false;
        ThrowHeldItem = false;
	ComboCount = 0;
    }


    public override string ToString()
    {
	var output = $"[PlayerActionStateObject]\n";
	foreach(var member in typeof(PlayerActionStateObject).GetMembers())
	{
	    if(member.MemberType == MemberTypes.Property)
	    {
		output += $"\t{member.Name}: {((PropertyInfo)member).GetValue(this)}\n";
	    }
	}
	return output;
    }
}
