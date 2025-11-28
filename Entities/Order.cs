namespace WebApplication4.Entities
{
    public class Order
    {
        public int ID { get; set; }
        public string UID { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal Bill { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

    }
}

