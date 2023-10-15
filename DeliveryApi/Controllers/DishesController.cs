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
    public async Task<ActionResult<DishesMenuResponse>> GetDishes(DishCategory? category, bool vegeterian = false, DishSorting sortingBy = DishSorting.NameAsc,[Range(1,int.MaxValue)]int page = 1)
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
}