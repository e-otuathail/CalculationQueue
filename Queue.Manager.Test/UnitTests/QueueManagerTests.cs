using NUnit;
using Moq;

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
    }
}
