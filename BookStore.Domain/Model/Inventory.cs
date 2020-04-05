﻿using System;
using System.Collections.Generic;

namespace P0Library.Model
{
    public partial class Inventory
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? StoreId { get; set; }
        public int Quantity { get; set; }

        public virtual Products Product { get; set; }
        public virtual Stores Store { get; set; }
    }
}
