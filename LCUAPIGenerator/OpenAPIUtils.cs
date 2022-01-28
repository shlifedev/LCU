using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LCU.ModelGenerator
{
    /// <summary>
    /// http://www.mingweisamuel.com/lcu-schema/lcu/openapi.json
    /// </summary>
    public static class OpenAPIUtils
    {
        private static string url = "http://www.mingweisamuel.com/lcu-schema/lcu/openapi.json";
        public static void ReqJson()
        {
            string data = null;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            var response = myReq.GetResponse();
            var stream = response.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(stream, encode);
            data = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            System.IO.File.WriteAllText("api.json", data);
        }
    }
}


