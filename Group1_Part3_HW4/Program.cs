using System;
using System.Formats.Asn1;
using System.Net;
using System.Xml.Linq;

namespace Group1_Part3_HW4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create JSON string
            string jsonString = @"{""menu"": {
                                        ""id"": ""file"",
                                        ""value"": ""File"",
                                        ""popup"": {
                                            ""menuitem"":   [
                                                {""value"": ""New"", ""onclick"": ""CreateNewDoc()""},
                                                {""value"": ""Open"", ""onclick"": ""OpenDoc()""},
                                                {""value"": ""Close"", ""onclick"": ""CloseDoc()""}
                                                            ]
                                                    }
                                              }
                                }";

            Console.WriteLine($"Initial JSON data: {jsonString}");
            Console.WriteLine("-----------------------------------------------------------------------------------");

            var jsonData = new JsonData(jsonString);

            // Create adapter to convert from JSON to XML
            var adapter = new DataAdapter(new JsonToXmlConverter());

            // Convert data and output the result
            var xmlData = adapter.Convert(jsonData);
            xmlData.Display();
            Console.WriteLine("-----------------------------------------------------------------------------------");

            // Create adapter to convert from XML to JSON
            adapter = new DataAdapter(new XmlToJsonConverter());

            // Convert data and output the result
            var jsonData2 = adapter.Convert(xmlData);
            jsonData2.Display();
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }
    }
}