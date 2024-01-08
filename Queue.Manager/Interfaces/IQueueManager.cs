namespace Queue.Manager.Interfaces
{
    public interface IQueueManager<T> where T : class
    {
        int Count { get; }
        T Current { get; }

        void ChangeComparer(IComparer<T> newComparer);
        T Dequeue();
        void Enqueue(T item);
        int GetDirectionOfMove(int currentPosition, int newPosition);
        void Demote(T item, int newPosition);
        void Promote(T item, int newPosition);
        T[] ReOrder(T item, int newPostion);
        T[] Sort();
        T[] RemoveItemAt(int queuePosition);
    }
}