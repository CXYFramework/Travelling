using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelling.OpenApiLogic;
using Travelling.OpenApiLogic.HotelDTO;
using Travelling.DBInitilizeLogic;
namespace Travelling.RatePlanCacheUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, string> CheckExists = new Dictionary<string, string>();

            OTAHotelServiceLogic hotelServiceLogic = new OTAHotelServiceLogic();
            IList<HotelRateRlanCacheDTO> changeItems = hotelServiceLogic.GetHotelRatePlanCacheChange("2");

            changeItems = changeItems.OrderBy(a => a.HotelCode).ToList();
            foreach (var item in changeItems)
            {
                string hotelId = string.Empty;
                if (CheckExists.ContainsKey(item.HotelCode))
                {
                    hotelId = CheckExists[item.HotelCode];
                }
                else
                {
                    hotelId = DB_HotelInitilizeLogic.GetHotelIdByCode(Convert.ToInt32(item.HotelCode));
                    CheckExists.Add(item.HotelCode, hotelId);

                }
                if (!string.IsNullOrEmpty(hotelId))
                {
                    
                    // string responseXml = hotelServiceLogic.GetHotelRatePlan(item.HotelCode, item.RatePlanCode);
                    DB_PriceInitilizeLogic.Process(hotelId, item.HotelCode,item.RatePlanCode, item.Start, item.End, false);
                    Console.WriteLine(item.HotelCode + "," + item.RatePlanCode);
                }
            }

            Console.Read();
        }
    }
}
