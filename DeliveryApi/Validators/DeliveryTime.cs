using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace DeliveryApi.Validators;

public class DeliveryTime : ValidationAttribute
{
    private readonly IConfiguration _configuration;

    public DeliveryTime(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public override bool IsValid(object value)
    {
        if (value is DateTime delTime)
        {
            var now = DateTime.UtcNow;
            var minDeliveryTime = now.AddMinutes(double.Parse(_configuration["Time:DeliveryTime"]));
            return delTime >= minDeliveryTime;
        }

        return false;
    }
}