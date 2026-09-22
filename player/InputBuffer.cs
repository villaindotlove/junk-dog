using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class InputBuffer<T>
{
    private Queue<T> _values;
    private uint _size;
    
    public InputBuffer(uint size)
    {
        Debug.Assert(size > 0);
        _values = new();
        _size = size;
    }

    public bool Contains(T val)
    {
        return _values.Contains(val);
    }

    public void Add(T val)
    {
        if(_values.Count == _size)
        {
            _values.Dequeue();
        }
        _values.Enqueue(val);
    }

    public T Last()
    {
        return _values.Last();
    }
}