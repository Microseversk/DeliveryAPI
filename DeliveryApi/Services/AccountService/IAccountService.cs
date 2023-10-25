using DeliveryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Services;

public interface IAccountService
{
    public Task<string> CreateUser(UserRegistrationDTO model);
    public Task<string> LoginUser(UserLoginDTO model);
    public Task LogoutUser(string token);
    public Task<UserProfileDTO> GetProfile(string token);
    public Task EditProfile(string token, UserEditProfileDTO model);
}