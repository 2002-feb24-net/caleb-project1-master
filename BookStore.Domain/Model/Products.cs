﻿using System;
using System.Collections.Generic;

namespace BookStore.Domain.Model
{
    public partial class Products
    {
        public Products()
        {
            Inventory = new HashSet<Inventory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
    }
}
