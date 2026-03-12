using System.Diagnostics.CodeAnalysis;

namespace ParcelCostCalculator.Models;

public class WeightedParcel : Parcel
{
    /// <summary>
    /// Weight of the parcel in kilograms. Must be a positive integer.
    /// </summary>
    public required int Weight { get; init; }

    [SetsRequiredMembers]
    public WeightedParcel(int length, int width, int height, int weight) : base(length, width, height)
    {

        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Weigth must be a positive integer.");
        }

        Weight = weight;
    }

    [SetsRequiredMembers]
    public WeightedParcel(Parcel parcel, int weight) : base(parcel.Length, parcel.Width, parcel.Height)
    {
        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Weigth must be a positive integer.");
        }

        Weight = weight;
    }
}