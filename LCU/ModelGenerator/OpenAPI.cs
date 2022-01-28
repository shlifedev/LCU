using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LCU.ModelGenerator
{ 
    /// <summary>
    /// http://www.mingweisamuel.com/lcu-schema/lcu/openapi.json
    /// </summary>
    public static class OpenAPI
    {
        private static string url = "http://www.mingweisamuel.com/lcu-schema/lcu/openapi.json";
        public static void ReqJson()
        {
            string data = null;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            var response =  myReq.GetResponse();
            var stream = response.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(stream, encode); 
            Char[] read = new Char[256];
            // Reads 256 characters at a time.
            int count = readStream.Read(read, 0, 256); 
            while (count > 0)
            {
                // Dumps the 256 characters on a string and displays the string to the console.
                String str = new String(read, 0, count);
                data += str;
                count = readStream.Read(read, 0, 256);
            } 
            // Releases the resources of the response.
            response.Close();
            // Releases the resources of the Stream.
            readStream.Close();  
            System.IO.File.WriteAllText("api.json", data);
        }
    }
}
