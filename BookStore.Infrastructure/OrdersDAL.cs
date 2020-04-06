using BookStore.Domain;
using BookStore.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure
{
    public class OrdersDAL : IOrdersDAL
    {
        readonly BookStoreContext context;

        public OrdersDAL(BookStoreContext context)
        {
            this.context = context;
        }
        public OrdersDAL()
        {
            this.context = new BookStoreContext();
        }

        public void RemoveOrderItem(OrderItem item)
        {
            context.OrderItem.Remove(item);
            context.SaveChanges();
        }

        public Orders FindByID(int id)
        {
            var res = context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Store)
                    .Include("Products.P")
                    .Include("Products.P.P")
                    .FirstOrDefault(m => m.Id == id);
            context.Entry(res).Reload();
            return res;
        }

        public void Remove(int id)
        {
            try
            {
                var toRemove = context.Orders.Include(x => x.OrderItem).FirstOrDefault(x => x.Id == id);
                context.OrderItem.RemoveRange(toRemove.OrderItem);
                context.Remove(toRemove);
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return;
            }
        }

        public IEnumerable<Stores> GetStores()
        {
            return context.Stores;
        }

        public async Task<IEnumerable<Orders>> GetOrds()
        {
            return await context.Orders.Include(o => o.Customer).Include(o => o.Store).ToListAsync();
        }

        /// <summary>
        /// Adds an order to database
        /// </summary>
        /// <param name="cust"></param>
        public int Add(Orders o)
        {
            context.Orders.Add(o);
            context.SaveChanges();
            context.Entry(o).Reload();
            return o.Id;
        }

        /// <summary>
        /// Sets order's state to edited
        /// </summary>
        /// <param name="cust"></param>
        public void Edit(Orders o)
        {
            context.Entry(o).State = EntityState.Modified;
            context.SaveChanges();
        }

        //Returns price of added item
        public void AddProduct(Products item)
        {
            context.Products.Add(item);
            context.SaveChanges();
        }

        //Returns price of added item
        public void AddOrderItem(OrderItem item)
        {
            context.OrderItem.Add(item);
            context.SaveChanges();
        }

        //Searches orders by given param, param is checked against Order columns according to mode
        //Mode Codes:
        //  1: Get orders by location
        //  2: By customer
        //  3: Get details of 1 specific order
        public async Task<List<Orders>> GetOrders(int mode = 0, params string[] search_param)
        {
            var orderList = context.Orders.Include("Store").Include("Customer").AsQueryable();
            switch (mode)
            {
                case 1:
                    orderList = orderList
                    .Where(o => o.Store.Address == search_param[0]);
                    break;
                case 2:
                    orderList = orderList
                    .Where(o => o.Customer.FirstName == search_param[0] && o.Customer.LastName == search_param[1]);
                    break;
                case 3:
                    orderList = orderList
                    .Where(o => o.Id == Convert.ToInt32(search_param[0]));
                    break;
                default:
                    break;
            }
            return await orderList
                        .Include("Products")
                        .Include("Products.P")
                        .Include("Products.P.P")
                        .ToListAsync();
        }
    }
}
