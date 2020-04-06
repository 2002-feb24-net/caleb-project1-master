using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Model
{
    public partial class Inventory
    {
        [Key]
        [Display(Name = "Inventory Item ID")]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// Id of this product in inventory
        /// </summary>
        [Display(Name = "Product ID")]
        [Range(0, int.MaxValue)]
        public int? ProductId { get; set; }

        /// <summary>
        /// Store that the inventory belongs to
        /// </summary>
        [Display(Name = "Store ID")]
        [Range(0, int.MaxValue)]
        public int? StoreId { get; set; }

        /// <summary>
        /// Quantity of the specific item in stock
        /// </summary>
        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// Id of product in inventory
        /// </summary>
        public virtual Products Product { get; set; }

        /// <summary>
        /// Navigation property to store holding this inventory
        /// </summary>
        public virtual Stores Store { get; set; }

        /// <summary>
        /// order items referring to this inventory item
        /// </summary>
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
