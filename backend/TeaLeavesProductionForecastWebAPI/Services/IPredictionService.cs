using TeaLeavesProductionForecastWebAPI.DTO;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public interface IPredictionService
    {
        Task<decimal> PredictionAsync(PredictionDto dto);
    }
}
