using BookStore.Domain.Model;
using BookStore.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.MSTest
{
    [TestClass]
    public class RepositoryTest
    {
        [TestInitialize]
        public void TestSetup()
        {
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
            }
            finally
            {
                conn.Close();
            }
        }
        [TestMethod]
        public async Task TestCustAdd()
        {
            CustomersDAL Repo;
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
                    Repo = new CustomersDAL(context);
                    var custs = await Repo.GetCusts();
                    int initial_count = custs.ToList().Count;
                    Customers cust1 = new Customers
                    {
                        Username = "usert1",
                        Password = "passwordt",
                        FirstName = "fsm",
                        LastName = "lsm"
                    };
                    Customers cust2 = new Customers
                    {
                        Username = "usert2",
                        Password = "passwordt",
                        FirstName = "fsm",
                        LastName = "lsm"
                    };
                    Customers cust3 = new Customers
                    {
                        Username = "usert3",
                        Password = "passwordt",
                        FirstName = "fsm",
                        LastName = "lsm"
                    };
                    int x = Repo.Add(cust1); int y = Repo.Add(cust2); int z = Repo.Add(cust3);
                    custs = await Repo.GetCusts();
                    int final_count = custs.ToList().Count;
                    Assert.IsTrue(final_count == (initial_count + 3));
                    Repo.Remove(x);
                    Repo.Remove(y);
                    Repo.Remove(z);
                }
            }
            finally
            {
                conn.Close();
            }
        }
        [TestMethod]
        public void TestCustEdit()
        {
            CustomersDAL Repo;
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
                    Repo = new CustomersDAL(context);
                    Customers cust1 = new Customers
                    {
                        Username = "usert1",
                        Password = "passwordt",
                        FirstName = "fsm",
                        LastName = "lsm"
                    };

                    int target = Repo.Add(cust1);
                    var toEdit = Repo.FindByID(target);
                    toEdit.FirstName = "James";
                    Repo.Edit(toEdit);
                    var editedCust = Repo.FindByID(target);
                    Assert.AreEqual("James", editedCust.FirstName);
                    editedCust.FirstName = "Fsm";
                    Repo.Edit(editedCust);
                }
            }
            finally
            {
                conn.Close();
            }
        }
        [TestMethod]
        public async Task TestCustDelete()
        {
            CustomersDAL Repo;
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
                    Repo = new CustomersDAL(context);



                    var custs = await Repo.GetCusts();
                    int initialCount = custs.ToList().Count;
                    var newCust = new Customers
                    {
                        Username = "usertd",
                        Password = "passwordtd",
                        FirstName = "fsm",
                        LastName = "lsm"
                    };
                    int addedID = Repo.Add(newCust);

                    Repo.Remove(addedID);
                    custs = await Repo.GetCusts();
                    int finalCount = custs.ToList().Count;
                    Assert.AreEqual(initialCount, finalCount);
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
