using ParcelCostCalculator.Enums;
using ParcelCostCalculator.Models;
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

    [Fact]
    public void CalculateTotalParcelOrderCost_NullCostCalculatedParcelList_ShouldThrowNullArgumentException()
    {
        // Arrange
        // Act
        // Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _sut.CalculateTotalParcelOrderCost(null!));

        Assert.Equal("costCalculatedParcels", ex.ParamName);
    }

    [Fact]
    public void CalculateTotalParcelOrderCost_EmptyCostCalculatedParcelList_ShouldReturnTotalOrderCostOfZero()
    {
        // Arrange
        var costCalculatedParcels = new List<CostCalculatedParcel>();

        // Act
        var totalOrderCost = _sut.CalculateTotalParcelOrderCost(costCalculatedParcels);

        // Assert
        Assert.Equal(0, totalOrderCost);
    }

    [Fact]
    public void CalculateTotalParcelOrderCost_SingleItemCostCalculatedParcelList_ShouldReturnTotalOrderCostEquivalentToSingleItemCost()
    {
        // Arrange
        var parcel = new Parcel(1, 2, 3);
        var costCalculatedParcel = new CostCalculatedParcel(parcel, 10);
        var costCalculatedParcels = new List<CostCalculatedParcel> { costCalculatedParcel };

        // Act
        var totalOrderCost = _sut.CalculateTotalParcelOrderCost(costCalculatedParcels);

        // Assert
        Assert.Equal(costCalculatedParcel.Cost, totalOrderCost);
    }

    [Fact]
    public void CalculateTotalParcelOrderCost_MultipleItemsCostCalculatedParcelList_ShouldReturnTotalOrderCostAcrossAllParcels()
    {
        // Arrange
        var parcels = new List<Parcel>
        {
            new (1, 2, 3),
            new (50, 2, 3),
            new (100, 2, 3),
        };

        // 3 parcels with hard-code cost of 10, so the expected cost is 30
        var costCalculatedParcels = parcels
            .Select(p => new CostCalculatedParcel(p, 10))
            .ToList();

        var expectedTotalCost = costCalculatedParcels.Sum(p => p.Cost);

        // Act
        var totalOrderCost = _sut.CalculateTotalParcelOrderCost(costCalculatedParcels);

        // Assert
        Assert.Equal(expectedTotalCost, totalOrderCost);
    }
}
