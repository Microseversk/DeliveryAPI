using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DeliveryApi.Context;
using DeliveryApi.Enums;
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
    public async Task<IActionResult> AddDishes(List<DishDTO> model)
    {
        await _dishService.AddDishes(model);
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(DishesMenuResponse),200)]
    [ProducesResponseType(typeof(Response),400)]
    public async Task<IActionResult> GetDishes(DishCategory? category, bool vegeterian = false, DishSorting sortingBy = DishSorting.NameAsc,[Range(1,int.MaxValue)]int page = 1)
    {
        try
        {
            return Ok(await _dishService.GetDishMenu(category, vegeterian, sortingBy, page));
        }
        catch (Exception e)
        {
            return BadRequest(new Response { Message = e.Message });
            throw;
        }
    }

    [HttpGet("item/{id}")]
    [ProducesResponseType(typeof(DishDTO),200)]
    [ProducesResponseType(typeof(Response),400)]
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
    [ProducesResponseType(typeof(bool),200)]
    public async Task<IActionResult> GetUserRateOnDish(Guid id)
    {
        var token = Request.Headers["Authorization"].ToString();
        token = token.Substring("Bearer ".Length);
        bool response = (await _dishService.CheckUserRated(token, id) == null) ? false : true;
        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("item/{id}/rating/")]
    public async Task<IActionResult> PostUserRate(Guid id, double value)
    {
        var token = Request.Headers["Authorization"].ToString();
        token = token.Substring("Bearer ".Length);
        
        await _dishService.PutUserRating(token, id, value);
        
        return Ok();
    }
}