using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class UserProfile
{
    [MinLength(1)]
    public string FullName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public Guid? AddressId { get; set; }

    public DateTime? BirthDate { get; set; }

    public Gender Gender { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
}