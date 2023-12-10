namespace Group1_Part3_HW4
{
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
}