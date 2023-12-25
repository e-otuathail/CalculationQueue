public class CustomObject : ICustomObject
{
    public int QueuePosition { get; set; }
    public bool ReOrder { get; set; }
    public int ReOrderDirection { get; set; }
    public int RequestedQueuePosition { get; set; }
    public string Name { get; set; } = "Undefined";
    public string Region { get; set; } = "Undefined";

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is not ICustomObject) return false;
        return Equals(obj as ICustomObject) ;
    }

    private bool Equals(ICustomObject? other)
    {
        if (other == null) return false;
        if (other is not ICustomObject) return false;
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
