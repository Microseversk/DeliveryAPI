using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("api/admin/[action]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly DeliveryContext _context;

    public AdminController(DeliveryContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetUserList()
    {
        return Ok(await _context.Users.ToListAsync());
    }
    
    [HttpGet]
    public async Task<ActionResult> DeleteUsers()
    {
        var usersToDelete = _context.Users.Where(u => u.Role != Role.Admin);
        _context.RemoveRange(usersToDelete);
        await _context.SaveChangesAsync();
        return Ok("all users were deleted");
    }
}