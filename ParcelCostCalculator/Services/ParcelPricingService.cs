using ParcelCostCalculator.Enums;
using ParcelCostCalculator.Models;

namespace ParcelCostCalculator.Services;

public class ParcelPricingService: IParcelPricingService
{
    private const int SpeedyShippingMultiplier = 2;
    private const int OverweightChargePerKg = 2;

    // Whole dollar amount mapping for each parcel type
    private static readonly Dictionary<ParcelTypeEnum, int> ParcelTypeCostMapping = new()
    {
        { ParcelTypeEnum.Small, 3 },
        { ParcelTypeEnum.Medium, 8 },
        { ParcelTypeEnum.Large, 15 },
        { ParcelTypeEnum.ExtraLarge, 25 }
    };

    private static readonly Dictionary<ParcelTypeEnum, int> ParcelTypeWeightLimitMapping = new()
    {
        { ParcelTypeEnum.Small, 1 },
        { ParcelTypeEnum.Medium, 3 },
        { ParcelTypeEnum.Large, 6 },
        { ParcelTypeEnum.ExtraLarge, 10 }
    };

    public ParcelPricingService()
    {
    }

    public CostCalculatedParcel CalculateParcelShippingCost(Parcel parcel)
    {
        ArgumentNullException.ThrowIfNull(parcel);

        // Currently impossible case since ParcelType is determined by the Parcel class itself,
        // and all parcel types are accounted for in the cost mappings
        // but we should still handle it gracefully if a new parcel type is added in the future without updating the cost mapping
        if (!ParcelTypeCostMapping.TryGetValue(parcel.Type, out var cost))
        {
            throw new ArgumentOutOfRangeException($"Unsupported parcel type: {parcel.Type}");
        }

        if (IsParcelOverWeightLimit(parcel, out var weightDifference))
        {
            // Add overweight charge of $2 per kg over the limit
            cost += weightDifference * OverweightChargePerKg;
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

    public bool IsParcelOverWeightLimit(Parcel parcel, out int weightDifference)
    {
        ArgumentNullException.ThrowIfNull(parcel);

        weightDifference = 0;

        if (parcel is not WeightedParcel weightedParcel)
        {
            // If the parcel doesn't have a weight, we can't determine if it's overweight, so we assume it's not
            return false;
        }

        // Currently impossible case since ParcelType is determined by the Parcel class itself,
        // and all parcel types are accounted for in the weight limit mappings
        if (!ParcelTypeWeightLimitMapping.TryGetValue(parcel.Type, out var weightLimit))
        {
            throw new ArgumentOutOfRangeException($"Unsupported parcel type: {parcel.Type}");
        }

        weightDifference = weightedParcel.Weight - weightLimit;

        return weightDifference > 0;
    }
}
