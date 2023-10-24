using System.ComponentModel.DataAnnotations;

namespace DeliveryApi.Validators;

public class BirthDate : ValidationAttribute
{
    public BirthDate()
    {
        ErrorMessage = $@"birthDate <= {DateTime.Now.AddYears(-3)} and birthDate >= {DateTime.Now.AddYears(-100)}";
    }
    public override bool IsValid(object value)
    {
        if (value is DateTime birthDate)
        {
            var now = DateTime.UtcNow.AddYears(-3);
            var minDate = DateTime.UtcNow.AddYears(-100);
            return birthDate <= now && birthDate >= minDate;
        }

        if (value is null)
        {
            return true;
        }

        return false;
    }
}