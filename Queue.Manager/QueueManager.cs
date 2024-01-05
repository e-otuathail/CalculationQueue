using Queue.Manager.Interfaces;

namespace Queue.Manager
{
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
            item.QueuePosition = queue.Count() + 1;
            queue.Enqueue(item);
        }

        public T[] RemoveItemAt(int queuePosition)
        {
            if (queue.Count < queuePosition - 1)
                throw new InvalidOperationException("Invalid Queue Position");

            T[] items = queue.ToArray();
            Array.Sort(items, comparer);
            T itemToRemove = items[queuePosition - 1];
            for (int i = queuePosition; i < items.Length; i++)
            {
                items[i].QueuePosition--;
            }
            queue = new Queue<T>(items.Where(x => !x.Equals(itemToRemove)).ToArray());
            return queue.ToArray();
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

        /// <summary>
        /// Method is called when the item is being moved down the queue. From higher to lower.
        /// </summary>
        /// <param name="item">The item in the Queue that is being moved to a new position</param>
        /// <param name="newPosition"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void IncrementPosition(T item, int newPosition)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (item.QueuePosition - newPosition < 0)
                throw new ArgumentException($"Current queue position <{item.QueuePosition}> " +
                    $"cannot be higher in the queue than the requested position <{newPosition}>");
            if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");
            if (item.QueuePosition < 1 || item.QueuePosition > queue.Count)
                throw new InvalidOperationException("Invalid Current Position");
            if (newPosition < 1 || newPosition > queue.Count)
                throw new InvalidOperationException("Invalid New Position");

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

        public void DecrementPosition(T item, int newPosition)
        {
            if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");
            if (item.QueuePosition < 1 || item.QueuePosition > queue.Count)
                throw new InvalidOperationException("Invalid Current Position");
            if (newPosition < 1 || newPosition > queue.Count)
                throw new InvalidOperationException("Invalid New Position");

            T[] items = queue.ToArray();
            Array.Sort(items, comparer);

            // Decrement the position by 1 for items lower than current position
            // and higher than or equal to new position
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

            var moveDirection = GetDirectionOfMove(item.QueuePosition, newPostion);
            switch (moveDirection)
            {
                case 0:
                    break;
                case 1:
                    IncrementPosition(item, newPostion);
                    break;
                case -1:
                    DecrementPosition(item, newPostion);
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

}