using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenWeatherCS.Models;
using System.Net.Http;
using System.IO;
using System.Xml.Linq;
using OpenWeatherCS.Utils;
using System.Net;
using System.Diagnostics;

namespace OpenWeatherCS.Services
{
    public class OpenWeatherMapService : IWeatherService
    {
        private const string APP_ID = "b6690a95607edce36670e8056ede35b3";
        private const int MAX_FORECAST_DAYS = 40;
        private HttpClient client;

        public OpenWeatherMapService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
        }

        public async Task<IEnumerable<DailyForeCast>> GetForecastAsync(string location, int days)
        {
            if (location == null) throw new ArgumentNullException("Location can't be null.");
            if (location == string.Empty) throw new ArgumentException("Location can't be an empty string.");
            if (days <= 0) throw new ArgumentOutOfRangeException("Days should be greater than zero.");
            if (days > MAX_FORECAST_DAYS) throw new ArgumentOutOfRangeException($"Days can't be greater than {MAX_FORECAST_DAYS}");

            var query = $"forecast?q={location}&type=accurate&mode=xml&units=metric&APPID={APP_ID}"; //&cnt={days}
            var response = await client.GetAsync(query);            

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedApiAccessException("Invalid API key.");
                case HttpStatusCode.NotFound:
                    throw new LocationNotFoundException("Location not found.");
                case HttpStatusCode.OK:
                    var s = await response.Content.ReadAsStringAsync();
                    var x = XElement.Load(new StringReader(s));
                    DateTime sunRise = DateTime.Parse(x.Element("sun").Attribute("rise").Value);
                    DateTime sunSet = DateTime.Parse(x.Element("sun").Attribute("set").Value);

                    var primaryData = x.Descendants("time").Select(w => new WeatherForecast
                    {
                        Description = w.Element("symbol").Attribute("name").Value,
                        ID = int.Parse(w.Element("symbol").Attribute("number").Value),
                        IconID = w.Element("symbol").Attribute("var").Value,
                        Date = (DateTime.Parse(w.Attribute("from").Value)),
                        WindType = w.Element("windSpeed").Attribute("name").Value,
                        WindSpeed = double.Parse(w.Element("windSpeed").Attribute("mps").Value),
                        WindDirection = w.Element("windDirection").Attribute("code").Value,
                        Temperature = double.Parse(w.Element("temperature").Attribute("value").Value),
                        MaxTemperature = double.Parse(w.Element("temperature").Attribute("max").Value),
                        MinTemperature = double.Parse(w.Element("temperature").Attribute("min").Value),
                        Pressure = double.Parse(w.Element("pressure").Attribute("value").Value),
                        Humidity = double.Parse(w.Element("humidity").Attribute("value").Value)
                    }).ToList();
                    var data =getDailyForeCast(primaryData, sunRise, sunSet);
                    return data;
                default:
                    throw new NotImplementedException(response.StatusCode.ToString());
            }           
        }

        private IEnumerable<DailyForeCast> getDailyForeCast(List<WeatherForecast> primaryData, DateTime sunRise, DateTime sunSet)
        {
            List<DailyForeCast> data=new List<DailyForeCast>();
            List<WeatherForecast> dayData=new List<WeatherForecast>();
            List<WeatherForecast> nightData=new List<WeatherForecast>();
            DateTime current = primaryData[0].Date;
            foreach (WeatherForecast myforecast in primaryData)
            {
                if(myforecast.Date.Day!=current.Day)
                {
                    List<WeatherForecast> allData = dayData.Concat(nightData).ToList();
                    data.Add(new DailyForeCast {
                        Description = commonDescription(allData),
                        IconID = commonIcon(allData),
                        DayTemperature = avgTemp(dayData),
                        WindSpeed = avgWind(allData),
                        MaxTemperature = MinMaxTemp(dayData, nightData)[1],
                        MinTemperature = MinMaxTemp(dayData, nightData)[0],
                        NightTemperature = avgTemp(nightData),
                        Date = current,
                        ID=commonID(allData),
                        DayData = dayData,
                        NightData = nightData
                    });
                    current = myforecast.Date;
                    dayData.Clear();
                    nightData.Clear();
                }
                if ( myforecast.Date.Hour >= (Math.Round(sunRise.Hour / 3.0) * 3) && myforecast.Date.Hour < (Math.Round(sunSet.Hour / 3.0) * 3))
                    dayData.Add(myforecast);
                else 
                    nightData.Add(myforecast);

            }
            return data.AsEnumerable();
        }

        private int commonID(List<WeatherForecast> dayData)
        {
            List<int> id = dayData.Select(x => x.ID).ToList();
            var groupsWithCounts = from s in id
                                   group s by s into g
                                   select new
                                   {
                                       Item = g.Key,
                                       Count = g.Count()
                                   };

            var groupsSorted = groupsWithCounts.OrderByDescending(g => g.Count);
            return groupsSorted.First().Item;

        }

        private double[] MinMaxTemp(List<WeatherForecast> dayData, List<WeatherForecast> nightData)
        {
            double[] MinMax_temp = new double[2] { 10000, -500 };
            dayData.AddRange(nightData);
            foreach (WeatherForecast wf in dayData)
            {
                if (wf.MinTemperature < MinMax_temp[0])
                    MinMax_temp[0] = wf.MinTemperature;
                if (wf.MaxTemperature > MinMax_temp[1])
                    MinMax_temp[1] = wf.MaxTemperature;
            }
            return MinMax_temp;
        }

        private double avgWind(List<WeatherForecast> dayData)
        {
            return (dayData.Select(x => x.WindSpeed)).Average();
        }

        private double avgTemp(List<WeatherForecast> dayData)
        {
            if (dayData.Count() == 0)
                return 0;
            return (dayData.Select(x => x.Temperature)).Average();
        }

        private string commonIcon(List<WeatherForecast> dayData)
        {
            List<string> icon= dayData.Select(x => x.IconID).ToList();
            var groupsWithCounts = from s in icon
                                   group s by s into g
                                   select new
                                   {
                                       Item = g.Key,
                                       Count = g.Count()
                                   };

            var groupsSorted = groupsWithCounts.OrderByDescending(g => g.Count);
            return groupsSorted.First().Item;

        }

        private string commonDescription(List<WeatherForecast> dayData)
        {
            List<string> desc = dayData.Select(x => x.Description).ToList();
            var groupsWithCounts = from s in desc
                                   group s by s into g
                                   select new
                                   {
                                       Item = g.Key,
                                       Count = g.Count()
                                   };

            var groupsSorted = groupsWithCounts.OrderByDescending(g => g.Count);
            return groupsSorted.First().Item;
        }
    }
}