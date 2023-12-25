public interface ICustomObject
{
    string Name { get; set; }
    int QueuePosition { get; set; }
    string Region { get; set; }
    bool ReOrder { get; set; }
    int ReOrderDirection { get; set; }
    int RequestedQueuePosition { get; set; }

}