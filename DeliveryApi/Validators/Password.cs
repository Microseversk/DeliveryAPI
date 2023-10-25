using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DeliveryApi.Validators;

public class Password : ValidationAttribute
{
    public Password()
    {
        ErrorMessage = "Password min length 6 and contain min 1 digit";
    }

    public override bool IsValid(object value)
    {
        int minLength = 6;
        
        if (value is string && value.ToString().Length >= minLength && Regex.IsMatch(value.ToString(), @"\d"))
        {
            return true;
        }
        return false;
    }
}