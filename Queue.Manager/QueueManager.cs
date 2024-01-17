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
            //item.QueuePosition = queue.Count() + 1;
            item.SetQueuePosition(queue.Count() + 1);
            queue.Enqueue(item);
        }

        public T[] RemoveItemAt(int queuePosition)
        {           
            if (queuePosition > queue.Count)
                throw new IndexOutOfRangeException("Queue Position is out of range");

            T[] items = queue.ToArray();
            Array.Sort(items, comparer);
            T itemToRemove = items[queuePosition - 1];
            for (int i = queuePosition; i < items.Length; i++)
            {
                //items[i].QueuePosition--;
                items[i].SetQueuePosition(items[i].QueuePosition - 1);
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
        /// Method is called when an item is being moved up the queue. 
        /// i.e. From Position 2 to Position 1
        /// </summary>
        /// <param name="item">The item in the queue that is being moved</param>
        /// <param name="requestedPosition"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Promote(T item, int requestedPosition)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");
            if (requestedPosition < 1 || requestedPosition > queue.Count)
                throw new ArgumentException("The Requested Position is Invalid");
            if (item.QueuePosition - requestedPosition < 0)
                throw new InvalidOperationException($"Current queue position <{item.QueuePosition}> " +
                    $"cannot be higher in the queue than the requested position <{requestedPosition}>");

            T[] items = queue.ToArray();
            Array.Sort(items, comparer);

            // Demote items that are higher than the current position and lower than or equal to the requested position
            for (int i = requestedPosition - 1; i < item.QueuePosition - 1; i++)
            {
                items.ElementAt(i).SetQueuePosition(items.ElementAt(i).QueuePosition + 1);
            }

            // Update item with new position
            item.SetQueuePosition(requestedPosition);

            queue.Clear();

            foreach (var arrayItem in items)
            {
                queue.Enqueue(arrayItem);
            }
        }

        /// <summary>
        /// Method is called when an item is being moved down the queue. 
        /// i.e. From Position 1 to Position 2
        /// </summary>
        /// <param name="item"></param>
        /// <param name="requestedPosition">The item in the queue that is being moved</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Demote(T item, int requestedPosition)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");
            if (requestedPosition < 1 || requestedPosition > queue.Count)
                throw new ArgumentException("The Requested Position is Invalid");
            if (item.QueuePosition - requestedPosition > 0)
                throw new InvalidOperationException($"Current queue position <{item.QueuePosition}> " +
                    $"cannot be lower in the queue than the requested position <{requestedPosition}>");

            T[] items = queue.ToArray();
            Array.Sort(items, comparer);

            // Promote items that are lower than the current position and higher than or equal to the requested position
            for (int i = item.QueuePosition; i < requestedPosition; i++)
            {
                items.ElementAt(i).SetQueuePosition(items.ElementAt(i).QueuePosition - 1);
            }

            // Update item with new position
            item.SetQueuePosition(requestedPosition);

            queue.Clear();

            foreach (var arrayItem in items)
            {
                queue.Enqueue(arrayItem);
            }
        }

        public T[] ReOrder(T item, int requestedPostion)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (queue.Count == 0) throw new InvalidOperationException("Queue is empty");
            if (!queue.Contains(item)) throw new InvalidOperationException("Item not found in the Queue");
            if (queue.Count < requestedPostion || requestedPostion < 1) throw new InvalidOperationException("New position is out of bounds");

            var moveDirection = GetDirectionOfMove(item.QueuePosition, requestedPostion);
            switch (moveDirection)
            {
                case 0:
                    break;
                case 1:
                    Promote(item, requestedPostion);
                    break;
                case -1:
                    Demote(item, requestedPostion);
                    break;
                default:
                    break;
            }

            T[] updatedQueuePositionItems = queue.ToArray();
            Array.Sort(updatedQueuePositionItems, comparer);
            return updatedQueuePositionItems;
        }

        public int Count => queue.Count;
        public T GetFirstItem
        {
            get
            {
                var enumerator = queue.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current;
            }
        }

        /// <summary>
        /// Return the direction for the requested re-order
        /// </summary>
        /// <param name="currentPosition">The current position before the re-order takes place</param>
        /// <param name="requestedPosition">The new queue position after the re-order takes place</param>
        /// <returns>Int | 0 (no change) | 1 (promote e.g. p.5 --> p.4) | -1 (demote e.g. p.2 --> p.6)</returns>
        public int GetDirectionOfMove(int currentPosition, int requestedPosition)
        {
            return currentPosition - requestedPosition == 0 ? 0 :
                (currentPosition - requestedPosition) > 0 ? 1 : -1;
        }
        // Example of how to change the comparer at runtime
        public void ChangeComparer(IComparer<T> newComparer)
        {
            this.comparer = newComparer;
        }
    }

}