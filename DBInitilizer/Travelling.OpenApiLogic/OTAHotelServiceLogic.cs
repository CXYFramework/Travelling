using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Travelling.OpenApiLogic
{
    public class OTAHotelServiceLogic
    {
        public string GetHotelByAreaId(int? areaId)
        {
            StringBuilder reqXml = new StringBuilder();
            reqXml.AppendFormat("<ns:OTA_HotelSearchRQ Version=\"1.0\" PrimaryLangID=\"zh\" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelSearchRQ.xsd\" xmlns=\"http://www.opentravel.org/OTA/2003/05\">");
            reqXml.Append("<ns:Criteria>");
            reqXml.AppendFormat("<ns:Criterion>");
            reqXml.Append("<ns:HotelRef ");

            if (areaId != null)
            {
                reqXml.AppendFormat(" HotelCityCode=\"{0}\"", areaId);
            }

            reqXml.Append(" />");

            reqXml.Append("</ns:Criterion>");
            reqXml.Append("</ns:Criteria>");
            reqXml.Append("</ns:OTA_HotelSearchRQ>");


            string strRequestType = "OTA_HotelSearch";
            string strInputXML = reqXml.ToString();

            return CommonProcess(strRequestType, strInputXML);

           // return System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hotelreal.xml"));


        }

        public string GetHotelDetailsByID(string hotelCode)
        {
            //StringBuilder reqXml = new StringBuilder();
            //reqXml.AppendFormat("<OTA_HotelDescriptiveInfoRQ Version=\"1.0\" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelDescriptiveInfoRQ.xsd\" xmlns=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            //reqXml.Append("<HotelDescriptiveInfos>");
            //reqXml.AppendFormat("<HotelDescriptiveInfo HotelCode=\"{0}\" PositionTypeCode=\"502\">", hotelCode);
            //reqXml.Append("</HotelDescriptiveInfo>");
            //reqXml.Append("</HotelDescriptiveInfos>");
            //reqXml.Append("</OTA_HotelDescriptiveInfoRQ>");

            //string strRequestType = "OTA_ HotelDescriptiveInfo";
            //string strInputXML = reqXml.ToString();
         
            //return CommonProcess(strRequestType, strInputXML);

            return System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "real.xml"));

        }

        private string CommonProcess(string strRequestType, string strInputXML)
        {
            APICommon apicommon = new APICommon();

            string requestHeader = apicommon.GetHeadXML(strRequestType, "");

            strInputXML = "<HotelRequest><RequestBody xmlns:ns=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + strInputXML + "</RequestBody></HotelRequest>";

            string requestXML = string.Format(requestHeader, strInputXML);
            string url = APICommon.APIService + "Hotel/" + strRequestType + ".asmx";
            WebSvcCaller process = new WebSvcCaller();
            Hashtable ht = new Hashtable();
            ht.Add("requestXML", requestXML);
            XmlDocument xd = WebSvcCaller.QuerySoapWebService(url, "Request", ht);

            return xd.InnerText;
        }
    }
}
