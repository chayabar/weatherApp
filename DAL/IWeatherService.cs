using System.Collections.Generic;
using DAL;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface IWeatherService
    {
        Task<IEnumerable<DailyForeCast>> GetForecastAsync(string location, int days);
    }
}
