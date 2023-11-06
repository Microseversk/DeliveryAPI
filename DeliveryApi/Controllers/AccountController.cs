using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;

[Route("/api/account/")]
[ApiController]
[ProducesResponseType(typeof(ErrorResponse), 400)]
[ProducesResponseType(typeof(ErrorResponse), 500)]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    public async Task<IActionResult> Register(UserRegistrationDTO model)
    {
        return Ok(new TokenResponse { Token = await _accountService.CreateUser(model) });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    public async Task<IActionResult> Login(UserLoginDTO model)
    {
        return Ok(new TokenResponse { Token = await _accountService.LoginUser(model) });
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _accountService.LogoutUser(token);
        return Ok();
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileDTO),200)]
    public async Task<ActionResult<UserProfileDTO>> GetProfile()
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        return Ok(await _accountService.GetProfile(token));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> EditProfile(UserEditProfileDTO model)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _accountService.EditProfile(token, model);
        return Ok();
    }
}