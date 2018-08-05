using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    class BL_Imp : IBL
    {
        public Task<IEnumerable<DailyForeCast>> GetForecastAsync(string location, int days)
        {
            throw new NotImplementedException();
        }
    }
}
