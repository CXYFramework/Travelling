using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Travelling.Unitity
{
    public class CommonUtil
    {
        public static string GetHotelID(int hotelCode)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(hotelCode + "CtripHotel", "MD5");
        }
    }
}
