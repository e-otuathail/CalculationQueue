//using Queue.Manager.Interfaces;

namespace Queue.Manager
{
    public class CustomObject
    {
        public string Name { get; set; } = "Undefined";
        public string Region { get; set; } = "Undefined";
        public int QueuePosition { get; private set; } = 0;

        internal void SetQueuePosition(int position) => QueuePosition = position;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not CustomObject) return false;
            return Equals(obj as CustomObject);
        }

        private bool Equals(CustomObject? other)
        {
            if (other == null) return false;
            if (other is not CustomObject) return false;
            if (other.QueuePosition == 0) return false;

            return other != null &&
                Name == other.Name &&
                Region == other.Region &&
                QueuePosition == other.QueuePosition;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name, this.Region, this.QueuePosition);
        }
    }
}
