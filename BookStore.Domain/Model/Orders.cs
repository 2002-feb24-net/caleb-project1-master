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
        /// Id of customer placing the order
        /// </summary>
        [Required(ErrorMessage = "Customer name required.")]
        public int CustomerId { get; set; }
        /// <summary>
        /// Id of store the order was placed to 
        /// </summary>
        [Required(ErrorMessage = "Store is required.")]
        public int StoreId { get; set; }
        /// <summary>
        /// Total order cost
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Timestamp of order creation
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// Navigation property to customer making the order
        /// </summary>
        public virtual Customers Customer { get; set; }
        /// <summary>
        /// Navigation property to store customer is ordering from
        /// </summary>
        public virtual Stores Store { get; set; }
        /// <summary>
        /// OrderItems associated with this order
        /// </summary>
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
