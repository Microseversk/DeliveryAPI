using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DeliveryApi.Validators;

public class PhoneRussia : ValidationAttribute
{
    public PhoneRussia()
    {
        ErrorMessage = "Phone mask -> '7 xxx xxx xx xx' ";
    }

    public override bool IsValid(object value)
    {
        if (value is string phoneNumber)
        {
            var pattern = @"^7\d{3}\d{3}\d{2}\d{2}$";
            return Regex.IsMatch(phoneNumber,pattern);
        }
        return false;
    }
}