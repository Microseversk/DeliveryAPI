using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeliveryApi.Helpers;

public static class JwtParseHelper
{
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