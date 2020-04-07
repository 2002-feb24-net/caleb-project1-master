using BookStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain
{
    public interface IStoresDAL
    {
        /// <summary>
        /// Updates quantity of inventory in database
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="qty">quantity to decrease by</param>
        void UpdateInventory(int id, int qty);

        /// <summary>
        /// Retrieves quantity of a specific item in inventory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetQty(int id);

        /// <summary>
        /// Retrieves a store's inventory
        /// </summary>
        /// <param name="id">location id or name</param>
        /// <returns>list of items</returns>
        List<Inventory> GetInventory(int id);
    }
}
