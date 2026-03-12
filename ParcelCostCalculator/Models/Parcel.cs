using ParcelCostCalculator.Enums;

namespace ParcelCostCalculator.Models;

public class Parcel
{
    public required int Length { get; init; }

    public required int Width { get; init; }

    public required int Height { get; init; }

    public Parcel(int length, int width, int height)
    {
        Length = length;
        Width = width;
        Height = height;
    }

    public ParcelTypeEnum Type => ClassifyParcelType();

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