using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Model
{
    public partial class Orders
    {
        public Orders()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        /// <summary>
        /// Id of customer that placed the order
        /// </summary>
        [Required(ErrorMessage = "The Customer is required")]
        public int? CustomerId { get; set; }

        /// <summary>
        /// Location order was placed to 
        /// </summary>
        [Required(ErrorMessage = "The Store is required")]
        public int? StoreId { get; set; }

        /// <summary>
        /// Total order price
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Timestamp of order
        /// </summary>
        public DateTime? OrderTime { get; set; }

        /// <summary>
        /// Navigation property to ordering customer
        /// </summary>
        public virtual Customers Customer { get; set; }

        /// <summary>
        /// navigation property to store ordered from
        /// </summary>
        public virtual Stores Store { get; set; }

        /// <summary>
        /// Items belonging to this order
        /// </summary>
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
