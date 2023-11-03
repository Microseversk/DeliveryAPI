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

[Route("/api/dish/")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishesController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(DishesMenuResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> GetDishes(DishCategory? category, bool vegeterian = false,
        DishSorting sortingBy = DishSorting.NameAsc, [Range(1, int.MaxValue)] int page = 1)
    {
        return Ok(await _dishService.GetDishMenu(category, vegeterian, sortingBy, page));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Dish), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> GetDishById(Guid id)
    {
        return Ok(await _dishService.GetDishById(id));
    }

    [Authorize]
    [HttpGet("{id}/rating/check")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> GetUserRateOnDish(Guid id)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        bool response = (await _dishService.CheckUserRated(token, id) == null) ? false : true;
        return Ok(response);
    }

    [Authorize]
    [HttpPost("{id}/rating")]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> PostUserRate(Guid id, [Range(0, 10)] double value)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _dishService.PutUserRating(token, id, value);
        return Ok();
    }
}