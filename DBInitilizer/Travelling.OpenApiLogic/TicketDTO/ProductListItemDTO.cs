using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling.OpenApiLogic.TicketDTO
{
    public class ProductListItemDTO
    {
        public int ID;
        public string Name;
        public int MarketPrice;
        public int Price;
        public bool IsReturnCash;
        public int ReturnCashAmount;
    }
}
