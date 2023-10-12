using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class UserLogin
{
    [EmailAddress]
    public string Email { get; set; }
    
    [MinLength(1)]
    public string Password { get; set; }

}