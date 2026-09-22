using System;
using System.Collections.Generic;

public class ProgrammableGameInput : IGameInput
{
    private Dictionary<string, Queue<bool>> _boolActions = [];
    private Dictionary<string, Queue<float>> _floatActions = [];

    public void Encode(
        Queue<float> x,
        Queue<float> y,
        Queue<bool> jump,
        Queue<bool> grab)
    {
        _floatActions[ActionName.LEFT] = [];
        foreach (var val in x)
        {
            _floatActions[ActionName.LEFT].Enqueue(val);
        }

        _floatActions[ActionName.UP] = [];
        foreach (var val in y)
        {
            _floatActions[ActionName.UP].Enqueue(val);
        }

        _boolActions[ActionName.JUMP] = [];
        foreach (var val in jump)
        {
            _boolActions[ActionName.JUMP].Enqueue(val);
        }

        _boolActions[ActionName.GRAB] = [];
        foreach (var val in grab)
        {
            _boolActions[ActionName.GRAB].Enqueue(val);
        }
    }

    public float GetAxis(string positiveAction, string negativeAction)
    {
        return _floatActions[positiveAction].Dequeue();
    }

    public bool IsActionJustPressed(string actionName)
    {
        return _boolActions[actionName].Dequeue();
    }

    public bool IsActionPressed(string actionName)
    {
        return _boolActions[actionName].Dequeue();
    }
}