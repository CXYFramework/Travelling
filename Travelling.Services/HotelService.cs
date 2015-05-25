using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelling.Model.DTO;
using Travelling.LuceneSearch;
namespace Travelling.Services
{
    public class HotelService
    {
        public IList<HotelDescriptionDTO> GetHotelDescriptions()
        {
            return Lucene.GetAllIndexRecords().ToList();

        }
    }
}
