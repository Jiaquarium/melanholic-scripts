using System.Collections.Generic;

// Queue that will stop Enqueueing once the limit is reached.
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