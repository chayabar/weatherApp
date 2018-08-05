using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DailyForeCast
    {
        public List<WeatherForecast> DayData { get; set; }
        public List<WeatherForecast> NightData { get; set; }
        public DateTime Date { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public double DayTemperature { get; set; }
        public double NightTemperature { get; set; }
        public string IconID { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public double WindSpeed { get; set; }
    }
}
