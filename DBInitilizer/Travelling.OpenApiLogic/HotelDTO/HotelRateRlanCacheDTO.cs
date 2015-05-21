using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Travelling.OpenApiLogic.HotelDTO
{
    public class HotelRateRlanCacheDTO
    {
        public string CityCode { get; set; }
        public string HotelCode { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string RatePlanCode { get; set; }

    }
}
