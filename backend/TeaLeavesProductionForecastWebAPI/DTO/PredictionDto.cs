namespace TeaLeavesProductionForecastWebAPI.DTO
{
    public class PredictionDto
    {
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int PlantCount { get; set; }
        public string FertilizerType { get; set; }
        public string Pruning { get; set; }
        public decimal SoilPH { get; set; }
        public decimal AvgRainfall { get; set; }
        public decimal AverageTemperature { get; set; }
        public decimal AvgHumidityPercent { get; set; }
    }

    public class PredictResponseDto
    {
        public decimal PredictionResult { get; set; }
    }

    public class GetPredictionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int PlantCount { get; set; }
        public string FertilizerType { get; set; }
        public string Pruning { get; set; }
        public decimal SoilPH { get; set; }
        public decimal AvgRainfall { get; set; }
        public decimal AverageTemperature { get; set; }
        public decimal AvgHumidityPercent { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public decimal PredictionResult { get; set; }
        public string FullName { get; set; }
        public string Estate { get; set; }
    }
}
