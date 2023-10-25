using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DeliveryApi.Enums;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryApi.Helpers;

public class JwtTokenCreateHelper
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly double _durationInMinutes;

    public JwtTokenCreateHelper(string key, string issuer, string audience, double durationInMinutes)
    {
        _key = key;
        _issuer = issuer;
        _audience = audience;
        _durationInMinutes = durationInMinutes;
    }

    public string GenerateToken(string email, Role role)
    {

        var claims = new List<Claim>{new Claim(ClaimTypes.Email, email), new Claim(ClaimTypes.Role, role.ToString())};
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var jwtToken = new JwtSecurityToken(
            issuer:_issuer,
            audience:_audience,
            claims:claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_durationInMinutes),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
    
}