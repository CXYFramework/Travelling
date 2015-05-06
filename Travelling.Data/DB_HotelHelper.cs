using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Travelling.Models;
using Travelling.Model.DTO;

namespace Travelling.Data
{
    public class DB_HotelHelper
    {
        public IEnumerable<HotelDescriptionDTO> GetHotelDescription()
        {
            string sql = @"select HotelName ,h.Id,HotelCityCode , AreaID ,a.AddressLine,ae.Description,
                            (
                            select top 1 DescriptionText from TextItem where HotelID=h.Id and Category=5

                            ) as DescriptionText,
                            (
                            select top 1 URL from ImageItem where HotelID=h.Id and Category=1

                            ) as url,

                            (select min(AmountBeforeTax) from BaseByGuestAmt g join Rate r on g.RateId=r.Id
                            join RatePlan p on p.Id=r.RatePlanId
                            where p.HoteID=h.Id) as MinPrice

                            from Hotel h join Address a on a.HotelID=h.Id
                            join AddressExtension ae on ae.AddressID=a.Id";

            using (var conn = BaseConnection.GetOpenConnection())
            {

                return conn.Query<HotelDescriptionDTO>(sql);
            }

        }
    }
}
