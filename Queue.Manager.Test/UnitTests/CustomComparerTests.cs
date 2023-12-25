namespace Queue.Manager.Test.UnitTests
{
    public class CustomComparerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Compare_WhenObjectsHaveSameQueuePositions_ThenReturnZero()
        {
            // Arrange
            ICustomObject x = new CustomObject { Name = "Item C", Region = "HK", QueuePosition = 3 };
            ICustomObject y = new CustomObject { Name = "Item C", Region = "HK", QueuePosition = 3 };
            ICustomComparer sut = new CustomComparer();

            // Act
            var result = sut.Compare(x, y);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Compare_WhenObjectsHaveDifferentQueuePositions_ThenDoesNotReturnZero()
        {
            // Arrange
            ICustomObject x = new CustomObject { Name = "Item C", Region = "HK", QueuePosition = 3 };
            ICustomObject y = new CustomObject { Name = "Item B", Region = "Lux", QueuePosition = 2 };
            ICustomComparer sut = new CustomComparer();

            // Act
            var result = sut.Compare(x, y);

            // Assert
            Assert.That(result, Is.Not.EqualTo(0));
        }
    }
}