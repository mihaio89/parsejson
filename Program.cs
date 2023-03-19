using System;
using System.IO;
using System.Text.Json;

namespace ConsoleApp
{
    class Program
    {
        const string property = "property name";
        static void Main(string[] args)
        {
            // Read the entire file into a string
            string jsonFileContent = File.ReadAllText("input.json");

            // Split the string into individual JSON objects
            string[] jsonObjects = jsonFileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            
            // Initialize a string builder to concatenate the element values
            var elementBuilder = new System.Text.StringBuilder();


            // Parse each JSON object and print the element property
            foreach (string jsonObject in jsonObjects)
            {
                // Parse the JSON object into a JsonDocument
                using JsonDocument document = JsonDocument.Parse(jsonObject);

                // Get the element property and print its value
                JsonElement element = document.RootElement.GetProperty(property);

                // Check if the element property is a str
                if (element.ValueKind == JsonValueKind.String)
                {
                    // Append the element value to the string builder
                    elementBuilder.Append(element.GetString());
                }
                else
                {
                    // If the element property is not a string, append a string representation of the object
                    elementBuilder.Append(element.ToString());
                }
                //Console.WriteLine(element);
                // Append a new line character to separate each element value
                elementBuilder.Append(Environment.NewLine);
            }

             File.WriteAllText("output.txt", elementBuilder.ToString());
        }
    }
}
