using System.Text.Json.Serialization;

namespace DeliveryApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    InProcess,
    Delivered
}