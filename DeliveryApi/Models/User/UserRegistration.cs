using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;
using DeliveryApi.Validators;

namespace DeliveryApi.Models;

public class UserRegistration
{
    [MinLength(1)]
    public string FullName { get; set; }

    [MinLength(6)]
    public string Password { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public Guid? AddressId { get; set; }

    [BirthDate]
    public DateTime? BirthDate { get; set; }

    public Gender Gender { get; set; }
    
    [PhoneRussia]
    public string? Phone { get; set; }
}