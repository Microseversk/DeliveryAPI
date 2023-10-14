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
            var now = DateTime.Now.AddYears(-3);
            var minDate = DateTime.Now.AddYears(-100);
            return birthDate <= now && birthDate >= minDate;
        }

        return false;
    }
}