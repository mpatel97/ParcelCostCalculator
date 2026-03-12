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

    public ParcelOrderCost CalculateParcelOrder(ParcelOrder parcelOrder)
    {
        ArgumentNullException.ThrowIfNull(parcelOrder);

        var parcels = parcelOrder.Parcels;

        var costCalculatedParcels = parcels
            .Select(_parcelPricingService.CalculateParcelShippingCost)
            .ToList(); // Materialize the list to avoid multiple enumerations

        var totalCost = _parcelPricingService.CalculateTotalParcelOrderCost(costCalculatedParcels);

        var parcelOrderCost = new ParcelOrderCost(costCalculatedParcels, totalCost);

        ApplySpeedyShipping(parcelOrder, parcelOrderCost);

        return parcelOrderCost;
    }

    private void ApplySpeedyShipping(ParcelOrder parcelOrder, ParcelOrderCost parcelOrderCost)
    {
        if (parcelOrder.IsSpeedyShipping)
        {
            var speedyShipping = _parcelPricingService.CalculateSpeedyShippingCost(parcelOrderCost.TotalCost);

            parcelOrderCost.SpeedyShippingCost = speedyShipping;
        }
    }
}
