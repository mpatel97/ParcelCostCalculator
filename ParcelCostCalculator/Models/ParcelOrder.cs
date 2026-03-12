using System.Diagnostics.CodeAnalysis;

namespace ParcelCostCalculator.Models;

public class ParcelOrder
{
    public IList<Parcel> Parcels { get; init; }

    public bool IsSpeedyShipping { get; init; }

    [SetsRequiredMembers]
    public ParcelOrder(IList<Parcel> parcels, bool isSpeedyShipping = false)
    {
        ArgumentNullException.ThrowIfNull(parcels);

        Parcels = parcels;
        IsSpeedyShipping = isSpeedyShipping;
    }
}
