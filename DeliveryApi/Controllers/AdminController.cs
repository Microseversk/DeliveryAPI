using DeliveryApi.Context;
using DeliveryApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Controllers;

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
    
    [HttpGet("{guid}")]
    public async Task<ActionResult<List<UserDTO>>> GetUserList(Guid id)
    {
        return Ok(await _context.Users.FirstOrDefaultAsync(u => u.Id == id));
    }

    [HttpGet]
    public async Task<ActionResult> DeleteUsers()
    {
        _context.Users.RemoveRange(_context.Users.ToList());
        await _context.SaveChangesAsync();
        return Ok("all users were deleted");
    }
}