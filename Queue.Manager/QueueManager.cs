using System;
using System.Collections.Generic;

public class QueueManager<T> : IQueueManager<T> where T : CustomObject
{
    private Queue<T> queue;
    private IComparer<T> comparer;

    public QueueManager(IComparer<T> comparer)
    {
        this.queue = new Queue<T>();
        this.comparer = comparer;
    }

    public QueueManager() : this(Comparer<T>.Default) { }

    public void Enqueue(T item)
    {
        queue.Enqueue(item);
    }

    public T Dequeue()
    {
        if (queue.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        T[] items = queue.ToArray();
        Array.Sort(items, comparer);
        queue = new Queue<T>(items);
        return queue.Dequeue();
    }

    public T[] Sort()
    {
        if (queue.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        T[] items = queue.ToArray();
        Array.Sort(items, comparer);
        return items;
    }

    public void MoveUp(T item, int newPosition)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");

        T[] items = queue.ToArray();
        Array.Sort(items, comparer);

        // Increase position by 1 for items higher than the current position but lower or equal to the new position
        for (int i = newPosition - 1; i < item.QueuePosition - 1; i++)
        {
            ++items.ElementAt(i).QueuePosition;
        }

        // Update item with new position
        item.QueuePosition = newPosition;

        queue.Clear();

        foreach (var arrayItem in items)
        {
            queue.Enqueue(arrayItem);
        }
    }

    public void MoveDown(T item, int newPosition)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");

        T[] items = queue.ToArray();
        Array.Sort(items, comparer);

        // Decrease position by 1 for items lower than the current position and higher or equal to the new position
        for (int i = item.QueuePosition; i < newPosition; i++)
        {
            --items.ElementAt(i).QueuePosition;
        }

        // Update item with new position
        item.QueuePosition = newPosition;

        queue.Clear();

        foreach (var arrayItem in items)
        {
            queue.Enqueue(arrayItem);
        }
    }

    public T[] ReOrder(T item, int newPostion)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");
        if (!queue.Contains(item)) throw new InvalidOperationException("Item not found in the Queue");
        if (queue.Count < newPostion || newPostion < 1) throw new InvalidOperationException("New position is out of bounds");

        var direction = GetDirectionOfMove(item.QueuePosition, newPostion);
        switch (direction)
        {
            case 0:
                break;
            case 1:
                MoveUp(item, newPostion);
                break;
            case -1:
                MoveDown(item, newPostion);
                break;
            default:
                break;
        }

        T[] updatedQueuePositionItems = queue.ToArray();
        Array.Sort(updatedQueuePositionItems, comparer);
        return updatedQueuePositionItems;
    }

    public int Count => queue.Count;
    public T Current => queue.GetEnumerator().Current;

    /// <summary>
    /// Return the direction for the requested re-order
    /// </summary>
    /// <param name="currentPosition">The current position before the re-order takes place</param>
    /// <param name="newPosition">The new queue position after the re-order takes place</param>
    /// <returns>Int | 0 (no change) | 1 (move up e.g. p.5 --> p.4) | -1 (move down e.g. p.2 --> p.6)</returns>
    public int GetDirectionOfMove(int currentPosition, int newPosition)
    {
        return currentPosition - newPosition == 0 ? 0 : 
            (currentPosition - newPosition) > 0 ? 1 : -1;
    }
    // Example of how to change the comparer at runtime
    public void ChangeComparer(IComparer<T> newComparer)
    {
        this.comparer = newComparer;
    }
}
