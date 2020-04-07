﻿using BookStore.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Model;

namespace BookStore.UI.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersDAL _context;
        private readonly ICustomersDAL _custContext;
        private readonly IStoresDAL _storeContext;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IOrdersDAL oDAL, ICustomersDAL cDAL, IStoresDAL lDAL, ILogger<OrdersController> logger)
        {
            _context = oDAL;
            _custContext = cDAL;
            _storeContext = lDAL;
            this.logger = logger;
        }

        public async Task<IActionResult> SearchStoreOrders(string storeAddress)
        {
            TempData["LocOrds"] = storeAddress;
            logger.LogInformation($"Searching orders for store {1}", storeAddress);
            return View("Index", await _context.GetOrders(1, storeAddress));
        }
        public async Task<IActionResult> SearchCustOrders(string firstName, string lastName)
        {
            TempData["CustOrdsFN"] = firstName;
            TempData["CustOrdsLN"] = lastName;
            logger.LogInformation($"Finding orders for customer: {1} {2}", firstName, lastName);
            var ords = await _context.GetOrders(2, firstName, lastName);
            return View("Index", ords);
        }
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var bookstoreContext = await _context.GetOrds();
            return View(bookstoreContext.ToList());
        }
        public async Task<IActionResult> SortOrders(string SortOption)
        {
            List<Orders> ords = null;
            if (TempData["LocOrds"] != null)
            {
                ords = await _context.GetOrders(1, (string)TempData["LocOrds"]);
            }
            else if (TempData["CustOrds"] != null)
            {
                var fn = (string)TempData["CustOrdsFN"];
                var ln = (string)TempData["CustOrdsLN"];
                ords = await _context.GetOrders(2, fn, ln);
            }
            else
            {
                ords = await _context.GetOrders();
            }
            IEnumerable<Orders> sortedOrdersList = null;
            switch (SortOption)
            {
                case "Latest":
                    sortedOrdersList = ords.OrderByDescending(x => x.OrderTime);
                    break;
                case "Earliest":
                    sortedOrdersList = ords.OrderBy(x => x.OrderTime);
                    break;
                case "Most Expensive":
                    sortedOrdersList = ords.OrderByDescending(x => x.Total);
                    break;
                case "Least Expensive":
                    sortedOrdersList = ords.OrderBy(x => x.Total);
                    break;
            }
            return View("Index", sortedOrdersList);
        }

        /// <summary>
        /// Get the inventory of a store to show when adding an order
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        private CreateOrderViewModel StoreInventoryDisplay(int storeID)
        {
            TempData["StoreID"] = storeID;
            var inventoryItemModel = new CreateOrderViewModel
            {
                Inventory = _storeContext.GetInventory(storeID)
            };
            ViewData["ProductID"] = new SelectList(_storeContext.GetInventory(storeID), "Id", "P.Name");
            return inventoryItemModel;
        }

        private bool AddOrderItem(OrderItem item)
        {
            // save storeid from previous failed order and current quantity in tempdata - will overwrite when assigning to previous item
            int storeID = Convert.ToInt32(TempData["StoreID"]);
            TempData["StoreID"] = storeID;
            int orderID = Convert.ToInt32(TempData["OrderID"]);
            TempData["OrderID"] = orderID;
            //validate current (order)item's inventory quantity
            if (ModelState.IsValid && item.ValidateQuantity(_storeContext.GetQty(item.InventoryId)))
            {
                //set the old failed quantity to current entered quantity
                //update product's quantity then set the corresponding order id 
                _storeContext.UpdateInventory(item.InventoryId, item.Quantity);
                item.OrderId = orderID;
                _context.AddOrderItem(item);
                logger.LogInformation($"Adding item to order {1}", item.OrderId);
                return true;
            }
            ModelState.AddModelError("QuantityError", "Quantity input invalid, reattempt.");
            return false;
        }

        public IActionResult AddMore([Bind("OrderId", "Quantity", "InventoryId")] OrderItem item)
        {
            if (!AddOrderItem(item))
            {
                TempData["ItemAddError"] = true;
            }
            int storeId = Convert.ToInt32(TempData["StoreID"]);
            StoreInventoryDisplay(storeId);
            return RedirectToAction("CreateOrderItem");
        }

        public IActionResult CreateOrderItem()
        {
            if (TempData["ItemAddError"] != null && (bool)TempData["ItemAddError"] == true)
            {
                ModelState.AddModelError("QuantityError", "Quantity input invalid.");
            }
            int storeID = Convert.ToInt32(TempData["StoreID"]);
            TempData["StoreID"] = storeID;

            return View(StoreInventoryDisplay(storeID));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrderItem([Bind("OrderId", "Quantity", "InventoryId")] OrderItem item)
        {

            if (AddOrderItem(item))
            {
                return RedirectToAction(nameof(Index));
            }//else error
            ModelState.AddModelError("QuantityError", "Quantity input invalid, reattempt.");
            int storeID = Convert.ToInt32(TempData["StoreID"]);
            return View(StoreInventoryDisplay(storeID));
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = _context.FindByID(Convert.ToInt32(id));
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            if (TempData["VerificationError"] != null && (bool)TempData["VerificationError"] == true)
            {
                ModelState.AddModelError("VerificationError", "Invalid username or password, try again.");
            }
            logger.LogInformation("Creating order");
            ViewData["StoreId"] = new SelectList(_context.GetStores(), "Id", "Address");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CustomerId,StoreId,Username,Password")] OrderViewModel order)
        {
            string Username = order.Username, Password = order.Password;
            if (ModelState.IsValid)
            {
                int custid;
                var cst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                if (Password == (_custContext.VerifyCustomer(Username, out custid)))
                {
                    var new_order = new Orders
                    {
                        StoreId = order.StoreId,
                        CustomerId = custid,
                        Total = 0,
                        OrderTime = TimeZoneInfo.ConvertTime(DateTime.Now, cst)
                    };
                    TempData["OrderID"] = _context.Add(new_order);
                    TempData["StoreID"] = order.StoreId;
                    logger.LogInformation("Order created");
                    return RedirectToAction("CreateOrderItem");
                }
                else
                {
                    TempData["VerificationError"] = true;
                    return RedirectToAction("Create");
                }
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.FindByID(Convert.ToInt32(id));
            if (order == null)
            {
                return NotFound();
            }
            IEnumerable<Customers> custEnum = await _custContext.GetCusts();
            ViewData["CustomerId"] = new SelectList(custEnum, "Id", "FirstName", order.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.GetStores(), "Id", "Address", order.StoreId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,StoreId,Total,OrderTime")] Orders order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Edit(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<Customers> custEnum = await _custContext.GetCusts();
            ViewData["CustomerId"] = new SelectList(custEnum, "Id", "FirstName", order.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.GetStores(), "Id", "Address", order.StoreId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public IActionResult Delete(int? id)
        {
            logger.LogInformation($"Order {1} deletion attempt", id);
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.FindByID(Convert.ToInt32(id));
            if (order == null)
            {
                return NotFound();
            }


            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.FindByID(id);
            _context.Remove(order.Id);
            logger.LogInformation($"Order {1} successfully removed.", id);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return !(_context.FindByID(id) == null);
        }
    }
}
