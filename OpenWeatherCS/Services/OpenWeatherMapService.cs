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
        private const int MAX_FORECAST_DAYS = 5;
        private HttpClient client;

        public OpenWeatherMapService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(string location, int days)
        {
            if (location == null) throw new ArgumentNullException("Location can't be null.");
            if (location == string.Empty) throw new ArgumentException("Location can't be an empty string.");
            if (days <= 0) throw new ArgumentOutOfRangeException("Days should be greater than zero.");
            if (days > MAX_FORECAST_DAYS) throw new ArgumentOutOfRangeException($"Days can't be greater than {MAX_FORECAST_DAYS}");

            var query = $"forecast?q={location}&type=accurate&mode=xml&units=metric&cnt={days}&APPID={APP_ID}";
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


                    var data = x.Descendants("time").Select(w => new WeatherForecast
                    {
                        Description = w.Element("symbol").Attribute("name").Value,
                        ID = int.Parse(w.Element("symbol").Attribute("number").Value),
                        IconID = w.Element("symbol").Attribute("var").Value,
                        Date = (DateTime.Parse(w.Attribute("from").Value)).Date,
                        WindType = w.Element("windSpeed").Attribute("name").Value,
                        WindSpeed = double.Parse(w.Element("windSpeed").Attribute("mps").Value),
                        WindDirection = w.Element("windDirection").Attribute("code").Value,
                        Temperature = double.Parse(w.Element("temperature").Attribute("value").Value),
                        MaxTemperature = double.Parse(w.Element("temperature").Attribute("max").Value),
                        MinTemperature = double.Parse(w.Element("temperature").Attribute("min").Value),
                        Pressure = double.Parse(w.Element("pressure").Attribute("value").Value),
                        Humidity = double.Parse(w.Element("humidity").Attribute("value").Value)
                    });

                    return data;
                default:
                    throw new NotImplementedException(response.StatusCode.ToString());
            }           
        }
    }
}