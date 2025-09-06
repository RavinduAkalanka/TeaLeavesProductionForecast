using Microsoft.AspNetCore.Mvc;
using TeaLeavesProductionForecastWebAPI.Services;

namespace TeaLeavesProductionForecastWebAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("all-predictions/{userId}")]
        public async Task<IActionResult> GetAllPredictionsByUser(int userId)
        {
            var predictions = await _dashboardService.GetAllPredictionById(userId);

            if (predictions == null || !predictions.Any())
                return NotFound(new { message = "No predictions found for this user." });

            return Ok(predictions);
        }


        [HttpGet("dashboard-user/{userId}")]
        public async Task<IActionResult> GetUserByIdForDashboard(int userId)
        {
            var user = await _dashboardService.GetUserByIdForDashboardAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }
    }
}
