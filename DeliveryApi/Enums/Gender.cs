namespace DeliveryApi.Enums;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Man,
    Woman
}