using System;
using System.Collections.Generic;

namespace APIsEx2.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Canceled { get; set; }
        public string Address { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
