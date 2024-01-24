# Queue.Manager Project

## Overview
The Queue.Manager project is a robust and efficient queue management system. It is designed to handle and manage objects in a queue, providing functionalities such as enqueueing, dequeueing, sorting, and moving items within the queue.

## Features
- Enqueue: Add items to the queue.
- Dequeue: Remove items from the queue.
- Sort: Sort items in the queue based on their queue position.
- Move: Change the position of an item in the queue.

## Usage
Here is a basic example of how to use the Queue.Manager with a Queue.Caller:

```
using Queue.Manager;

QueueManager queueManager = new QueueManager(); 
CustomObject item = new CustomObject(); 
item.SetQueuePosition(1); queueManager.Enqueue(item);

```

## Limitations
- The Queue.Manager does not support multi-threading. Concurrent operations may lead to unexpected results.
- The Queue.Manager operates on a first-in, first-out (FIFO) basis. It does not support priority queuing.

## Testing
The project includes a suite of NUnit tests to ensure the functionality and reliability of the Queue.Manager.

## Contributing
Contributions are welcome. Please submit a pull request with any enhancements or bug fixes.

## License
This project is licensed under the terms of the MIT license.

