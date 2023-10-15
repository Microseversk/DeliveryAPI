namespace DeliveryApi.Enums;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Dessert,
    Drink
}