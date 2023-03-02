using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoApp.Configuration;
using TodoApp.Models.DTOs.Requests;
using TodoApp.Models.DTOs.Responses;

namespace TodoApp.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthManagementController : ControllerBase
  {
    private readonly UserManager<IdentityUser>? _userManager;
    private readonly Configuration.JwtConfig? _jwtConfig;

    public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
    {
      _userManager = userManager;
      _jwtConfig = optionsMonitor.CurrentValue;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
    {
      if (ModelState.IsValid)
      {
        // We can utilise the model
        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser != null)
        {
          return BadRequest(new RegistrationResponse()
          {
            Errors = new List<string>() {
                "Email already in use"
            },
            Success = false
          });
        }
        var newUser = new IdentityUser() { Email = user.Email, UserName = user.Username };
      }

      return BadRequest(new RegistrationResponse()
      {
        Errors = new List<string>() {
            "Invalid payload"
        },
        Success = false
      });

    }
  }
}
