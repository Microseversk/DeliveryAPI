using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryApi.Helpers;

public class JwtTokenHelper
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly double _durationInMinutes;

    public JwtTokenHelper(string key, string issuer, string audience, double durationInMinutes)
    {
        _key = key;
        _issuer = issuer;
        _audience = audience;
        _durationInMinutes = durationInMinutes;
    }

    public string GenerateToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.UTF8.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
            }),
            Expires = DateTime.Now.AddMinutes(_durationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _issuer,
            Audience = _audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}