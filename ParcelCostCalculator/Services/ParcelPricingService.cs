using ParcelCostCalculator.Enums;
using ParcelCostCalculator.Models;

namespace ParcelCostCalculator.Services;

public class ParcelPricingService: IParcelPricingService
{
    private const int SpeedyShippingMultiplier = 2;

    // Whole dollar amount mapping for each parcel type
    private static readonly Dictionary<ParcelTypeEnum, int> ParcelTypeCostMapping = new()
    {
        { ParcelTypeEnum.Small, 3 },
        { ParcelTypeEnum.Medium, 8 },
        { ParcelTypeEnum.Large, 15 },
        { ParcelTypeEnum.ExtraLarge, 25 }
    };

    public ParcelPricingService()
    {
    }

    public CostCalculatedParcel CalculateParcelShippingCost(Parcel parcel)
    {
        ArgumentNullException.ThrowIfNull(parcel);

        // Curreently impossible case since ParcelType is determined by the Parcel class itself,
        // and all parcel types are accounted for in the cost mappings
        // but we should still handle it gracefully if a new parcel type is added in the future without updating the cost mapping
        if (!ParcelTypeCostMapping.TryGetValue(parcel.Type, out var cost))
        {
            throw new ArgumentOutOfRangeException($"Unsupported parcel type: {parcel.Type}");
        }

        var costCalculatedParcel = new CostCalculatedParcel(parcel, cost);
        return costCalculatedParcel;
    }

    public int CalculateTotalParcelOrderCost(IList<CostCalculatedParcel> costCalculatedParcels)
    {
        ArgumentNullException.ThrowIfNull(costCalculatedParcels);

        return costCalculatedParcels.Sum(p => p.Cost);
    }

    public int CalculateSpeedyShippingCost(int totalParcelOrderCost)
    {
        return totalParcelOrderCost * SpeedyShippingMultiplier;
    }
}
