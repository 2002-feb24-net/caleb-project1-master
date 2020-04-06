using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Model
{
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }
        /// <summary>
        /// Customer ID
        /// </summary>
        [Display(Name = "CustomerID")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Customer username
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(30, ErrorMessage = "Maximum username length is 30")]
        [MinLength(1, ErrorMessage = "Minimum username length is 1")]
        public string Username { get; set; }

        /// <summary>
        /// Customer password
        /// </summary>
        [Display(Name = "Password")]
        [MaxLength(100, ErrorMessage = "Maximum password length is 100")]
        [MinLength(1, ErrorMessage = "Minium password length is 1")]
        [ScaffoldColumn(false)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// Customer first name
        /// </summary>
        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Maximum name length is 50")]
        public string FirstName { get; set; }

        /// <summary>
        /// Customer last name
        /// </summary>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Maximum name length is 50")]
        public string LastName { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
