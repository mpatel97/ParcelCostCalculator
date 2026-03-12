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

    public ParcelOrderCost CalculateParcelOrder(IEnumerable<Parcel> parcels)
    {
        ArgumentNullException.ThrowIfNull(parcels);

        var costCalculatedParcels = parcels
            .Select(_parcelPricingService.CalculateParcelShippingCost)
            .ToList(); // Materialize the list to avoid multiple enumerations

        var totalCost = _parcelPricingService.CalculateTotalParcelOrderCost(costCalculatedParcels);

        var parcelOrderCost = new ParcelOrderCost(costCalculatedParcels, totalCost);
        return parcelOrderCost;
    }
}
