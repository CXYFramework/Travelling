using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling.OpenApiLogic.TicketDTO
{
    public class LabelSatisticsDTO
    {
        public string LableType;
        public IList<SubLabelSatisticsDTO> SubLabelSatistics;
    }
}
