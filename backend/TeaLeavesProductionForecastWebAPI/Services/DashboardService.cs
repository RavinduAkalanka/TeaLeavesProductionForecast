using Microsoft.EntityFrameworkCore;
using TeaLeavesProductionForecastWebAPI.Data;
using TeaLeavesProductionForecastWebAPI.DTO;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;

        public DashboardService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<GetPredictionDto>> GetAllPredictionById(int userId)
        {
            var allPrediction = await (from p in _db.Predictions
                                       join u in _db.Users on p.UserId equals u.Id
                                       where p.UserId == userId
                                       select new GetPredictionDto
                                       {
                                           Id = p.Id,
                                           UserId = p.UserId,
                                           Year = p.Year,
                                           Month = p.Month,
                                           PlantCount = p.PlantCount,
                                           FertilizerType = p.FertilizerType,
                                           Pruning = p.Pruning,
                                           SoilPH = p.SoilPH,
                                           AvgRainfall = p.AvgRainfall,
                                           AverageTemperature = p.AverageTemperature,
                                           AvgHumidityPercent = p.AvgHumidityPercent,
                                           CreatedAt = p.CreatedAt,
                                           PredictionResult = p.PredictionResult,
                                           FullName = u.FullName,
                                           Estate = u.Estate
                                       }).ToListAsync();

            return allPrediction;
        }

        public async Task<UserResponseDto?> GetUserByIdForDashboardAsync(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Area = user.Area,
                Estate = user.Estate,
                Role = user.Role.ToString()
            };
        }

    }
}
