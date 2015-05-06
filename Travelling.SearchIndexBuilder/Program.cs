using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling.SearchIndexBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            HotelDescriptionBuilder builder = new HotelDescriptionBuilder();
            builder.GenerateSearchIndex();
        }
    }
}
