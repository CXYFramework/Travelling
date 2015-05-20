﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Travelling.OpenApiLogic.HotelDTO;
using System.Xml.Linq;
using System.Xml.XPath;

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
        public string GetHotelRatePlan(string hotelCode, string ratePlanCode = "")
        {
            StringBuilder reqXml = new StringBuilder("<ns:OTA_HotelRatePlanRQ TimeStamp=\"2013-06-01T00:00:00.000+08:00\" Version=\"1.0\">");
            reqXml.Append("<ns:RatePlans>");

            reqXml.Append("<ns:RatePlan>");
            reqXml.AppendFormat("<ns:DateRange Start=\"{0}\" End=\"{1}\"/>", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(25).ToString("yyyy-MM-dd"));
            reqXml.Append("<ns:RatePlanCandidates>");

            if (!string.IsNullOrWhiteSpace(ratePlanCode))
            {
                reqXml.AppendFormat("<ns:RatePlanCandidate AvailRatesOnlyInd=\"{0}\" RatePlanCode=\"{1}\" >", "false", ratePlanCode);
            }
            else
            {
                reqXml.AppendFormat("<ns:RatePlanCandidate AvailRatesOnlyInd=\"false\" />");
            }
            reqXml.Append("<ns:HotelRefs>");
            reqXml.AppendFormat("<ns:HotelRef HotelCode=\"{0}\"/>", hotelCode);
            reqXml.Append("</ns:HotelRefs>");
            reqXml.Append("</ns:RatePlanCandidate>");
            reqXml.Append("</ns:RatePlanCandidates>");
            reqXml.Append("<ns:TPA_Extensions RestrictedDisplayIndicator=\"false\"/>");
            reqXml.Append("</ns:RatePlan>");

            reqXml.Append("</ns:RatePlans>");
            reqXml.Append("</ns:OTA_HotelRatePlanRQ>");
            string strRquestType = "OTA_HotelRatePlan";

            return CommonProcess(strRquestType, reqXml.ToString());
        }
        public IList<HotelRateRlanCacheDTO> GetHotelRatePlanCacheChange(string cityCode, string hotelCode = "")
        {

            StringBuilder reqXml = new StringBuilder();
            reqXml.Append("<ns:OTA_HotelCacheChangeRQ Version=\"1.0\">");
            reqXml.AppendFormat("<ns:CacheSearchCriteria CacheFromTimestamp=\"{0}\">", DateTime.Now.AddHours(-1).GetDateTimeFormats('s')[0].ToString());
            reqXml.AppendFormat("<ns:CacheSearchCriterion HotelCityCode=\"{0}\"", cityCode);
            if (!string.IsNullOrWhiteSpace(hotelCode))
            {
                reqXml.AppendFormat(" HotelCode=\"{0}\"", hotelCode);
            }
            reqXml.Append("/>");
            reqXml.Append("</ns:CacheSearchCriteria>");
            reqXml.Append("</ns:OTA_HotelCacheChangeRQ>");

            string strRequestType = "OTA_HotelCacheChange";
            string responseXml = CommonProcess(strRequestType, reqXml.ToString());
            System.IO.File.AppendAllText("D:\\tttt.xml", responseXml);

            var changeItems = new List<HotelRateRlanCacheDTO>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
           
            nsmgr.AddNamespace("c", "http://www.opentravel.org/OTA/2003/05");
            var rsNode = xmlDoc.SelectSingleNode("Response/HotelResponse");


            var hotelNode = rsNode["OTA_HotelCacheChangeRS"];

            string timeSpan = ((XmlElement)hotelNode).GetAttribute("TimeStamp");

            var childNodes = hotelNode.ChildNodes;
            foreach (XmlNode item in childNodes)
            {
                Console.WriteLine(item.Name);
                if (item.Name.ToLower() == "CacheChangeInfo".ToLower())
                {

                    HotelRateRlanCacheDTO changeItem = null;

                    changeItem = new HotelRateRlanCacheDTO();
                    changeItem.HotelCode = ((XmlElement)item).GetAttribute("HotelCode").Trim();

                    var otherInfoNode = (XmlElement)item.SelectSingleNode("c:OtherInfo", nsmgr);
                    changeItem.RatePlanCode = otherInfoNode.GetAttribute("RatePlanCode");
                    changeItems.Add(changeItem);

                }

            }
            return changeItems;

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
