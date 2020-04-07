using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Model
{
    public partial class Inventory
    {
        public Inventory()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        [Key]
        [Display(Name = "Inventory Item ID")]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        /// <summary>
        /// Id of product in inventory
        /// </summary>
        [Display(Name = "Product ID")]
        [Range(0, int.MaxValue)]
        public int ProductId { get; set; }
        /// <summary>
        /// Id of store in inventory
        /// </summary>
        [Display(Name = "Store ID")]
        [Range(0, int.MaxValue)]
        public int StoreId { get; set; }
        /// <summary>
        /// Quantity of the this item in inventory
        /// </summary>
        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// Navigation property to inventory's associated product
        /// </summary>
        public virtual Products Product { get; set; }
        /// <summary>
        /// Navigation property to inventory's associated store
        /// </summary>
        public virtual Stores Store { get; set; }
        /// <summary>
        /// order items referencing this inventory item
        /// </summary>
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
