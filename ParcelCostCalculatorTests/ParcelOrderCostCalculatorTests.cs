using ParcelCostCalculator;
using ParcelCostCalculator.Services;

namespace ParcelCostCalculatorTests;

public class ParcelOrderCostCalculatorTests
{
    private readonly ParcelOrderCostCalculator _sut;

    public ParcelOrderCostCalculatorTests()
    {
        // Since this is a simple service with no dependencies, we can use the real implementation for testing.
        // If it had dependencies, we would mock them here.
        var parcelPricingService = new ParcelPricingService();

        _sut = new ParcelOrderCostCalculator(parcelPricingService);
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


}
