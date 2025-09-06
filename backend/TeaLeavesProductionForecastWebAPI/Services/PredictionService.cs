using System.Net.Http;
using System.Text.Json;
using System.Text;
using TeaLeavesProductionForecastWebAPI.Data;
using TeaLeavesProductionForecastWebAPI.DTO;
using TeaLeavesProductionForecastWebAPI.Model;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly AppDbContext _db;
        private readonly HttpClient _httpClient;

        public PredictionService(AppDbContext db, HttpClient httpClient)
        {
            _db = db;
            _httpClient = httpClient;
        }

        public async Task<decimal> PredictionAsync(PredictionDto dto)
        {
            var modelInput = new
            {
                dto.Year,
                dto.Month,
                dto.PlantCount,
                dto.FertilizerType,
                dto.Pruning,
                dto.SoilPH,
                dto.AvgRainfall,
                dto.AverageTemperature,
                dto.AvgHumidityPercent
            };

            var json = JsonSerializer.Serialize(modelInput);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Call Flask API
            var flaskUrl = "http://127.0.0.1:5000/predict";
            var response = await _httpClient.PostAsync(flaskUrl, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Flask API error: {response.StatusCode}");

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<PredictResponseDto>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            decimal predictionResult = responseObj?.PredictionResult ?? 0;

            // Save prediction to DB
            var prediction = new Prediction
            {
                UserId = dto.UserId,
                Year = dto.Year,
                Month = dto.Month,
                PlantCount = dto.PlantCount,
                FertilizerType = dto.FertilizerType,
                Pruning = dto.Pruning,
                SoilPH = dto.SoilPH,
                AvgRainfall = dto.AvgRainfall,
                AverageTemperature = dto.AverageTemperature,
                AvgHumidityPercent = dto.AvgHumidityPercent,
                PredictionResult = predictionResult,  
                CreatedAt = DateTime.Now
            };

            _db.Predictions.Add(prediction);
            await _db.SaveChangesAsync();

            return predictionResult;
        }
    }
}
