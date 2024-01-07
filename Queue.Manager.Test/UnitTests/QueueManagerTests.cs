using NUnit;
using Moq;
using NUnit.Framework.Internal;
using Queue.Manager.Interfaces;

namespace Queue.Manager.Test.UnitTests
{
    public class QueueManagerTests
    {
        [Test]
        public void GetDirectionOfMove_WhenCurrentAndNewPositionAreTheSame_ThenReturnZero()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            int currentPosition = 1;
            int newPosition = 1;
            
            // Act
            var result = sut.GetDirectionOfMove(currentPosition, newPosition);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Dequeue_WhenNoItemsInTheQueue_ThenThrowInvalidOperationException()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Dequeue());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Queue is empty"));
        }


        [Test]
        public void Dequeue_WhenItemsInTheQueue_ThenReduceTheNumberOfItemsByOne()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            sut.Enqueue(new CustomObject { Name = "Item A", Region = "Dub" });
            sut.Enqueue(new CustomObject { Name = "Item B", Region = "Lux" });
            sut.Enqueue(new CustomObject { Name = "Item C", Region = "HK" });
            int itemCountBefore = sut.Count;

            // Act
            var itemDequeued = sut.Dequeue();

            // Assert
            Assert.That(itemCountBefore -1, Is.EqualTo(sut.Count));
        }

        [Test]
        public void Enqueue_WhenItemAddedToTheQueue_ThenTheQueuePositionIsSetToTheQueueCountPlusOne()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());

            // Act
            sut.Enqueue(new CustomObject { Name = "Item A", Region = "Dub" });
            sut.Enqueue(new CustomObject { Name = "Item B", Region = "Lux" });
            sut.Enqueue(new CustomObject { Name = "Item C", Region = "HK" });

            // Assert
            var items = sut.Sort();
            Assert.Multiple(() =>
            {
                Assert.That(items[0].QueuePosition, Is.EqualTo(1));
                Assert.That(items[1].QueuePosition, Is.EqualTo(2));
                Assert.That(items[2].QueuePosition, Is.EqualTo(3));
            });
            
        }

        [Test]
        public void RemoveItemAt_WhenItemRemovedFromSpecifiedPosition_ThenNumberOfItemsInTheQueueIsReducedByOne()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            sut.Enqueue(new CustomObject { Name = "Item A", Region = "Dub" });
            sut.Enqueue(new CustomObject { Name = "Item B", Region = "Lux" });
            sut.Enqueue(new CustomObject { Name = "Item C", Region = "HK" });

            // Act
            var itemsRemaining = sut.RemoveItemAt(3);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(itemsRemaining, Has.Length.EqualTo(2));
                Assert.That(sut.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void RemoveItemAt_WhenItemRemovedFromSpecifiedPosition_ThenQueuePositionForRemainingItemsIsUpdated()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            sut.Enqueue(new CustomObject { Name = "Item A", Region = "Dub" });
            sut.Enqueue(new CustomObject { Name = "Item B", Region = "Lux" });
            sut.Enqueue(new CustomObject { Name = "Item C", Region = "HK" });
            sut.Enqueue(new CustomObject { Name = "Item D", Region = "HK" });

            // Act
            var itemsRemaining = sut.RemoveItemAt(2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(itemsRemaining[0].QueuePosition, Is.EqualTo(1));
                Assert.That(itemsRemaining[1].QueuePosition, Is.EqualTo(2));
                Assert.That(itemsRemaining[2].QueuePosition, Is.EqualTo(3));
            });
        }

        [Test]
        public void DecrementPosition_WhenitemsAreBetweenCurrentAndNewPosition_ThenDecrementQueuePositionForAllByOne()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            var item = new CustomObject { Name = "Item A", Region = "Dub" };
            var itemInQueue = new CustomObject { Name = "Item B", Region = "Lux" };
            sut.Enqueue(item);          // QueuePosition = 1
            sut.Enqueue(itemInQueue);   // QueuePosition = 2

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
            QueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            var itemInQueue = new CustomObject { Name = "Item A", Region = "Dub" };
            var item = new CustomObject { Name = "Item B", Region = "Lux" };
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
        public void IncrementPosition_WhenCurrentPositionIsLessThanNewPosition_ThenThrowInvalidOperationException()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            CustomObject item = new CustomObject { Name = "Item A", Region = "Dub" };
            sut.Enqueue(item);

            int newPosition = 2;

            string expectedMessage = $"Current queue position <{item.QueuePosition}> " +
                $"cannot be higher in the queue than the requested position <{newPosition}>";

            // Act
            var ex = Assert.Throws<InvalidOperationException>(
                () => sut.IncrementPosition(item, newPosition));

            // Assert
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
        
        [Test]
        public void IncrementPosition_WhenParameterNamedItemIsNull_ThenThrowArgumentNullException()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            CustomObject? customObject = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.IncrementPosition(customObject, 2));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'item')"));
        }


        [Test]
        public void IncrementPosition_WhenNoItemsInTheQueue_ThenThrowInvalidOperationException()
        {
            // Arrange
            IQueueManager<CustomObject> sut = new QueueManager<CustomObject>(new QueuePositionComparer());
            CustomObject customObject = new CustomObject { Name = "Item A", Region = "Dub" };

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => sut.IncrementPosition(customObject, 1));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Queue is empty"));
        }
    }
}
