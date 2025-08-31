namespace TeaLeavesProductionForecastWebAPI.Model
{
    public class Prediction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int  PlantCount { get; set; }
        public string FertilizerType { get; set; }
        public string Pruning { get; set; }
        public decimal SoilPH { get; set; }
        public decimal AvgRainfall { get; set; }
        public decimal  AverageTemperature { get; set; }
        public decimal AvgHumidityPercent { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public decimal PredictionResult { get; set; }
    }
}
