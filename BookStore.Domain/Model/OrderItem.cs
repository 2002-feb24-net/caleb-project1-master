using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace BookStore.Domain.Model
{
    public partial class OrderItem
    {
        /// <summary>
        /// Id of specific orderitem
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of order this orderitem relates to
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Quantity of this orderitem purchased
        /// </summary>
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        /// <summary>
        /// Id of product for selected item
        /// </summary>
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        /// <summary>
        /// Id of inventory for purchased item
        /// </summary>
        [ForeignKey("InventoryId")]
        public int InventoryId { get; set; }

        /// <summary>
        /// Navigation property to inventory this item pulls from
        /// </summary>
        public virtual Inventory Inventory { get; set; }
        /// <summary>
        /// Navigation property to order related to this item
        /// </summary>
        public virtual Orders Order { get; set; }
        /// <summary>
        /// Navigation property to product related to this item
        /// </summary>
        public virtual Products Product { get; set; }

        /// <summary>
        /// Ensures request is not for more than available quantity, more than half total quantity, and less than 100 total
        /// </summary>
        /// <param name="item">object containing requested quantity</param>
        /// <param name="quantity">available item quantity</param>
        /// <returns></returns>
        public bool ValidateQuantity(int quantity)
        {
            Debug.WriteLine(this.Quantity > 0);
            return this.Quantity > 0 && (this.Quantity < quantity * .5 || (this.Quantity < 100 && this.Quantity < quantity));
        }
    }
}
