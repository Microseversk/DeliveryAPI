using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;
using DeliveryApi.Validators;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public Role Role { get; set; } = Role.User;

    [EmailAddress]
    public string Email { get; set; }

    [MinLength(1)]
    public string FullName { get; set; }
    
    [BirthDate]
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    [PhoneRussia]
    public string? Phone { get; set; }
    public Guid? AddressId { get; set; }
    public string HashedPassword { get; set; }
}