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
        [Display(Name = "Customer ID")]
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Customer user name
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        [MinLength(1, ErrorMessage = "Minimum username length is 1.")]
        [MaxLength(50, ErrorMessage = "Maximum username length is 50.")]
        public string Username { get; set; }
        /// <summary>
        /// Customer password
        /// </summary>
        [Display(Name = "Password")]
        [MinLength(1, ErrorMessage = "Minium password length is 1.")]
        [MaxLength(50, ErrorMessage = "Maximum password length is 50.")]
        [ScaffoldColumn(false)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        /// <summary>
        /// Customer first name
        /// </summary>
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(200, ErrorMessage = "Maximum name length is 200.")]
        public string FirstName { get; set; }
        /// <summary>
        /// Customer last name
        /// </summary>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Maximum name length is 100.")]
        public string LastName { get; set; }

        /// <summary>
        /// Orders related to specified customer
        /// </summary>
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
