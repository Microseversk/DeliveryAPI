using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class User
{
    [Key]
    public string Email { get; set; }
    
    public string FullName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public Gender Gender { get; set; }
    
    public string Phone { get; set; }
    
    public string Address { get; set; }
}