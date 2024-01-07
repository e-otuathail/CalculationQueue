// See https://aka.ms/new-console-template for more information
// Using the ReorderableQueue
using Queue.Manager;
using System.Text.RegularExpressions;

QueueManager<CustomObject> reorderableQueue = new QueueManager<CustomObject>(new QueuePositionComparer());

reorderableQueue.Enqueue(new CustomObject { Name = "A", Region = "Region 1" }); // QueuePosition = 1
reorderableQueue.Enqueue(new CustomObject { Name = "B", Region = "Region 2" }); // QueuePosition = 2
reorderableQueue.Enqueue(new CustomObject { Name = "C", Region = "Region 3" }); // QueuePosition = 3
reorderableQueue.Enqueue(new CustomObject { Name = "D", Region = "Region 1" }); // QueuePosition = 4
reorderableQueue.Enqueue(new CustomObject { Name = "E", Region = "Region 2" }); // QueuePosition = 5
reorderableQueue.Enqueue(new CustomObject { Name = "F", Region = "Region 3" }); // QueuePosition = 6

Console.WriteLine();

var sortedQueue = reorderableQueue.Sort();

Console.WriteLine("Sorted Queue");
foreach (var item in sortedQueue)
{
    Console.WriteLine($"Item: {item.Name} >> Position: {item.QueuePosition} ");
}

Console.WriteLine();

Console.WriteLine("Select an item to re-order: ");
var reOrderItem = Console.ReadLine();
string pattern = @"[A-Z]";

if (reOrderItem != null)
{
    Match m = Regex.Match(reOrderItem, pattern, RegexOptions.IgnoreCase);
    if (m.Success)
        reOrderItem = reOrderItem.ToUpper();
    else
    {
        Console.WriteLine("Unexpected input. Enter character A - Z");
        return;
    }
}
else
{
    Console.WriteLine("Unexpected input. Please selected an Item from the list");
    return;
}

Console.WriteLine($"Choose Item {reOrderItem}'s new position: ");

var sPosition = Console.ReadLine();
bool canParse = Int32.TryParse(sPosition, out int iPosition);
if (!canParse)
{
    Console.WriteLine("Unexpected input. Please selected a valid position in the queue");
    return;
}

var reOrderedQueue = reorderableQueue.ReOrder(sortedQueue.Where(x => x.Name.Contains(reOrderItem)).First(), iPosition);

Console.WriteLine("Re-Ordered");

foreach (var item in reOrderedQueue)
{
    Console.WriteLine($"Item: {item.Name} >> Position: {item.QueuePosition}");
}
    

return;

 
