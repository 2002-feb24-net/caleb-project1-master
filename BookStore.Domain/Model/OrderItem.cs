using System;
using System.Collections.Generic;

namespace BookStore.Domain.Model
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? Quantity { get; set; }
        public int? InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Orders Order { get; set; }
    }
}
