using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Model
{
    public partial class Stores
    {
        public Stores()
        {
            Inventory = new HashSet<Inventory>();
            Orders = new HashSet<Orders>();
        }
        /// <summary>
        /// Store Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        [Display(Name = "Store Address")]
        [MaxLength(100, ErrorMessage = "Maximum address length is 100")]
        public string Address { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
