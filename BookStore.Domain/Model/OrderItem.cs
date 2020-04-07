using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookStore.Domain.Model
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Orders Order { get; set; }

        /// <summary>
        /// Checks if request is for gte 50% of the available quantity or lt 100 and lt available qty
        /// </summary>
        /// <param name="item">object containing requested quantity</param>
        /// <param name="qty">available item quantity</param>
        /// <returns></returns>
        public bool ValidateQuantity(int qty)
        {
            Debug.WriteLine(this.Quantity > 0);
            return this.Quantity > 0 && (this.Quantity < qty * .5 || (this.Quantity < 100 && this.Quantity < qty));
        }
    }
}
