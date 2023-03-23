using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;


namespace ConsoleApp;
    class Program
    {
        const string property = "mappingOutput";

        const string path = "mappingOutput.json";

        static void Main(string[] args)
        {
            string what = args[0];

        switch (what) {
            case "e":
                ExtractProperty();
                break;
            case "m":
                CreateJsonFromMultiJson();
                break;
            case "r":
                ReplaceEndLine();
                break;
            default:

            break;
        }


        }

        static void ExtractProperty()
        {
            // Read the entire file into a string
            string jsonFileContent = File.ReadAllText("input.json");

              var propertyBuilder = new System.Text.StringBuilder();
            // Split the string into individual JSON objects
            string[] jsonObjects = jsonFileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                foreach (string jsonObject in jsonObjects)
                {
                    using JsonDocument doc = JsonDocument.Parse(jsonObject);

                        JsonElement el = doc.RootElement.GetProperty(property);
                        if (el.ValueKind == JsonValueKind.String)
                        {
                            propertyBuilder.Append(el.GetString());
                        }
                        else
                        {
                            propertyBuilder.Append(el.ToString());
                        }

                        propertyBuilder.Append(Environment.NewLine); // Append a newline after each property
                }
         //
         // Console.WriteLine(propertyBuilder.ToString());
          File.WriteAllText("mappingOutput.json", propertyBuilder.ToString());
        }


        static object XGetArrayElements(JsonElement arrayElement)
        {
            var array = new List<object>();

            // Loop over all elements in the array and add their properties to a new dictionary
            foreach (JsonElement element in arrayElement.EnumerateArray())
            {
                var dict = new Dictionary<string, object>();

                foreach (JsonProperty property in element.EnumerateObject())
                {
                    dict[property.Name] = property.Value.ToString();
                }

                array.Add(dict);
            }

            return array;
        }

        static void ReplaceEndLine() {

            string json = File.ReadAllText(path);

            int count = 0;
            json = Regex.Replace(json, @"\r?\n", Environment.NewLine);



            File.WriteAllText(path, json);

            Console.WriteLine($"Replaced {count} occurrences of \\n with {Environment.NewLine}");

        }

        
  static void CreateJsonFromMultiJson()
    {
    // Read input from file
    string input = File.ReadAllText("mappingOutput.json");

    // Split multi-JSON into separate JSON objects
    string[] jsonObjects = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

    // Create output JSON
    JObject outputJson = new JObject();

    foreach (string jsonObject in jsonObjects)
    {
        // Parse input JSON
        JObject inputJson = JObject.Parse(jsonObject);

        // Add i_ext_ref32 property only once
        if (!outputJson.ContainsKey("i_ext_ref32"))
        {
            outputJson.Add("i_ext_ref32", inputJson["i_ext_ref32"]);
        }

        // Extract relevant arrays
        JArray itBitItArray = inputJson["it_bit_it"] as JArray;
        JArray itBitPyArray = inputJson["it_bit_py"] as JArray;
        JArray itBitTxArray = inputJson["it_bit_tx"] as JArray;

        // Add arrays to output JSON
        if (itBitItArray != null)
        {
            if (!outputJson.ContainsKey("it_bit_it"))
            {
                outputJson.Add("it_bit_it", new JArray());
            }

            ((JArray)outputJson["it_bit_it"]).Merge(itBitItArray);
        }

        if (itBitPyArray != null)
        {
            if (!outputJson.ContainsKey("it_bit_py"))
            {
                outputJson.Add("it_bit_py", new JArray());
            }

            ((JArray)outputJson["it_bit_py"]).Merge(itBitPyArray);
        }

        if (itBitTxArray != null)
        {
            if (!outputJson.ContainsKey("it_bit_tx"))
            {
                outputJson.Add("it_bit_tx", new JArray());
            }

            ((JArray)outputJson["it_bit_tx"]).Merge(itBitTxArray);
        }
    }

    // Write output to file
    File.WriteAllText("output.json", outputJson.ToString());    
    }

}
