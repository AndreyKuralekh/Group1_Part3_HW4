using System.Text.Json;
using System.Xml;

namespace Group1_Part3_HW4
{
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
}