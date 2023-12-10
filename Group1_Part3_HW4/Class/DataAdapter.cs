namespace Group1_Part3_HW4
{
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