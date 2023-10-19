using DeliveryApi.Context;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Services.BasketService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;

[Authorize]
[Route("/")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet("cart/")]
    [ProducesResponseType(typeof(List<BasketDTO>), 200)]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<IActionResult> GetUserBusket()
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            return Ok(await _basketService.GetUserBasket(token));
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }
    
    [HttpGet("cart/{id}")]
    [ProducesResponseType(typeof(Response),500)]
    public async Task<IActionResult> AddToUserBasket(Guid id)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            await _basketService.AddToUserBasket(token, id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
        
    }

    [HttpDelete("cart/{id}")]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<IActionResult> DeleteFromUserBasket(Guid id, bool increase)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            await _basketService.DeleteFromUserBasket(token, id, increase);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }
}