using ParcelCostCalculator.Models;
using ParcelCostCalculator.Services;

namespace ParcelCostCalculator;

public class ParcelOrderCostCalculator
{
    private readonly IParcelPricingService _parcelPricingService;

    public ParcelOrderCostCalculator(IParcelPricingService parcelPricingService)
    {
        ArgumentNullException.ThrowIfNull(parcelPricingService);

        _parcelPricingService = parcelPricingService;
    }

    public ParcelOrderCost CalculateTotalOrderCost(IEnumerable<Parcel> parcels)
    {
        ArgumentNullException.ThrowIfNull(parcels);

        var costCalculatedParcels = parcels
            .Select(_parcelPricingService.CalculateParcelShippingCost)
            .ToList(); // Materialize the list to avoid multiple enumerations

        // Total cost is a simple sum, no need to split into seperate method for now
        var totalCost = costCalculatedParcels.Sum(p => p.Cost);

        var parcelOrderCost = new ParcelOrderCost(costCalculatedParcels, totalCost);
        return parcelOrderCost;
    }
}
