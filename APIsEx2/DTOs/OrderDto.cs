using APIsEx2.Models;

namespace APIsEx2.DTOs
{
    public class OrderDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool OrderStatus { get; set; }
        public string Address { get; set; } = null!;
        public string Name { get; set; } =null!;
        public string Phone { get; set; } = null!;
    }
}
