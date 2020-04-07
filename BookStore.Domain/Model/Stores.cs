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
        /// Stores's Id, primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Location name
        /// </summary>
        [Display(Name = "Store Address")]
        [MaxLength(200, ErrorMessage = "Max address length is 200.")]
        public string Address { get; set; }

        /// <summary>
        /// Inventories related to specified store
        /// </summary>
        public virtual ICollection<Inventory> Inventory { get; set; }
        /// <summary>
        /// Orders related to specified store
        /// </summary>
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
