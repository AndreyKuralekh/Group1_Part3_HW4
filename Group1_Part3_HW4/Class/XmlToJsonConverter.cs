using Newtonsoft.Json;
using System.Xml;

namespace Group1_Part3_HW4
{
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
}