using DeliveryApi.Context;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add Services
builder.Services.AddScoped<IAuthService, AuthService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();