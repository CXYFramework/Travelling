using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling.Model.DTO
{
    public class HotelDescriptionDTO
    {
        public string Id { get; set; }
        public string HotelName { get; set; }
        public int HotelCityCode { get; set; }
        public int AreaID { get; set; }
        public string AddressLine { get; set; }
        public string Description { get; set; }
        public string DescriptionText { get; set; }
        public string Url { get; set; }
        public decimal MinPrice { get; set; }


    }
}
