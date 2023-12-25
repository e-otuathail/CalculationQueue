using NUnit;

namespace Queue.Manager.Test.UnitTests
{
    public class CustomObjectTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OnObjectCreation_WhenDefaultConstructorIsUsed_ThenNamePropertyDefaultsToUndefined()
        {
            // Arrange
            // Act
            ICustomObject sut = new CustomObject();

            // Assert
            Assert.That(sut.Name, Is.EqualTo("Undefined"));
        }

        [Test]
        public void OnObjectCreation_WhenDefaultConstructorIsUsed_ThenRegionPropertyDefaultsToUndefined()
        {
            // Arrange
            // Act
            ICustomObject sut = new CustomObject();

            // Assert
            Assert.That(sut.Region, Is.EqualTo("Undefined"));
        }

        [Test]
        public void Equals_WhenDefaultConstructorIsUsed_ThenReturnFalse()
        {
            // Arrange
            ICustomObject objectBeingCompared = new CustomObject();
            ICustomObject sut = new CustomObject 
            { 
                QueuePosition = 1 
            };

            // Act
            var result = sut.Equals(objectBeingCompared);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void GetHashCode_WhenDefaultConstructorIsUsed_ThenReturnTheSameHashCode()
        {
            // Arrange
            ICustomObject sutA = new CustomObject();
            ICustomObject sutB = new CustomObject();

            // Act
            var resultA = sutA.GetHashCode();
            var resultB = sutB.GetHashCode();

            // Assert
            Assert.That(resultA, Is.EqualTo(resultB));
        }
    }
}