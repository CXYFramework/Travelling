using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelling.OpenApiLogic;
using Travelling.OpenApiLogic.HotelDTO;
namespace Travelling.RatePlanCacheUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            OTAHotelServiceLogic hotelServiceLogic = new OTAHotelServiceLogic();
            IList<HotelRateRlanCacheDTO> changeItems = hotelServiceLogic.GetHotelRatePlanCacheChange("2");
            foreach (var item in changeItems)
            {
                string responseXml = hotelServiceLogic.GetHotelRatePlan(item.HotelCode, item.RatePlanCode);
            }


        }
    }
}
