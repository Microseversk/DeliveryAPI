using System.ComponentModel;
using System.Reflection;
using Npgsql.PostgresTypes;

namespace DeliveryApi.Helpers;

public static class EnumHelper
{
    public static string GetDescription(Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
        }

        return value.ToString();
    }
}