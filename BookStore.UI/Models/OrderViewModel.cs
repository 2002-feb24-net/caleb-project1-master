using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain;
using BookStore.Domain.Model;

namespace BookStore.UI.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        /// <summary>
        /// Username
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        [MinLength(1, ErrorMessage = "Minimum username length is 1")]
        [MaxLength(50, ErrorMessage = "Maximum username length is 50")]
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Password")]
        [MinLength(1, ErrorMessage = "Minium password length is 1")]
        [MaxLength(50, ErrorMessage = "Maximum password length is 50")]
        [ScaffoldColumn(false)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// Id of order
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Customer that placed the order
        /// </summary>
        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }

        /// <summary>
        /// Store to which order was placed
        /// </summary>
        [Required(ErrorMessage = "Store is required")]
        public int StoreId { get; set; }

        /// <summary>
        /// Total order price
        /// </summary>
        public decimal? Total { get; set; }

        /// <summary>
        /// Timestamp of order creation
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// Navigation property to store customer is ordering from
        /// </summary>
        public virtual Stores Store { get; set; }

        /// <summary>
        /// Navigation property to customer making the order
        /// </summary>
        public virtual Customers Customer { get; set; }

        /// <summary>
        /// OrderItems associated with this order
        /// </summary>
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
