
// Comparer based on Priority property
public class CustomComparer : IComparer<ICustomObject>, ICustomComparer
{
    public int Compare(ICustomObject? x, ICustomObject? y)
    {
        if (x is null || y is null)
            return -1;
        return x.QueuePosition.CompareTo(y.QueuePosition);
    }
}
