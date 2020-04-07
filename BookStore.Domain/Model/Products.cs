using System;
using System.Collections.Generic;

namespace BookStore.Domain.Model
{
    public partial class Products
    {
        public Products()
        {
            Inventory = new HashSet<Inventory>();
            OrderItem = new HashSet<OrderItem>();
        }

        /// <summary>
        /// Products's Id primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of product
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Price of single unit of this product
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// string for referencing book images and backup url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Inventory of this product in related stores
        /// </summary>
        public virtual ICollection<Inventory> Inventory { get; set; }
        /// <summary>
        /// Orderitem of this product
        /// </summary>
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
