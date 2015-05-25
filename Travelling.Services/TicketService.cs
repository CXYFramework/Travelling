using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelling.OpenApiLogic;
namespace Travelling.Services
{
    public class TicketService
    {
        OTATicketServiceLogic otaTicketService = new OTATicketServiceLogic();
        public string GetTicketBySearch(int areaId, string keywords)
        {
            return otaTicketService.GetTicketBySearch(areaId, keywords);
        }  
    }
}
