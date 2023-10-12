using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Models;

public class UserDTO
{
    [Key]
    public Guid Id { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [MinLength(1)]
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    
    public Gender Gender { get; set; }

    [Phone]
    public string? Phone { get; set; }
    
    public Guid? AddressId { get; set; }
    
    public string HashedPassword { get; set; }
}