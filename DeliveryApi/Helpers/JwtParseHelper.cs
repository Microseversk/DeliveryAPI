using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeliveryApi.Context;
using DeliveryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace DeliveryApi.Helpers;

public static class JwtParseHelper
{
    public static string NormalizeToken(StringValues token)
    {
        var normingToken = token.ToString();
        return normingToken.Substring("Bearer ".Length);
    }

    public async static Task<UserDTO> GetUserFromContext(string token, DeliveryContext context)
    {
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