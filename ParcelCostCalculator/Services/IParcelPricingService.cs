using ParcelCostCalculator.Models;

namespace ParcelCostCalculator.Services;

public interface IParcelPricingService
{
    public CostCalculatedParcel CalculateParcelShippingCost(Parcel parcel);

    int CalculateTotalParcelOrderCost(IList<CostCalculatedParcel> costCalculatedParcels);

    int CalculateSpeedyShippingCost(int totalParcelOrderCost);

    bool IsParcelOverWeightLimit(Parcel parcel, out int weightDifference);
}
