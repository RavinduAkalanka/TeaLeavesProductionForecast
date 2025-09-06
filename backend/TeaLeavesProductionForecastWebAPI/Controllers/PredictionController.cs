using Microsoft.AspNetCore.Mvc;
using TeaLeavesProductionForecastWebAPI.DTO;
using TeaLeavesProductionForecastWebAPI.Services;

namespace TeaLeavesProductionForecastWebAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        public PredictionController(IPredictionService predictionrService)
        {
            _predictionService = predictionrService;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> GetPrediction([FromBody] PredictionDto dto)
        {
            try
            {
                var predictionResult = await _predictionService.PredictionAsync(dto);

                return Ok(new PredictResponseDto { PredictionResult = predictionResult });
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
    }
}
