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
        /// products primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of products
        /// </summary>
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        /// <summary>
        /// Price of a single unit of this product
        /// </summary>
        public decimal Price { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
    }
}
