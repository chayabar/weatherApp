using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    interface IBL
    {
        Task<IEnumerable<DailyForeCast>> GetForecastAsync(string location, int days);

    }
}
