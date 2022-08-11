using System;
using System.Collections.Generic;

namespace APIsEx.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int AvailableUnits { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
