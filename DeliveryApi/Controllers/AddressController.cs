using DeliveryApi.Migrations;
using DeliveryApi.Models;
using DeliveryApi.Services.AddressService;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;
[Route("/address/")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<AddressDTO>),200)]
    [ProducesResponseType(typeof(Response),500)]
    public async Task<IActionResult> GetObjectChildren(int parentObjectId, string? query)
    {
        return Ok(await _addressService.GetObjectChildren(parentObjectId, query));
    }
    
    [HttpGet("getaddresschain")]
    [ProducesResponseType(typeof(List<AddressDTO>),200)]
    [ProducesResponseType(typeof(Response),500)]
    public async Task<IActionResult> GetAddressChain(int objectId)
    {
        return Ok(await _addressService.GetAddressChain(objectId));
    }

}