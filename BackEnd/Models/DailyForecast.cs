namespace BackEnd.Models
{
    public class DailyForecast
	{
        public DateTime Date { get; set; }
        public string Summary { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public double MaxUVIndex { get; set; }
        public double MaxWindSpeed { get; set; }
    }
}

