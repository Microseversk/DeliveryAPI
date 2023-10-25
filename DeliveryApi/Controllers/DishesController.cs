using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;

[Route("/")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishesController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("addDishes")]
    public async Task<IActionResult> AddDishes(List<Dish> model)
    {
        await _dishService.AddDishes(model);
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(DishesMenuResponse), 200)]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<IActionResult> GetDishes(DishCategory? category, bool vegeterian = false,
        DishSorting sortingBy = DishSorting.NameAsc, [Range(1, int.MaxValue)] int page = 1)
    {
        try
        {
            return Ok(await _dishService.GetDishMenu(category, vegeterian, sortingBy, page));
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }

    [HttpGet("item/{id}")]
    [ProducesResponseType(typeof(Dish), 200)]
    [ProducesResponseType(typeof(Response), 400)]
    public async Task<IActionResult> GetDishById(Guid id)
    {
        try
        {
            return Ok(await _dishService.GetDishById(id));
        }
        catch (Exception e)
        {
            return BadRequest(new Response { Message = e.Message });
        }
    }

    [Authorize]
    [HttpGet("item/{id}/rating/check")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<IActionResult> GetUserRateOnDish(Guid id)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            bool response = (await _dishService.CheckUserRated(token, id) == null) ? false : true;
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }

    [Authorize]
    [HttpPost("item/{id}/rating/")]
    [ProducesResponseType(typeof(Response),500)]
    public async Task<IActionResult> PostUserRate(Guid id,[Range(0,10)]double value)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            await _dishService.PutUserRating(token, id, value);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }
}