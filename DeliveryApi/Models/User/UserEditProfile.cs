using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class UserEditProfile
{
    [MinLength(1)]
    public string FullName { get; set; }

    public Guid? AddressId { get; set; }

    public DateTime? BirthDate { get; set; }

    public Gender Gender { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
}