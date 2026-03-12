using ParcelCostCalculator.Enums;
using ParcelCostCalculator.Services;
using ParcelCostCalculatorTests.Data;

namespace ParcelCostCalculatorTests;

public class ParcelPricingServiceTests
{
    private readonly IParcelPricingService _sut;

    public ParcelPricingServiceTests()
    {
        _sut = new ParcelPricingService();
    }

    [Fact]
    public void CalculateParcelShippingCost_NullParcel_ShouldThrowNullArgumentException()
    {
        // Arrange
        // Act
        // Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _sut.CalculateParcelShippingCost(null!));

        Assert.Equal("parcel", ex.ParamName);
    }

    [Theory]
    [InlineData(ParcelTypeEnum.Small, 3)]
    [InlineData(ParcelTypeEnum.Medium, 8)]
    [InlineData(ParcelTypeEnum.Large, 15)]
    [InlineData(ParcelTypeEnum.ExtraLarge, 25)]
    public void CalculateParcelShippingCost_ParcelType_ShouldCalculateCostForParcelType(ParcelTypeEnum parcelType, int expectedCost)
    {
        // Arrange
        var parcel = TestData.ParcelCostMappingTestData[parcelType];

        // Act
        var costCalculationParcel = _sut.CalculateParcelShippingCost(parcel);

        // Assert
        Assert.NotNull(costCalculationParcel);
        Assert.Equal(expectedCost, costCalculationParcel.Cost);
    }
}
