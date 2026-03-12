using ParcelCostCalculator.Enums;
using ParcelCostCalculator.Models;

namespace ParcelCostCalculatorTests;

public class ParcelTypeMappingTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidLength_ShouldThrowArgumentOutOfRangeException(int length)
    {
        // Arrange
        // Act
        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Parcel(length, 1, 1));

        Assert.Equal("length", ex.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidWidth_ShouldThrowArgumentOutOfRangeException(int width)
    {
        // Arrange
        // Act
        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Parcel(1, width, 1));

        Assert.Equal("width", ex.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidHeight_ShouldThrowArgumentOutOfRangeException(int height)
    {
        // Arrange
        // Act
        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Parcel(1, 1, height));

        Assert.Equal("height", ex.ParamName);
    }

    [Theory]
    [InlineData(1, 2, 3, ParcelTypeEnum.Small)]
    [InlineData(9, 9, 9, ParcelTypeEnum.Small)]
    [InlineData(10, 11, 12, ParcelTypeEnum.Medium)]
    [InlineData(49, 9, 8, ParcelTypeEnum.Medium)]
    [InlineData(50, 51, 52, ParcelTypeEnum.Large)]
    [InlineData(99, 50, 3, ParcelTypeEnum.Large)]
    [InlineData(100, 101, 102, ParcelTypeEnum.ExtraLarge)]
    [InlineData(101, 1, 2, ParcelTypeEnum.ExtraLarge)]
    public void ParcelTypeClassification_ParcelDimensions_ShouldClassifyExpectedParcelType(int length, int width, int height, ParcelTypeEnum expectedParcelType)
    {
        // Arrange
        // Act
        var parcel = new Parcel(length, width, height);

        // Assert
        Assert.Equal(expectedParcelType, parcel.Type);
    }
}
