using BookStore.Domain;
using BookStore.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Infrastructure
{
    public class StoresDAL : IStoresDAL
    {
        private readonly BookStoreContext context;

        public StoresDAL(BookStoreContext cont)
        {
            context = cont;
        }
        public StoresDAL()
        {
            context = new BookStoreContext();
        }

        public IEnumerable<Stores> GetStores()
        {
            return context.Stores.Include("Inventory.Product");
        }

        /// <summary>
        /// Adds customer to the database
        /// </summary>
        /// <param name="cust"></param>
        public void Add(Stores l)
        {
            context.Stores.Add(l);
        }

        /// <summary>
        /// Sets store state to edited
        /// </summary>
        /// <param name="cust"></param>
        public void Edit(Stores l)
        {
            context.Entry(l).State = EntityState.Modified;
        }

        public void UpdateInventory(int id, int qty)
        {
            var to_update = context.Inventory.Find(id);
            to_update.Quantity -= qty;
            context.SaveChanges();
        }

        public int GetQty(int id)
        {
            return context.Inventory.Find(id).Quantity;
        }

        public List<Inventory> GetInventory(int id)
        {
            var listInventoryModel = context.Inventory
                                            .Include("Product")
                                            .Where(i => i.StoreId == id)
                                            .ToList();

            return listInventoryModel;
        }
    }
}
