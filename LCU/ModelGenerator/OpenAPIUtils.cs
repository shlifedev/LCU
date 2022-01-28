using Newtonsoft.Json.Linq;
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
    public static class OpenAPIUtils
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

    public static class CodeGen
    {
        public class SchemaInfo
        {
            public SchemaInfo(JToken key, JToken desc, JToken props, JToken @enum, JToken type)
            {
                Key = key;
                Desc = desc;
                Props = props;
                Enum = @enum;
                Type = type;
            } 
            public JToken Key { get; set; }
            public JToken Desc { get; set; }
            public JToken Props { get; set; }
            public JToken Enum { get; set; }
            public JToken Type { get; set; }


            public bool IsEnum() => Enum != null;
            public bool IsProps() => Props != null;
            public bool HasDesc() => Desc != null; 
        }
        static void LoadAPI()
        {
            var json = System.IO.File.ReadAllText("api.json");
            var obj = JObject.Parse(json);

            var schemas = (JObject)obj["components"]["schemas"]; 
            foreach(KeyValuePair<string, JToken> schema in schemas)
            { 
                // class name
                var key = schema.Key; 
                // description
                var desc = schemas[schema.Key]["description"];
                // properties
                var properties = schemas[schema.Key]["properties"];
                // enum
                var @enum = schemas[schema.Key]["enum"];
                // schema type
                var type = schemas[schema.Key]["type"];

                SchemaInfo si = new SchemaInfo(key, desc, properties, @enum, type);
                GenerateEnum(si);
            }
        } 

        static void GenerateEnum(SchemaInfo si)
        {
            Console.WriteLine(si.IsEnum());
        }

        public static void Generate()
        {
            LoadAPI(); 
        }
    }
}

 
