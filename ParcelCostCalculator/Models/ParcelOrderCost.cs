using System.Diagnostics.CodeAnalysis;

namespace ParcelCostCalculator.Models;

public class ParcelOrderCost
{
    public required IList<CostCalculatedParcel> CostCalculatedParcels { get; init; }

    public required int TotalCost { get; init; }

    public int? SpeedyShippingCost { get; set; }

    [SetsRequiredMembers]
    public ParcelOrderCost(IList<CostCalculatedParcel> costCalculatedParcels, int totalCost)
    {
        ArgumentNullException.ThrowIfNull(costCalculatedParcels);

        CostCalculatedParcels = costCalculatedParcels;
        TotalCost = totalCost;
    }
}
