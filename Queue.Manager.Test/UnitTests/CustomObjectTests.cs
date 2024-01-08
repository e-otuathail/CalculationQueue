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
            CustomObject sut = new CustomObject();

            // Assert
            Assert.That(sut.Name, Is.EqualTo("Undefined"));
        }

        [Test]
        public void OnObjectCreation_WhenDefaultConstructorIsUsed_ThenRegionPropertyDefaultsToUndefined()
        {
            // Arrange
            // Act
            CustomObject sut = new CustomObject();

            // Assert
            Assert.That(sut.Region, Is.EqualTo("Undefined"));
        }

        [Test]
        public void GetHashCode_WhenDefaultConstructorIsUsed_ThenReturnTheSameHashCode()
        {
            // Arrange
            CustomObject sutA = new CustomObject();
            CustomObject sutB = new CustomObject();

            // Act
            var resultA = sutA.GetHashCode();
            var resultB = sutB.GetHashCode();

            // Assert
            Assert.That(resultA, Is.EqualTo(resultB));
        }
    }
}