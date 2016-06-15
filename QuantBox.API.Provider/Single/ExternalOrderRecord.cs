namespace QuantBox.APIProvider.Single
{
    class ExternalOrderRecord
    {
        public int InstrumentId;
        public OrderRecord BuyOpen;
        public OrderRecord BuyClose;
        public OrderRecord SellOpen;
        public OrderRecord SellClose;
    }
}