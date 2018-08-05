using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class City
    {
        public string name { get; set; }
        public List<DailyForeCast> cityForecast { get; set; }
    }
}
