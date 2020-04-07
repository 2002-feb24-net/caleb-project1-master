using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Model
{
    public partial class Products
    {
        public Products()
        {
            Inventory = new HashSet<Inventory>();
        }

        /// <summary>
        /// Products's Id primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of product
        /// </summary>
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        /// <summary>
        /// Price of single unit of this product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// URL for referencing book images
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Inventory of this item in related stores
        /// </summary>
        public virtual ICollection<Inventory> Inventory { get; set; }
    }
}
