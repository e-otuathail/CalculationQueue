
//using Queue.Manager.Interfaces;

namespace Queue.Manager
{
    public class CustomComparer : IComparer<CustomObject>
    {
        public int Compare(CustomObject? x, CustomObject? y) 
        {
            if (x is null || y is null)
                return -1;
            return x.QueuePosition.CompareTo(y.QueuePosition);
        }
    }
}
