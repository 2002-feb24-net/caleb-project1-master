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
    public class CustomersDAL : ICustomersDAL
    {
        readonly BookStoreContext context;

        public CustomersDAL(BookStoreContext context)
        {
            this.context = context;
        }

        public CustomersDAL()
        {
            context = new BookStoreContext();
        }

        public void Remove(int id)
        {
            var toRemove = context.Customers.Find(id);
            context.Customers.Remove(toRemove);
            context.SaveChanges();

        }

        public Customers FindByID(int id)
        {
            return context.Customers.Find(id);
        }

        public async Task<IEnumerable<Customers>> GetCusts()
        {
            return await context.Customers.ToListAsync();
        }

        /// <summary>
        /// Adds customer to the database
        /// </summary>
        /// <param name="cust"></param>
        public int Add(Customers cust)
        {
            context.Customers.Add(cust);
            context.SaveChanges();
            context.Entry(cust).Reload();
            return cust.Id;
        }

        /// <summary>
        /// Sets customer state to edited
        /// </summary>
        /// <param name="cust"></param>
        public void Edit(Customers cust)
        {
            var old = FindByID(cust.Id);
            context.Entry(old).State = EntityState.Detached;
            context.Set<Customers>().Attach(cust);
            context.Entry(cust).State = EntityState.Modified;
            context.SaveChanges();
        }

        //Search for customers by name or username
        //modes:
        //  1: Search by name
        //  2: username
        //  default: lastname
        public IEnumerable<Customers> SearchCust(int mode = 0, params string[] search_param)
        {
            if (mode == 1)
            {
                return context.Customers.Where(cust => cust.FirstName == search_param[0] && cust.LastName == search_param[1]);
            }
            else if (mode == 2)
            {
                return context.Customers.Where(cust => cust.Username == search_param[0]);
            }
            else
            {
                return context.Customers.Where(cust => cust.LastName == search_param[0]);
            }
        }

        //Customer to be verified gets id of -1 if username dne or is invalid
        //returns the customer's password
        public string VerifyCustomer(string username, out int id)
        {
            var cust = context.Customers.FirstOrDefault(cust => cust.Username == username);
            if (cust == null)
            {
                id = -1;
            }
            else
            {
                id = cust.Id;
                return cust.Password;
            }
            return "";

        }
    }
}
