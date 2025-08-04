using Microsoft.AspNetCore.Mvc;
using TeaLeavesProductionForecastWebAPI.DTO;
using TeaLeavesProductionForecastWebAPI.Services;

namespace TeaLeavesProductionForecastWebAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(dto);
                return Ok(new
                {
                    message = "User registered successfully",
                    user = new
                    {
                        user.Id,
                        user.FullName
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message,
                    details = ex.InnerException?.Message,
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var token = await _userService.LoginUserAsync(request);

            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new { token });
        }
    }    
}
