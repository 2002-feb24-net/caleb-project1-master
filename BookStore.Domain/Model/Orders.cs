using System;
using System.Collections.Generic;

namespace BookStore.Domain.Model
{
    public partial class Orders
    {
        public Orders()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderTime { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Stores Store { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
