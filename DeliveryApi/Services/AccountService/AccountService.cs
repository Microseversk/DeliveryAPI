using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure.Core;
using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeliveryApi.Services;

public class AccountService : IAccountService
{
    private readonly DeliveryContext _context;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenCreateHelper _tokenCreateHepler;

    public AccountService(DeliveryContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;

        string key = _configuration["JWTSettings:SecretKey"];
        string issuer = _configuration["JWTSettings:Issuer"];
        string audience = _configuration["JWTSettings:Audience"];
        double durationInMinute = double.Parse(_configuration["JWTSettings:DurationInMinute"]);

        _tokenCreateHepler = new JwtTokenCreateHelper(key, issuer, audience, durationInMinute);
    }

    public async Task<string> CreateUser(UserRegistrationDTO model)
    {
        var checkUser = await _context.User.FirstOrDefaultAsync(u => model.Email == u.Email);
        if (checkUser != null)
        {
            throw new Exception(message: "email data is already in use");
        }

        User newUser = new User
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

        var token = _tokenCreateHepler.GenerateToken(newUser.Email, newUser.Role);

        return token;
    }

    public async Task<string> LoginUser(UserLoginDTO model)
    {
        var user = await _context.User.SingleOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            throw new Exception(message: "Bad email");
            return null;
        }

        var verifyPassword = HashPasswordHelper.VerifyPassword(model.Password, user.HashedPassword);

        if (!verifyPassword)
        {
            throw new Exception(message: "Bad password");
            return null;
        }

        Role role = (user.Role == Role.Admin) ? Role.Admin : Role.User;

        var token = _tokenCreateHepler.GenerateToken(model.Email, role);
        return token;
    }

    public async Task LogoutUser(string token)
    {
        await _context.BannedTokens.AddAsync(new BannedToken{Token = token});
        await _context.SaveChangesAsync();
    }

    public async Task<UserProfileDTO> GetProfile(string token)
    {
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _context);

        return new UserProfileDTO
        {
            FullName = user.FullName,
            Email = user.Email,
            AddressId = user.AddressId,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Phone = user.Phone
        };
    }

    public async Task EditProfile(string token, UserEditProfileDTO model)
    {
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _context);

        if (user == null)
        {
            return;
        }

        user.FullName = model.FullName;
        user.AddressId = model.AddressId;
        user.BirthDate = model.BirthDate;
        user.Gender = model.Gender;
        user.Phone = model.Phone;
        await _context.SaveChangesAsync();
        return;
    }
}