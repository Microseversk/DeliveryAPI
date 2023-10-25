using DeliveryApi.Context;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Services.OrderService;
using DeliveryApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DeliveryApi.Controllers;

[Route("/")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IConfiguration _configuration;

    public OrderController(IOrderService orderService, IConfiguration configuration)
    {
        _orderService = orderService;
        _configuration = configuration;
    }

    [Authorize]
    [HttpGet("order/{id}")]
    [ProducesResponseType(typeof(OrderDTO),200)]
    [ProducesResponseType(typeof(Response),500)]
    public async Task<IActionResult> GetOrderInfo(Guid id)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            return Ok(await _orderService.GetOrderInfo(token, id));
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }

    [Authorize]
    [HttpGet("order")]
    [ProducesResponseType(typeof(Response),500)]
    [ProducesResponseType(typeof(List<OrderInfoDTO>),200)]
    public async Task<IActionResult> GetOrderList()
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            return Ok(await _orderService.GetOrderList(token));
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }

    [Authorize]
    [HttpPost("order")]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<IActionResult> CreateOrder(OrderCreateDTO model)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        if (new DeliveryTime(_configuration).IsValid(model.DeliveryTime) == false)
        {
            return BadRequest(new Response { Message = "Incorrect deliveryTime" });
        }

        try
        {
            await _orderService.CreateOrder(token, model);
        }
        catch (Exception e)
        {
            return StatusCode(400, new Response { Message = e.Message });
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("order/{id}/status")]
    public async Task<IActionResult> ConfirmOrder(Guid id)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);

        try
        {
            await _orderService.ConfirmOrder(token, id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }

    
}