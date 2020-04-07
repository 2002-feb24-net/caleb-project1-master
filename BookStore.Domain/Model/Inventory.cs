using System;
using System.Collections.Generic;

namespace BookStore.Domain.Model
{
    public partial class Inventory
    {
        public Inventory()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }

        public virtual Products Product { get; set; }
        public virtual Stores Store { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
