using DeliveryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Services;

public interface IAccountService
{
    public Task<string> CreateUser(UserRegistration model);
    public Task<string> LoginUser(UserLogin model);
    public Task<UserProfile> GetProfile(string token);

    public Task EditProfile(string token, UserEditProfile model);
}