using DeliveryApi.Context;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Services;

public class AuthService : IAuthService
{
    private readonly DeliveryContext _context;
    private readonly JwtTokenHelper _tokenHepler = new("key for token_asdasdjasdaskjdhaskjdhasdkjlahsdkjdlashlskdfvncmvnb xcmvbxwejcsldkjdchwiepchipweujhcwi", "Sasha", "all", 20); 
    public AuthService(DeliveryContext context)
    {
        _context = context;
    }
    public async Task<string> CreateUser(UserRegistration model)
    {
        var checkUser = await _context.Users.FirstOrDefaultAsync(u => model.Email == u.Email);
        if (checkUser != null)
        {
            return "email data is already in use";
        }
        UserDTO newUser = new UserDTO
        {
            Id = Guid.NewGuid(),
            Email = model.Email,
            FullName = model.FullName,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            Phone = model.Phone,
            AddressId = model.AddressId,
            HashedPassword = HashPasswordHelper.HashPassword(model.Password)
        };
        await _context.AddAsync(newUser);
        await _context.SaveChangesAsync();
        
        var token = _tokenHepler.GenerateToken(model.Email);
        
        return token;
    }

    public async Task<string> LoginUser(UserLogin model)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
        {
            return $"User: {user.Email} Not Found";
        }

        var verifyPassword = HashPasswordHelper.VerifyPassword(model.Password, user.HashedPassword);
        if (!verifyPassword)
        {
            return "bad password";
        }

        var token = _tokenHepler.GenerateToken(model.Email);
        return token;
    }
}