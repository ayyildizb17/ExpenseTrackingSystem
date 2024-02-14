using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ExpenseTrackingSystem.Entities.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/Admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<CustomUserRole> _roleManager;

    public AdminController(UserManager<CustomUser> userManager, RoleManager<CustomUserRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("CreateManager")]
   // [Authorize(Policy = "SysAdminPolicy")]
    public async Task<IActionResult> CreateManager([FromQuery] string username, [FromQuery] string password)
    {
       // _ = (User.FindFirst("id")?.Value);
        var role = await _roleManager.FindByNameAsync("Manager");
        var user = new Manager { UserName = username, Id = Guid.NewGuid().ToString(), RoleId = role.Id };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return Ok("User created successfully.");
        }

        return BadRequest("Failed to create user.");
    }

    [HttpPost("CreateEmployee")]
    public async Task<IActionResult> CreateEmployee(string username, string password, string managerUserName)
    {
        var role = await _roleManager.FindByNameAsync("Employee");
        var manager = await _userManager.FindByNameAsync(managerUserName);
        var user = new Employee { UserName = username, Id = Guid.NewGuid().ToString(), RoleId = role.Id, ManagerId = manager.Id };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return Ok("User created successfully.");
        }

        return BadRequest("Failed to create user.");
    }


}
