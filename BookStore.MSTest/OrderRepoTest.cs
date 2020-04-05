using BookStore.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P0Library.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.MSTest
{
    [TestClass]
    class OrderRepoTest
    {
        public void TestSetup(BookStoreContext context)
        {
            OrdersDAL Repo;
            StoresDAL LocDal;
            CustomersDAL CustDal;

            Repo = new OrdersDAL(context);
            LocDal = new StoresDAL(context);
            CustDal = new CustomersDAL(context);
            var cust = new Customers
            {
                Id = 1,
                Username = "usersample",
                Password = "passwordsample",
                FirstName = "fsample",
                LastName = "lsample"
            };
            var loc = new Stores
            {
                Id = 1,
                Address = "tacoma"
            };
            var prod = new Products
            {
                Id = 1,
                Name = "booksample",
                Price = 10
            };
            var inventory = new Inventory
            {
                Id = 888,
                ProductId = 1,
                StoreId = 1,
                Quantity = 10000
            };
            CustDal.Add(cust);
            LocDal.Add(loc);
            context.Products.Add(prod);
            context.Inventory.Add(inventory);
            context.SaveChanges();
        }

        [TestMethod]
        public async Task TestOrdtAdd()
        {
            OrdersDAL Repo;
            StoresDAL LocDal;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;

                using (var context = new BookStoreContext(options))
                {
                    context.Database.EnsureCreated();

                }
                using (var context = new BookStoreContext(options))
                {
                    TestSetup(context);
                    Debug.WriteLine(context.Customers.ToList().Count);
                    Repo = new OrdersDAL(context);
                    LocDal = new StoresDAL(context);
                    var custs = await Repo.GetOrds();
                    int initial_count = custs.ToList().Count;
                    Orders ord1 = new Orders
                    {
                        ProductId = 1,
                        CustomerId = 1,
                        StoreId = 1,
                        Price = 10,
                        OrderTime = DateTime.Now,
                        Quantity = 1
                    };

                    int x = Repo.Add(ord1);
                    custs = await Repo.GetOrds();
                    int final_count = custs.ToList().Count;
                    Assert.AreEqual(final_count, initial_count + 1);
                    Repo.Remove(x);
                }
            }
            finally
            {
                conn.Close();
            }
        }
        [TestMethod]
        public void TestOrdEdit()
        {
            OrdersDAL Repo;
            StoresDAL LocDal;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    LocDal = new StoresDAL(context);
                    var ord = new Orders
                    {
                        ProductId = 1,
                        CustomerId = 1,
                        StoreId = 1,
                        Price = 10,
                        OrderTime = DateTime.Now,
                        Quantity = 1
                    };
                    int target = Repo.Add(ord);
                    var toEdit = Repo.FindByID(target);
                    toEdit.Price = 0;
                    Repo.Edit(toEdit);
                    var editedCust = Repo.FindByID(target);
                    Assert.AreEqual(0, editedCust.Price);
                    Repo.Remove(target);
                }
            }
            finally
            {
                conn.Close();
            }
        }
        [TestMethod]
        public async Task TestOrdDelete()
        {
            OrdersDAL Repo;
            StoresDAL LocDal;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    LocDal = new StoresDAL(context);
                    var custs = await Repo.GetOrds();
                    int initialCount = custs.ToList().Count;
                    var newOrd = new Orders
                    {
                        ProductId = 1,
                        CustomerId = 1,
                        StoreId = 1,
                        Price = 10,
                        OrderTime = DateTime.Now,
                        Quantity = 1
                    };
                    int addedID = Repo.Add(newOrd);

                    Repo.Remove(addedID);
                    custs = await Repo.GetOrds();
                    int finalCount = custs.ToList().Count;
                    Assert.AreEqual(initialCount, finalCount);
                }
            }
            finally
            {
                conn.Close();
            }
        }
        [TestMethod]
        public async Task TestOrdDeleteEmpty()
        {
            OrdersDAL Repo;
            StoresDAL LocDal;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    LocDal = new StoresDAL(context);
                    var newOrd = new Orders
                    {
                        ProductId = 1,
                        CustomerId = 1,
                        StoreId = 1,
                        Price = 10,
                        OrderTime = DateTime.Now,
                        Quantity = 1
                    };
                    Repo.Add(newOrd);
                    var custs = await Repo.GetOrds();
                    int initialCount = custs.ToList().Count;
                    var newOrd1 = new Orders
                    {
                        ProductId = 1,
                        CustomerId = 1,
                        StoreId = 1,
                        Price = 10,
                        OrderTime = DateTime.Now,
                        Quantity = 1
                    };
                    int rem = Repo.Add(newOrd1);

                    custs = await Repo.GetOrds();
                    int current = custs.ToList().Count;
                    Assert.AreEqual(initialCount, initialCount);
                    Repo.Remove(rem);
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
