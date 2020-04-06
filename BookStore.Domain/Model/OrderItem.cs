using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookStore.Domain.Model
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }

        /// <summary>
        /// Checks if request is for greater than half of input quantity, less than 100, and less than input quantity
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
