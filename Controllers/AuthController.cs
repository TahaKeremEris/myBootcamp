using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BootcampDay1.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthService _authService;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuthService authService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return Unauthorized();

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (result.Succeeded)
        {
            var token = _authService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}
