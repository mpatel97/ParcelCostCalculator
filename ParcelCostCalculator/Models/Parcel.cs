using ParcelCostCalculator.Enums;
using System.Diagnostics.CodeAnalysis;

namespace ParcelCostCalculator.Models;

public class Parcel
{
    /// <summary>
    /// Length of the parcel in centimeters. Must be a positive integer.
    /// </summary>
    public required int Length { get; init; }

    /// <summary>
    /// Width of the parcel in centimeters. Must be a positive integer.
    /// </summary>
    public required int Width { get; init; }

    /// <summary>
    /// Height of the parcel in centimeters. Must be a positive integer.
    /// </summary>
    public required int Height { get; init; }

    [SetsRequiredMembers]
    public Parcel(int length, int width, int height)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive integer.");
        }

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be a positive integer.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be a positive integer.");
        }

        Length = length;
        Width = width;
        Height = height;
    }

    public ParcelTypeEnum Type => ClassifyParcelType();

    // Parcel type is in the same domain as Parcel, so it makes sense to keep the classification logic here.
    // If the classification logic becomes more complex, we can consider refactoring it into a separate service or utility class.
    private ParcelTypeEnum ClassifyParcelType()
    {
        var allDimensions = new[] { Length, Width, Height };

        if (allDimensions.All(d => d < 10))
        {
            return ParcelTypeEnum.Small;
        }
        else if (allDimensions.All(d => d < 50))
        {
            return ParcelTypeEnum.Medium;
        }
        else if (allDimensions.All(d => d < 100))
        {
            return ParcelTypeEnum.Large;
        }

        // At this stage, any dimension past 100cm is classified as ExtraLarge
        return ParcelTypeEnum.ExtraLarge;
    }
}