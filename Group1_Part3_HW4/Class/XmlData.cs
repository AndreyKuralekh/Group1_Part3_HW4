namespace Group1_Part3_HW4
{
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
}