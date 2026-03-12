using ParcelCostCalculator.Enums;
using ParcelCostCalculator.Models;

namespace ParcelCostCalculatorTests.Data;

public static class TestData
{
    public static IDictionary<ParcelTypeEnum, Parcel> ParcelCostMappingTestData => new Dictionary<ParcelTypeEnum, Parcel>
    {
        { ParcelTypeEnum.Small, new Parcel(1, 2, 3) },
        { ParcelTypeEnum.Medium, new Parcel(10, 12, 13) },
        { ParcelTypeEnum.Large, new Parcel(50, 51, 52) },
        { ParcelTypeEnum.ExtraLarge, new Parcel(100, 51, 10) }
    };
}
