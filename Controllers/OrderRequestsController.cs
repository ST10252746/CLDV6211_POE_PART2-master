using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10242546_CLDV6211_POE_.Data;
using ST10242546_CLDV6211_POE_.Models;

namespace ST10242546_CLDV6211_POE_.Controllers
{
    public class OrderRequestsController : Controller
    {
        private readonly KhumaloCraftDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public OrderRequestsController(KhumaloCraftDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        // GET: OrderRequests
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var orderRequests = from o in _context.OrderRequests.Include(o => o.Order).Include(o => o.Product)
                                select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                orderRequests = orderRequests.Where(o => o.OrderId.ToString() == searchString);
            }

            return View(await orderRequests.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: OrderRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderRequest = await _context.OrderRequests
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderRequestId == id);
            if (orderRequest == null)
            {
                return NotFound();
            }

            return View(orderRequest);
        }
        [Authorize(Roles = "Admin")]
        // GET: OrderRequests/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: OrderRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderRequestId,OrderId,ProductId,OrderStatus")] OrderRequest orderRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderRequest.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", orderRequest.ProductId);
            return View(orderRequest);
        }

        [Authorize(Roles = "Admin")]
        // GET: OrderRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderRequest = await _context.OrderRequests.FindAsync(id);
            if (orderRequest == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderRequest.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", orderRequest.ProductId);
            return View(orderRequest);
        }

        // POST: OrderRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderRequestId,OrderId,ProductId,OrderStatus")] OrderRequest orderRequest)
        {
            if (id != orderRequest.OrderRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderRequestExists(orderRequest.OrderRequestId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderRequest.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", orderRequest.ProductId);
            return View(orderRequest);
        }

        [Authorize(Roles = "Admin")]
        // GET: OrderRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderRequest = await _context.OrderRequests
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderRequestId == id);
            if (orderRequest == null)
            {
                return NotFound();
            }

            return View(orderRequest);
        }

        // POST: OrderRequests/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderRequest = await _context.OrderRequests.FindAsync(id);
            if (orderRequest != null)
            {
                _context.OrderRequests.Remove(orderRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderRequestExists(int id)
        {
            return _context.OrderRequests.Any(e => e.OrderRequestId == id);
        }
    }
}