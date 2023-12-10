using System;
using System.Formats.Asn1;
using System.Net;
using System.Text.Json;
using Newtonsoft.Json;
using System.Xml;
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
    abstract class Data
    {
        public abstract string Content { get; set; }
        public abstract void Display();
    }
    // Class to provide data in JSON format
    class JsonData : Data
    {
        public override string Content { get; set; }

        public JsonData(string content)
        {
            Content = content;
        }

        public override void Display()
        {
            Console.WriteLine("JSON data:");
            Console.WriteLine(Content);
        }
    }
    // Class to provide data in XML format
    class XmlData : Data
    {
        public override string Content { get; set; }

        public XmlData(string content)
        {
            Content = content;
        }

        public override void Display()
        {
            Console.WriteLine("XML data:");
            Console.WriteLine(Content);
        }
    }
    abstract class DataConverter
    {
        public abstract Data Convert(Data data);
    }
    // Class to convert JSON to XML
    class JsonToXmlConverter : DataConverter
    {
        public override Data Convert(Data data)
        {
            // Check data in JSON format
            if (data is JsonData jsonData)
            {
                using var doc = JsonDocument.Parse(jsonData.Content);
                var xml = new XmlDocument();
                var root = xml.CreateElement("root");
                xml.AppendChild(root);
                AddElements(doc.RootElement, root, xml);
                return new XmlData(xml.OuterXml);
            }
            else
            {
                return data;
            }
        }

        // Add element from JSON to XML
        private void AddElements(JsonElement jsonElement, XmlElement xmlElement, XmlDocument xml)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                var newElement = xml.CreateElement(property.Name);
                xmlElement.AppendChild(newElement);
                switch (property.Value.ValueKind)
                {
                    case JsonValueKind.String:
                    case JsonValueKind.Number:
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        newElement.InnerText = property.Value.ToString();
                        break;
                    case JsonValueKind.Array:
                        foreach (var item in property.Value.EnumerateArray())
                        {
                            var itemElement = xml.CreateElement("item");
                            newElement.AppendChild(itemElement);
                            AddElements(item, itemElement, xml);
                        }
                        break;
                    case JsonValueKind.Object:
                        AddElements(property.Value, newElement, xml);
                        break;
                    case JsonValueKind.Null:
                        break;
                }
            }
        }
    }
    // Class to convert from XML to JSON
    class XmlToJsonConverter : DataConverter
    {
        public override Data Convert(Data data)
        {
            if (data is XmlData xmlData)
            {
                var xml = new XmlDocument();
                xml.LoadXml(xmlData.Content);
                var json = new System.IO.StringWriter();
                var writer = new JsonTextWriter(json);
                AddElements(xml.DocumentElement, writer);
                return new JsonData(json.ToString());
            }
            else
            {
                return data;
            }
        }

        private void AddElements(XmlElement xmlElement, JsonWriter writer)
        {
            if (xmlElement.HasChildNodes)
            {
                writer.WritePropertyName(xmlElement.Name);
                writer.WriteStartObject();
                foreach (XmlNode node in xmlElement.ChildNodes)
                {
                    switch (node.NodeType)
                    {
                        case XmlNodeType.Element:
                            AddElements(node as XmlElement, writer);
                            break;
                        case XmlNodeType.Text:
                            writer.WritePropertyName(node.ParentNode.Name.ToString());
                            writer.WriteValue(node.Value);
                            break;
                    }
                }
                writer.WriteEndObject();
            }
            else
            {
                writer.WritePropertyName(xmlElement.Name);
                writer.WriteNull();
            }
        }
    }
    // Class adapter for converting data
    class DataAdapter : DataConverter
    {
        private DataConverter converter;

        public DataAdapter(DataConverter converter)
        {
            this.converter = converter;
        }
        public override Data Convert(Data data)
        {
            return converter.Convert(data);
        }
    }
}