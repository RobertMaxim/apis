namespace APIsEx2.DTOs
{
    public class OrderProductDto
    {
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }

    }
}
