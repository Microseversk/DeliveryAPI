using System.Security.Claims;
using System.Text;
using DeliveryApi.Context;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Jwt Settings Config
var secretKey = builder.Configuration.GetSection("JWTSettings:SecretKey").Value;
var issuer = builder.Configuration.GetSection("JWTSettings:Issuer").Value;
var audience = builder.Configuration.GetSection("JWTSettings:Audience").Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add Services
builder.Services.AddScoped<IAuthService, AuthService>();

//Add Auth
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        LifetimeValidator = (before, expires, token, parameters) =>
        {
            var utcNow = DateTime.UtcNow;
            return before <= utcNow && utcNow <= expires;
        },
        IssuerSigningKey = signingKey,
        ValidateIssuerSigningKey = true,
    };
});

//Add Policy
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AdminOnly", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));
});
// Connect to DB
builder.Services.AddDbContext<DeliveryContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DeliveryConnection"));
});

builder.Services.AddDbContext<AddressContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AddressConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();