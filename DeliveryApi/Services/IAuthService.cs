using DeliveryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Services;

public interface IAuthService
{
    public Task<string> CreateUser(UserRegistration model);
    public Task<string> LoginUser(UserLogin model);
}