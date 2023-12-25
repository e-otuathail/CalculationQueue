// See https://aka.ms/new-console-template for more information
// Using the ReorderableQueue
using System.Text.RegularExpressions;

IQueueManager<CustomObject> reorderableQueue = new QueueManager<CustomObject>(new CustomComparer());

reorderableQueue.Enqueue(new CustomObject { Name = "Item C", Region = "HK", QueuePosition = 3 });
reorderableQueue.Enqueue(new CustomObject { Name = "Item A", Region = "Dub", QueuePosition = 1 });
reorderableQueue.Enqueue(new CustomObject { Name = "Item B", Region = "Lux", QueuePosition = 2 });
reorderableQueue.Enqueue(new CustomObject { Name = "Item D", Region = "Dub", QueuePosition = 4 });
reorderableQueue.Enqueue(new CustomObject { Name = "Item E", Region = "Lux", QueuePosition = 5 });
reorderableQueue.Enqueue(new CustomObject { Name = "Item F", Region = "HK", QueuePosition = 6 });

Console.WriteLine();

var sortedQueue = reorderableQueue.Sort();

Console.WriteLine("Sorted Queue");
foreach (var item in sortedQueue)
{
    Console.WriteLine($"Position: {item.QueuePosition} {item.Name} {item.Region}");
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
    Console.WriteLine($"Position: {item.QueuePosition} {item.Name} {item.Region}");
}
    

return;

 
