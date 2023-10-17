using DeliveryApi.Context;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Services.BasketService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;

[Route("/")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }
    
    [Authorize]
    [HttpGet("cart/")]
    [ProducesResponseType(typeof(List<BasketDTO>),200)]
    [ProducesResponseType(typeof(Response),500)]
    public async Task<IActionResult> GetUserBusket()
    {
        var token = JwtParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        return Ok(await _basketService.GetUserBasket(token));
    }

    [Authorize]
    [HttpGet("cart/{id}")]
    public async Task<IActionResult> AddToUserBasket(Guid id)
    {
        var token = JwtParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _basketService.AddToUserBasket(token,id);

        return Ok();
    }
    
    [Authorize]
    [HttpDelete("cart/{id}")]
    public async Task<IActionResult> DeleteFromUserBasket(Guid id, bool increase)
    {
        var token = JwtParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _basketService.DeleteFromUserBasket(token,id,increase);

        return Ok();
    }
}