using ParcelCostCalculator;
using ParcelCostCalculator.Enums;
using ParcelCostCalculator.Models;
using ParcelCostCalculator.Services;
using ParcelCostCalculatorTests.Data;

namespace ParcelCostCalculatorTests;

public class ParcelOrderCostCalculatorTests
{
    private readonly ParcelPricingService _parcelPricingService;
    private readonly ParcelOrderCostCalculator _sut;

    public ParcelOrderCostCalculatorTests()
    {
        // Since this is a simple service with no dependencies, we can use the real implementation for testing.
        // If it had dependencies, we would mock them here.
        _parcelPricingService = new ParcelPricingService();

        _sut = new ParcelOrderCostCalculator(_parcelPricingService);
    }

    [Fact]
    public void Constructor_NullParcelPricingService_ShouldThrowArgumentNullException()
    {
        // Arrange
        // Act
        // Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new ParcelOrderCostCalculator(null!));

        Assert.Equal("parcelPricingService", ex.ParamName);
    }

    [Fact]
    public void CalculateTotalOrderCost_EmptyParcelList_ShouldReturnTotalOrderCostOfZero()
    {
        // Arrange
        var parcels = new List<Parcel>();

        // Act
        var parcelOrderCost = _sut.CalculateTotalOrderCost(parcels);

        // Assert
        Assert.NotNull(parcelOrderCost);
        Assert.Equal(0, parcelOrderCost.TotalCost);
    }

    [Fact]
    public void CalculateTotalOrderCost_SingleItemParcelList_ShouldReturnTotalOrderCostEquivalentToSingleItemCost()
    {
        // Arrange
        var smallParcel = TestData.ParcelCostMappingTestData[ParcelTypeEnum.Small];
        var parcels = new List<Parcel> { smallParcel };

        var costCalculatedSmallParcel = _parcelPricingService.CalculateParcelShippingCost(smallParcel);

        // Act
        var parcelOrderCost = _sut.CalculateTotalOrderCost(parcels);

        // Assert
        Assert.NotNull(parcelOrderCost);
        Assert.Equal(parcelOrderCost.TotalCost, costCalculatedSmallParcel.Cost);
    }

    [Fact]
    public void CalculateTotalOrderCost_MultipleItemsParcelList_ShouldReturnTotalOrderCostAcrossAllParcels()
    {
        // Arrange
        var allParcels = TestData.ParcelCostMappingTestData
            .Select(kvp => kvp.Value)
            .ToList();

        var expectedTotalCost = allParcels
            .Select(_parcelPricingService.CalculateParcelShippingCost)
            .Sum(p => p.Cost);

        // Act
        var parcelOrderCost = _sut.CalculateTotalOrderCost(allParcels);

        // Assert
        Assert.NotNull(parcelOrderCost);
        Assert.Equal(parcelOrderCost.TotalCost, expectedTotalCost);
    }
}
