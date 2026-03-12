using System.Diagnostics.CodeAnalysis;

namespace ParcelCostCalculator.Models;

public class CostCalculatedParcel
{
    public required Parcel Parcel { get; init; }

    public required int Cost { get; init; }

    [SetsRequiredMembers]
    public CostCalculatedParcel(Parcel parcel, int cost)
    {
        ArgumentNullException.ThrowIfNull(parcel);

        Parcel = parcel;
        Cost = cost;
    }
}
