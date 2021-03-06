using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace LCU.ModelGenerator
{
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
            foreach (KeyValuePair<string, JToken> schema in schemas)
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


        static void GenerateSchemas(SchemaInfo si)
        {
            System.IO.Directory.CreateDirectory("Models/Schemas");
            if (si.IsProps())
            {
                string code = $@"
public class {si.Key}
{{
    
}}
";
            }
        }
        static void GenerateEnum(SchemaInfo si)
        {
            System.IO.Directory.CreateDirectory("Models/Schemas/Enum");

            if (si.IsEnum())
            {
                var enumStrings = si.Enum.Values<string>().ToList();

                var datas = string.Join(",\n", enumStrings);
                string desc = si.HasDesc() ? $@"/// <summary>
/// {si.Desc}
/// </summary>" : null;
                string @enum = $@"
{desc} 
public enum {si.Key}
{{
{datas}
}}";
                System.IO.File.WriteAllText($"Models/Schemas/Enum/{si.Key}.cs", @enum);
            }


        }

        public static void Generate()
        {
            LoadAPI();
        }
    }
}


