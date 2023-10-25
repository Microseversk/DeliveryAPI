using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DeliveryApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HouseType
{
    [Description("Void")] Void,
    [Description("Владение")] Ownership,
    [Description("Дом")] House,
    [Description("Домовладение")] Household,
    [Description("Гараж")] Garage,
    [Description("Здание")] Building,
    [Description("Шахта")] Mine,
    [Description("Строение")] Structure,
    [Description("Сооружение")] Facility,
    [Description("Литера")] Letter,
    [Description("Корпус")] Corpus,
    [Description("Подвал")] Basement,
    [Description("Котельная")] BoilerRoom,
    [Description("Погреб")] Cellar,
    [Description("Объект незавершенного строительства")] ConstructionInProgress
}