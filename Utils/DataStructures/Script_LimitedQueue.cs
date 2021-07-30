using System.Collections.Generic;

public class Script_LimitedQueue<T> : Queue<T>
{
    public int Limit { get; set; }

    public Script_LimitedQueue(int limit) : base(limit)
    {
        Limit = limit;
    }

    public new void Enqueue(T item)
    {
        while (base.Count >= Limit)
            return;

        base.Enqueue(item);
    }
}