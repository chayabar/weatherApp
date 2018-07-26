using System.Collections.Generic;
using OpenWeatherCS.Models;
using System.Threading.Tasks;

namespace OpenWeatherCS.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<DailyForeCast>> GetForecastAsync(string location, int days);
    }
}
