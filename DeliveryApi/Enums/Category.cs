namespace DeliveryApi.Enums;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    Wok,
    Pizza,
    Soup,
    Desert,
    Drink
}