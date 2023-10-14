using System.Text.Json;
using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Models;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;

[Route("api/[action]")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishesController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpPost]
    public async Task<IActionResult> AddDish(List<DishDTO> model)
    {
        await _dishService.AddDishes(model);
        return Ok();
    }

    /*[HttpGet]
    public async Task<IActionResult> GetDishes(int page, DishCategory category,
        bool vegeterian, DishSorting sortingBy)
    {
        page = (page == null) ? 1 : page;
        var menu = await _dishService.GetDishMenu(category, vegeterian, sortingBy, page);
        return Ok(menu);
    }*/
}