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
    public void CalculateParcelShippingCost_GivenParcelType_ShouldCalculateCostForParcelType(ParcelTypeEnum parcelType, int expectedCost)
    {
        // Arrange
        var parcel = TestData.ParcelCostMappingTestData[parcelType];

        // Act
        var costCalculationParcel = _sut.CalculateParcelShippingCost(parcel);

        // Assert
        Assert.NotNull(costCalculationParcel);
        Assert.Equal(expectedCost, costCalculationParcel.Cost);
    }

    [Theory]
    [InlineData(ParcelTypeEnum.Small, 1, 3)]
    [InlineData(ParcelTypeEnum.Medium, 3, 8)]
    [InlineData(ParcelTypeEnum.Large, 6, 15)]
    [InlineData(ParcelTypeEnum.ExtraLarge, 10, 25)]
    public void CalculateParcelShippingCost_GivenWeightUnderParcelTypeLimit_ShouldCalculateCostForParcelTypeWithOverweightChargeApplied(ParcelTypeEnum parcelType, int weight, int expectedCost)
    {
        // Arrange
        var parcel = TestData.ParcelCostMappingTestData[parcelType];
        var weightedParcel = new WeightedParcel(parcel, weight);

        // Act
        var costCalculationParcel = _sut.CalculateParcelShippingCost(weightedParcel);

        // Assert
        Assert.NotNull(costCalculationParcel);
        Assert.Equal(expectedCost, costCalculationParcel.Cost);
    }

    [Theory]
    [InlineData(ParcelTypeEnum.Small, 2, 3+2)]
    [InlineData(ParcelTypeEnum.Medium, 4, 8+2)]
    [InlineData(ParcelTypeEnum.Large, 8, 15+2+2)] // over 2 units of weight limit, so 2 overweight charges applied
    [InlineData(ParcelTypeEnum.ExtraLarge, 13, 25+2+2+2)] // over 3 units of weight limit, so 3 overweight charges applied
    public void CalculateParcelShippingCost_GivenWeightOverParcelTypeLimit_ShouldCalculateCostForParcelTypeWithOverweightChargeApplied(ParcelTypeEnum parcelType, int weight, int expectedCost)
    {
        // Arrange
        var parcel = TestData.ParcelCostMappingTestData[parcelType];
        var weightedParcel = new WeightedParcel(parcel, weight);

        // Act
        var costCalculationParcel = _sut.CalculateParcelShippingCost(weightedParcel);

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

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 4)]
    [InlineData(3, 6)]
    [InlineData(25, 50)]
    public void CalculateSpeedyShippingCost_TotalParcelOrderCost_ShouldReturnDoubleTheTotalCost(int totalParcelOrderCost, int expectedSpeedyShippingCost)
    {
        // Arrange
        // Act
        var speedyShippingCost = _sut.CalculateSpeedyShippingCost(totalParcelOrderCost);

        // Assert
        Assert.Equal(expectedSpeedyShippingCost, speedyShippingCost);
    }

    [Fact]
    public void IsParcelOverWeightLimit_NullParcel_ShouldThrowNullArgumentException()
    {
        // Arrange
        // Act
        // Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _sut.IsParcelOverWeightLimit(null!, out var weightDifference));

        Assert.Equal("parcel", ex.ParamName);
    }

    [Theory]
    [InlineData(ParcelTypeEnum.Small, 2)]
    [InlineData(ParcelTypeEnum.Medium, 4)]
    [InlineData(ParcelTypeEnum.Large, 7)]
    [InlineData(ParcelTypeEnum.ExtraLarge, 11)]
    public void IsParcelOverWeightLimit_ParcelWeightLimitIsBreached_ShouldReturnTrue(ParcelTypeEnum parcelType, int parcelWeight)
    {
        // Arrange
        var parcel = TestData.ParcelCostMappingTestData[parcelType];
        var weightedParcel = new WeightedParcel(parcel, parcelWeight);

        // Act
        var isParcelOverWeightLimit = _sut.IsParcelOverWeightLimit(weightedParcel, out var weightDifference);

        // Assert
        Assert.True(isParcelOverWeightLimit);
        Assert.Equal(1, weightDifference); // All inline data breaches the weight limit by 1 unit, so the expected weight difference is 1
    }
}
