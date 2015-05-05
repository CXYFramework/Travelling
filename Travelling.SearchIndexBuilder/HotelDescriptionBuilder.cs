using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelling.Data;
using Travelling.LuceneSearch;

namespace Travelling.SearchIndexBuilder
{
    public class HotelDescriptionBuilder
    {
        public void GenerateSearchIndex()
        {
            DB_HotelHelper hotel = new DB_HotelHelper();

            var hoteDescriptions = hotel.GetHotelDescription();

            //Lucene.AddUpdateLuceneIndex(hoteDescriptions);

            var h = Lucene.GetAllIndexRecords();
        }
    }
}
