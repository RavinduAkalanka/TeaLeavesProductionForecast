export interface Prediction {
  userId: number;
  year: number;
  month: number;
  plantCount: number;
  fertilizerType: string;
  pruning: string;
  soilPH: number;
  avgRainfall: number;
  averageTemperature: number;
  avgHumidityPercent: number;
}
