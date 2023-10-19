using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeliveryApi.Context;
using DeliveryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace DeliveryApi.Helpers;

public static class JwtTokenParseHelper
{
    public static string NormalizeToken(StringValues token)
    {
        var normingToken = token.ToString();
        return normingToken.Substring("Bearer ".Length);
    }

    private static async Task<bool> CheckToken(string token, DeliveryContext context)
    {
        bool isBanned = await context.BannedTokens.FirstOrDefaultAsync(t => t.Token == token) != null ? true : false ;
        return isBanned;
    }

    public static async Task<UserDTO> GetUserFromContext(string token, DeliveryContext context)
    {
        if (await CheckToken(token, context) == true)
        {
            throw new Exception(message: $@"Token is banned\n{token}");
        }
        var userEmail = GetClaimValue(token, ClaimTypes.Email);
        var user = await context.User.FirstOrDefaultAsync(user => user.Email == userEmail);
        return user;
    }
    public static string GetClaimValue(string token, string claimType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = tokenHandler.ReadJwtToken(token);
        if (securityKey == null)
        {
            return null;
        }
        var claimValue = securityKey.Claims.FirstOrDefault(c => c.Type == claimType).Value;
        return claimValue;
    }
}