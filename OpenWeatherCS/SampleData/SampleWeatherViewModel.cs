using OpenWeatherCS.Models;
using System;
using System.Collections.Generic;

namespace OpenWeatherCS.SampleData
{
    public class SampleWeatherViewModel
    {

        public List<DailyForeCast> Forecast { get; set; }
        public DailyForeCast CurrentWeather { get; set; }

        public SampleWeatherViewModel()
        {
            CurrentWeather = new DailyForeCast()
            {
                Date = DateTime.Now,
                ID = 202,
                MaxTemperature = 34.12,
                MinTemperature = 20.45,
                IconID = "11d",
                DayTemperature = 23.55,
                NightTemperature = 18.35,
                Description = "thunderstorm with heavy rain ",
                WindSpeed = 1.85,
            };

            Forecast = new List<DailyForeCast>();
            Forecast.Add(new DailyForeCast()
            {
                Date = new DateTime(2017, 8, 3),
                ID = 310,
                MaxTemperature = 18.33,
                MinTemperature = 12.78,
                IconID = "09d",
                DayTemperature = 23,
                Description = "light intensity drizzle rain",
                WindSpeed = 1.85,
            });
            Forecast.Add(new DailyForeCast()
            {
                Date = new DateTime(2017, 8, 4),
                ID = 800,
                MaxTemperature = 34.65,
                MinTemperature = 20.32,
                IconID = "10d",
                DayTemperature = 23,
                Description = "clear day",
                WindSpeed = 1.85,
            });
        }
    }
}
