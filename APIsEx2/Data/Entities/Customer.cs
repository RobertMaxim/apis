using System;
using System.Collections.Generic;

namespace APIsEx2.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
