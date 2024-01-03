using NUnit;
using Moq;
using NUnit.Framework.Internal;

namespace Queue.Manager.Test.UnitTests
{
    public class QueueManagerTests
    {
        [SetUp]
        public void Setup()
        {
            var customObjectMock = new Mock<ICustomObject>();
            customObjectMock.Setup(m => m.QueuePosition).Returns(1);
            var customComparerMock = new Mock<ICustomComparer>();
        }

        [Test]
        public void GetDirectionOfMove_WhenCurrentAndNewPositionAreTheSame_ThenReturnZero()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new CustomComparer());
            int currentPosition = 1;
            int newPosition = 1;
            
            // Act
            var result = sut.GetDirectionOfMove(currentPosition, newPosition);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Dequeue_WhenNoItemsInTheQueue_ThenRaiseAnInvalidOperationException()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new CustomComparer());

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Dequeue());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Queue is empty"));
        }


        [Test]
        public void Dequeue_WhenItemsInTheQueue_ThenReduceTheNumberOfItemsByOne()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new CustomComparer());
            sut.Enqueue(new CustomObject { Name = "Item C", Region = "HK", QueuePosition = 3 });
            sut.Enqueue(new CustomObject { Name = "Item A", Region = "Dub", QueuePosition = 1 });
            sut.Enqueue(new CustomObject { Name = "Item B", Region = "Lux", QueuePosition = 2 });
            int itemCountBefore = sut.Count;

            // Act
            var itemDequeued = sut.Dequeue();

            // Assert
            Assert.That(itemCountBefore -1, Is.EqualTo(sut.Count));
        }


        [Test]
        public void DecrementPosition_WhenitemsAreBetweenCurrentAndNewPosition_ThenDecrementQueuePositionForAllByOne()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new CustomComparer());
            var item = new CustomObject { Name = "Item A", Region = "Dub", QueuePosition = 1 };
            var itemInQueue = new CustomObject { Name = "Item B", Region = "Lux", QueuePosition = 2 };
            sut.Enqueue(item);
            sut.Enqueue(itemInQueue);

            int newPosition = 2;

            // Act
            sut.DecrementPosition(item, newPosition);

            // Assert
            var actual = (from a in sut.Sort()
                            where a.Name == itemInQueue.Name
                            select a.QueuePosition)
                            .Single();

            Assert.That(actual, Is.EqualTo(1));
        }


        [Test]
        public void IncrementPosition_WhenitemsAreBetweenCurrentAndNewPosition_ThenIncrementQueuePositionForAllByOne()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new CustomComparer());
            var itemInQueue = new CustomObject { Name = "Item A", Region = "Dub", QueuePosition = 1 };
            var item = new CustomObject { Name = "Item B", Region = "Lux", QueuePosition = 2 };
            sut.Enqueue(itemInQueue);
            sut.Enqueue(item);

            int newPosition = 1;

            // Act
            sut.IncrementPosition(item, newPosition);

            // Assert
            var actual = (from a in sut.Sort()
                          where a.Name == itemInQueue.Name
                          select a.QueuePosition)
                              .Single();

            Assert.That(actual, Is.EqualTo(2));
        }

        [Test]
        public void IncrementPosition_WhenCurrentPositionIsLessThanNewPosition_ThenThrowArgumentException()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new CustomComparer());
            var item = new CustomObject { Name = "Item A", Region = "Dub", QueuePosition = 1 };
            sut.Enqueue(item);

            int newPosition = 2;

            string expectedMessage = $"Current queue position <{item.QueuePosition}> " +
                $"cannot be higher in the queue than the requested position <{newPosition}>";

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => sut.IncrementPosition(item, newPosition));

            // Assert
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}
