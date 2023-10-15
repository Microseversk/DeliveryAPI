using System.Text.Json.Serialization;

namespace DeliveryApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishSorting
{
    NameAsc,
    NameDesc,
    PriceAsc,
    PriceDesc,
    RatingAsc,
    RatingDesc,
}