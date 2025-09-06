using TeaLeavesProductionForecastWebAPI.DTO;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public interface IDashboardService
    {
        Task<List<GetPredictionDto>> GetAllPredictionById(int userId);
        Task<UserResponseDto?> GetUserByIdForDashboardAsync(int userId);
    }
}
