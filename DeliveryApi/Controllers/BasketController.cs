using DeliveryApi.Context;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Services.BasketService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;

[Authorize]
[Route("/api/basket/")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(List<BasketDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> GetUserBusket()
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        return Ok(await _basketService.GetUserBasket(token));
    }

    [HttpGet("dish/{dishId}")]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> AddToUserBasket(Guid dishId)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _basketService.AddToUserBasket(token, dishId);
        return Ok();
    }

    [HttpDelete("dish/{dishId}")]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> DeleteFromUserBasket(Guid dishId, bool increase)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _basketService.DeleteFromUserBasket(token, dishId, increase);
        return Ok();
    }
}