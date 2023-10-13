using System.Text.Json.Serialization;

namespace DeliveryApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    User,
    Admin,
}