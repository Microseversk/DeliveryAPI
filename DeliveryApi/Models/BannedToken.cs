using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Models;

public class BannedToken
{
    [Key]
    public Guid Id { get; set; }
    public string Token { get; set; }
}