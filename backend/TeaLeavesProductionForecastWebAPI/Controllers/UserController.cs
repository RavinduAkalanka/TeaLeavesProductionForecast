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

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);

            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = result });
        }


        [HttpPost("send-reset-password")]
        public async Task<IActionResult> SendResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null || request.UserId <= 0)
                return BadRequest(new { message = "Invalid request" });

            var result = await _userService.SendResetPasswordEmailAsync(request.UserId);

            if (result)
                return Ok(new { message = "OTP sent to your email" });

            return BadRequest(new { message = "Failed to send OTP" });
        }

        [HttpPost("verify-reset-password-otp")]
        public async Task<IActionResult> VerifyResetPasswordOtp([FromBody] ResetPasswordOtpRequest request)
        {
            if (request == null || request.UserId <= 0 || string.IsNullOrEmpty(request.Otp) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest(new { message = "Invalid request" });

            try
            {
                var success = await _userService.VerifyOtpAndResetPasswordAsync(request.UserId, request.Otp, request.NewPassword);

                if (success)
                    return Ok(new { message = "Password reset successfully" });

                return BadRequest(new { message = "Failed to reset password" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("send-reset-password-by-email")]
        public async Task<IActionResult> SendResetPasswordByEmail([FromBody] ResetPasswordRequestWithEmail request)
        {
            string email = request?.Email;  
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email is required" });

            var result = await _userService.SendResetPasswordEmailByEmailAsync(email);

            if (result)
                return Ok(new { message = "OTP sent to your email" });

            return BadRequest(new { message = "Email not found or failed to send OTP" });
        }

        [HttpPost("verify-reset-password-emailwith-otp")]
        public async Task<IActionResult> VerifyResetPasswordEmailWithOtp([FromBody] ResetPasswordOtpRequestWithEmail request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Otp) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest(new { message = "Invalid request" });

            try
            {
                var success = await _userService.VerifyOtpAndResetPasswordByEmailAsync(
                    request.Email, request.Otp, request.NewPassword);

                if (success)
                    return Ok(new { message = "Password reset successfully" });

                return BadRequest(new { message = "Failed to reset password" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
