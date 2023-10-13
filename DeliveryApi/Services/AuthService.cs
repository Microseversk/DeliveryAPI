using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeliveryApi.Services;

public class AuthService : IAuthService
{
    private readonly DeliveryContext _context;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenHelper _tokenHepler; 
    public AuthService(DeliveryContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        
        string key = _configuration["JWTSettings:SecretKey"];
        string issuer = _configuration["JWTSettings:Issuer"];
        string audience = _configuration["JWTSettings:Audience"];
        double durationInMinute = double.Parse(_configuration["JWTSettings:DurationInMinute"]);
        
        _tokenHepler = new JwtTokenHelper(key,issuer,audience,durationInMinute);
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
        
        var token = _tokenHepler.GenerateToken(newUser.Email, newUser.Role);
        
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

        Role role = (user.Role == Role.Admin) ? Role.Admin : Role.User;

        var token = _tokenHepler.GenerateToken(model.Email, role);
        return token;
    }
}