using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace BookStore.Domain.Model
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }

        /// <summary>
        /// Product purchased
        /// </summary>
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity of order purchased
        /// </summary>
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// navigation property to order
        /// </summary>
        public virtual Orders Order { get; set; }

        /// <summary>
        /// navigation property to inventory the item was purchased from
        /// </summary>
        public virtual Inventory Product { get; set; }

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
