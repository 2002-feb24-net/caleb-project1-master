﻿using BookStore.Domain.Model;
using BookStore.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.MSTest
{
    [TestClass]
    public class OrderRepoTest
    {
        public void TestSetup(Infrastructure.BookStoreContext context)
        {
            OrdersDAL Repo;
            StoresDAL StoreDAL;
            CustomersDAL CustDal;

            Repo = new OrdersDAL(context);
            StoreDAL = new StoresDAL(context);
            CustDal = new CustomersDAL(context);
            var cust = new Customers
            {
                Id = 1,
                Username = "usersample",
                Password = "passwordsample",
                FirstName = "fsample",
                LastName = "lsample"
            };
            var store = new Stores
            {
                Id = 1,
                Address = "123 test blvd"
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
            StoreDAL.Add(store);
            context.Products.Add(prod);
            context.Inventory.Add(inventory);
            context.SaveChanges();
        }

        [TestMethod]
        public async Task TestOrdtAdd()
        {
            OrdersDAL Repo;
            StoresDAL StoreDAL;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<Infrastructure.BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;

                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    context.Database.EnsureCreated();

                }
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    TestSetup(context);
                    Debug.WriteLine(context.Customers.ToList().Count);
                    Repo = new OrdersDAL(context);
                    StoreDAL = new StoresDAL(context);
                    var custs = await Repo.GetOrds();
                    int initial_count = custs.ToList().Count;
                    Orders ord1 = new Orders
                    {
                        CustomerId = 1,
                        StoreId = 1,
                        Total = 10,
                        OrderTime = DateTime.Now
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
            StoresDAL StoreDAL;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<Infrastructure.BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    StoreDAL = new StoresDAL(context);
                    var ord = new Orders
                    {
                        CustomerId = 1,
                        StoreId = 1,
                        Total = 10,
                        OrderTime = DateTime.Now
                    };
                    int target = Repo.Add(ord);
                    var toEdit = Repo.FindByID(target);
                    toEdit.Total = 0;
                    Repo.Edit(toEdit);
                    var editedCust = Repo.FindByID(target);
                    Assert.AreEqual(0, editedCust.Total);
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
            StoresDAL StoreDAL;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<Infrastructure.BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    StoreDAL = new StoresDAL(context);
                    var custs = await Repo.GetOrds();
                    int initialCount = custs.ToList().Count;
                    var newOrd = new Orders
                    {
                        CustomerId = 1,
                        StoreId = 1,
                        Total = 10,
                        OrderTime = DateTime.Now
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
            StoresDAL StoreDAL;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<Infrastructure.BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    StoreDAL = new StoresDAL(context);
                    var newOrd = new Orders
                    {
                        CustomerId = 1,
                        StoreId = 1,
                        Total = 10,
                        OrderTime = DateTime.Now
                    };
                    Repo.Add(newOrd);
                    var custs = await Repo.GetOrds();
                    int initialCount = custs.ToList().Count;
                    var newOrd1 = new Orders
                    {
                        CustomerId = 1,
                        StoreId = 1,
                        Total = 10,
                        OrderTime = DateTime.Now
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

        [TestMethod]
        public void TestOrdTotalUpdate()
        {
            OrdersDAL Repo;
            StoresDAL StoreDAL;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<Infrastructure.BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    StoreDAL = new StoresDAL(context);
                    var newCust = new Orders
                    {
                        CustomerId = 1,
                        StoreId = 1,
                        Total = 10,
                        OrderTime = DateTime.Now
                    };
                    int addedID = Repo.Add(newCust);
                    var orderItem = new OrderItem
                    {
                        OrderId = addedID,
                        InventoryId = 4,
                        Quantity = 2,
                    };
                    Repo.AddOrderItem(orderItem);
                    var order = Repo.FindByID(addedID);
                    Assert.AreEqual(0, order.Total);
                    Repo.RemoveOrderItem(orderItem);
                    Repo.Remove(addedID);
                }
            }
            finally
            {
                conn.Close();
            }
        }
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(100000000)]
        public void TestInvalidQty(int qty)
        {
            OrdersDAL Repo;
            StoresDAL StoreDAL;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<Infrastructure.BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    StoreDAL = new StoresDAL(context);
                    var orderItem = new OrderItem
                    {
                        OrderId = 1,
                        InventoryId = 5,
                        Quantity = qty,
                    };
                    Assert.IsFalse(orderItem.ValidateQuantity(StoreDAL.GetQty(5)));
                }
            }
            finally
            {
                conn.Close();
            }
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(3)]
        public void TestValidQty(int qty)
        {
            OrdersDAL Repo;
            StoresDAL StoreDAL;
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            try
            {
                var options = new DbContextOptionsBuilder<Infrastructure.BookStoreContext>()
                    .UseSqlite(conn)
                    .Options;
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new Infrastructure.BookStoreContext(options))
                {
                    TestSetup(context);
                    Repo = new OrdersDAL(context);
                    StoreDAL = new StoresDAL(context);
                    var orderItem = new OrderItem
                    {
                        OrderId = 1,
                        InventoryId = 5,
                        Quantity = qty
                    };
                    Assert.IsTrue(orderItem.ValidateQuantity(StoreDAL.GetQty(5)));
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
