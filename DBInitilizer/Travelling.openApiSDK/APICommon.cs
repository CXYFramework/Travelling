using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.Security;

/// <summary>
/// Summary description for APICommon
/// </summary>
public class APICommon
{

    public static string APIService = "http://openapi.ctrip.com/";

    public static int AllianceID =23155;

    public static int SID = 457699;

    public static string SecretKey = "976C0C32-0E3B-4F1E-866D-F425E8B5C450";

    public APICommon()
    {
        
    }

    /// <summary>
    /// Generate the timestamp for the signature
    /// </summary>
    /// <returns></returns>
    public static string GenerateTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 将DataRow数组转换到数据表
    /// </summary>
    /// <param name="drs"></param>
    /// <returns></returns>
    public static DataTable ArrayDataRowToDataTable(DataRow[] drs)
    {
        DataTable resultdt = null;
        if (drs.Length > 0)
        {
            resultdt = drs[0].Table.Clone();
            foreach (DataRow dr in drs)
            {
                resultdt.ImportRow(dr);
            }
        }
        return resultdt;
    }

    /// <summary>
    /// 生成API访问URI
    /// </summary>
    /// <param name="strFileName"></param>
    /// <returns></returns>
    public string GetRequestURL(string strFileName)
    {
        string ts = GenerateTimeStamp();
        string MD5SharedSecret = FormsAuthentication.HashPasswordForStoringInConfigFile(SecretKey, "MD5");
        string signature = FormsAuthentication.HashPasswordForStoringInConfigFile(ts + SID + MD5SharedSecret, "md5");
        string ResultURL = string.Format("{0}/{1}?sid={2}&AllianceID={3}&Timestamp={4}&Signature={5}", APIService, strFileName, SID, AllianceID, ts, signature);
        return ResultURL;
    }

    /// <summary>
    /// 自动生成xml头信息
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    public string GetHeadXML(string requestType, string culture)
    {
        string timeStamp = "";
        string signature = "";
        GetSignature(SID, AllianceID, SecretKey, out timeStamp, out signature, requestType);
        return "<?xml version=\"1.0\"?><Request><Header  AllianceID=\"" + AllianceID + "\" SID=\"" + SID + "\" TimeStamp=\"" + timeStamp + "\"  RequestType=\"" + requestType + "\" Signature=\"" + signature + "\" Culture=\"" + culture + "\" />{0}</Request>";
    }

    public string GetTicketJson(int ProtocolType,string reqXml, string Interface,string requestType)
    {
        string timeStamp = "";
        string signature = "";
        GetSignature(SID, AllianceID, SecretKey, out timeStamp, out signature, requestType);

        return "{\"AllianceID\":\"" + AllianceID + "\",\"SID\":\"" + SID + "\",\"ProtocolType\":" + ProtocolType + ",\"Signature\":\"" + signature + "\",\"TimeStamp\":\"" + timeStamp + "\",\"Channel\":\"Vacations\",\"Interface\":\"" + Interface + "\",\"IsError\":\"false\",\"RequestBody\":\"" + reqXml + "\",\"ResponseBody\":\"\",\"ErrorMessage\":\"\"}";


    }



    public void GetSignature(int sid, int allianceID, string secretKey, out string timeStamp, out string signature, string requestType)
    {
        //string allianceID = "1";
        //string SID = "50";
        //string SecretKey = "abcDFG645354";
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        timeStamp = Convert.ToInt64(ts.TotalSeconds).ToString();
        string MD5SharedSecret = FormsAuthentication.HashPasswordForStoringInConfigFile(secretKey, "MD5");
        signature = FormsAuthentication.HashPasswordForStoringInConfigFile(timeStamp + allianceID + MD5SharedSecret + sid + requestType, "MD5");
    }
}