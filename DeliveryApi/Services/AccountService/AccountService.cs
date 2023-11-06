using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure.Core;
using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Exceptions;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Validators;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeliveryApi.Services;

public class AccountService : IAccountService
{
    private readonly DeliveryContext _dContext;
    private readonly AddressContext _aContext;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenCreateHelper _tokenCreateHepler;

    public AccountService(DeliveryContext dContext, AddressContext aContext, IConfiguration configuration)
    {
        _dContext = dContext;
        _aContext = aContext;
        _configuration = configuration;

        string key = _configuration["JWTSettings:SecretKey"];
        string issuer = _configuration["JWTSettings:Issuer"];
        string audience = _configuration["JWTSettings:Audience"];
        double durationInMinute = double.Parse(_configuration["JWTSettings:DurationInMinute"]);

        _tokenCreateHepler = new JwtTokenCreateHelper(key, issuer, audience, durationInMinute);
    }

    public async Task<string> CreateUser(UserRegistrationDTO model)
    {
        var checkUser = await _dContext.User.FirstOrDefaultAsync(u => model.Email == u.Email);
        if (checkUser != null)
        {
            throw new BadRequestException("email data is already in use");
        }

        if (model.AddressId != null && !await Address.Isvalid(_aContext, model.AddressId))
        {
            throw new BadRequestException("Address not found");
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
        await _dContext.AddAsync(newUser);
        await _dContext.SaveChangesAsync();

        var token = _tokenCreateHepler.GenerateToken(newUser.Email, newUser.Role);

        return token;
    }

    public async Task<string> LoginUser(UserLoginDTO model)
    {
        var user = await _dContext.User.SingleOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            throw new BadRequestException("Bad email");
        }

        var verifyPassword = HashPasswordHelper.VerifyPassword(model.Password, user.HashedPassword);

        if (!verifyPassword)
        {
            throw new BadRequestException("Bad password");
        }

        Role role = (user.Role == Role.Admin) ? Role.Admin : Role.User;

        var token = _tokenCreateHepler.GenerateToken(model.Email, role);
        return token;
    }

    public async Task LogoutUser(string token)
    {
        if (await JwtTokenParseHelper.GetUserFromContext(token, _dContext) != null)
        {
            await _dContext.BannedTokens.AddAsync(new BannedToken{Token = token});
            await _dContext.SaveChangesAsync();    
        }
    }

    public async Task<UserProfileDTO> GetProfile(string token)
    {
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _dContext);
        if (user == null)
        {
            throw new BadRequestException("Invalid token");
        }
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
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _dContext);

        if (user == null)
        {
            throw new BadRequestException("Invalid token");
        }
        
        if (model.AddressId != null && !await Address.Isvalid(_aContext, model.AddressId))
        {
            throw new BadRequestException($@"Address {model.AddressId} not found");
        }

        user.FullName = model.FullName;
        user.AddressId = model.AddressId;
        user.BirthDate = model.BirthDate;
        user.Gender = model.Gender;
        user.Phone = model.Phone;
        await _dContext.SaveChangesAsync();
        return;
    }
}