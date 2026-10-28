public enum StateHandle
{
    IDLE,
    JUMP,
    FALL,
    MOVE,
    WALLSLIDE,
    WALLJUMP,
    NO_ACTION,
    GRAB,
    HOLD,
    THROW,
    PUNCH,
    LOCK
}

public static class StateHandleExtensions
{
    const string IDLE = "IDLE";
    const string JUMP = "JUMP";
    const string FALL = "FALL";
    const string MOVE = "MOVE";
    const string WALLSLIDE = "WALLSLIDE";
    const string WALLJUMP = "WALLJUMP";
    const string NO_ACTION = "NO_ACTION";
    const string GRAB = "GRAB";
    const string HOLD = "HOLD";
    const string THROW = "THROW";
    const string PUNCH = "PUNCH";
    const string LOCK = "LOCK";

    static string ToString(this StateHandle handle)
    {
	switch(handle)
	{
	    case StateHandle.IDLE:
		return IDLE;
	    case StateHandle.JUMP:
		return JUMP;
	    case StateHandle.FALL:
		return FALL;
	    case StateHandle.MOVE:
		return MOVE;
	    case StateHandle.WALLSLIDE:
		return WALLSLIDE;
	    case StateHandle.WALLJUMP:
		return WALLJUMP;
	    case StateHandle.NO_ACTION:
		return NO_ACTION;
	    case StateHandle.GRAB:
		return GRAB;
	    case StateHandle.HOLD:
		return HOLD;
	    case StateHandle.THROW:
		return THROW;
	    case StateHandle.PUNCH:
		return PUNCH;
	    case StateHandle.LOCK:
		return LOCK;
	    default: throw new System.Exception();
	}
    }

    public static StateHandle StateHandleFromString(string str)
    {
	switch(str)
	{
	    case IDLE:
		return StateHandle.IDLE;
	    case JUMP:
		return StateHandle.JUMP;
	    case FALL:
		return StateHandle.FALL;
	    case MOVE:
		return StateHandle.MOVE;
	    case WALLSLIDE:
		return StateHandle.WALLSLIDE;
	    case WALLJUMP:
		return StateHandle.WALLJUMP;
	    case NO_ACTION:
		return StateHandle.NO_ACTION;
	    case GRAB:
		return StateHandle.GRAB;
	    case HOLD:
		return StateHandle.HOLD;
	    case THROW:
		return StateHandle.THROW;
	    case PUNCH:
		return StateHandle.PUNCH;
	    case LOCK:
		return StateHandle.LOCK;
	    default:
		throw new System.Exception();
	}
    }
}
