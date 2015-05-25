using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling.OpenApiLogic
{
    public class OTATicketServiceLogic
    {

        public string GetTicketBySearch(int areaId, string keywords)
        {

            string reqXml =
                @"<ScenicSpotSearchRequest>
<DistributionChannel>9</DistributionChannel>
<PagingParameter>
<PageIndex>1</PageIndex>
<PageSize>20</PageSize>
</PagingParameter>
<SearchParameter>
<Keyword>北京</Keyword>
<SaleCityID>1</SaleCityID>
</SearchParameter>
</ScenicSpotSearchRequest>";

            APICommon apicommon = new APICommon();
            return WebSvcCaller.TicketCaller(apicommon.GetTicketJson(0, reqXml, "TicketSenicSpotSearch", "TicketSenicSpotSearch"));

        }
    }
}
