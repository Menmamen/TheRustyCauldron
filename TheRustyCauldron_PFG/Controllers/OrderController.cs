using System.Linq; 
using System.Web.Mvc;
using TheRustyCauldron_PFG.Models;
using System.Threading.Tasks;
using System.Data.Entity; 
using Microsoft.AspNet.Identity; 
using System; 

namespace TheRustyCauldron_PFG.Controllers
{
    [Authorize] 
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order/OrderForm
        [AllowAnonymous] // Allow anyone to see the order form for a potion
        public async Task<ActionResult> OrderForm(int potionId)
        {
            var potion = await db.Potions.FindAsync(potionId);
            if (potion == null)
            {
                return HttpNotFound(); // Potion not found, return 404
            }

            return View(potion);
        }

        // POST: Order/SubmitOrder
        [HttpPost]
        [AllowAnonymous] // Allow unauthenticated users to submit an order if you don't require login for ordering
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitOrder(int potionId, string customerName, string customerAddress, string customerEmail)
        {
            var potion = await db.Potions.FindAsync(potionId);
            if (potion == null)
            {
                TempData["ErrorMessage"] = "The potion you tried to order was not found.";
                return RedirectToAction("Index", "Home"); // Redirect to home or cauldron
            }

            // Create a new Order object
            var newOrder = new Order
            {
                PotionId = potion.Id,
                CustomerName = customerName,
                CustomerAddress = customerAddress,
                CustomerEmail = customerEmail,
                Price = potion.Price, // Capture the price at the time of order
                OrderDate = DateTime.Now
            };

            // Link the order to the currently logged-in user if available
            if (User.Identity.IsAuthenticated)
            {
                newOrder.ApplicationUserId = User.Identity.GetUserId();
            }
            // else, if you allow guests to order, the ApplicationUserId will be null.

            db.Orders.Add(newOrder); // Add the order to the context
            await db.SaveChangesAsync(); // Save to the database

            TempData["SuccessMessage"] = $"Thank you for your order! Your '{potion.Name}' potion (Price: {potion.Price.ToString("C")}) will be dispatched to {newOrder.CustomerName} at {newOrder.CustomerAddress}. We'll send an email to {newOrder.CustomerEmail}. Your Order ID is: {newOrder.OrderId}.";

            return RedirectToAction("OrderConfirmation");
        }

        public ActionResult OrderConfirmation()
        {
            // Simple view to display the success message from TempData
            return View();
        }

        // --- NEW: MyOrders Action ---
        // This action does not need [Authorize] if the controller itself is [Authorize]
        public async Task<ActionResult> MyOrders()
        {
            string userId = User.Identity.GetUserId(); // Get the ID of the current logged-in user

            // Fetch orders for this user, including the associated Potion details
            var userOrders = await db.Orders
                                     .Include(o => o.Potion) // Eager load the Potion details
                                     .Where(o => o.ApplicationUserId == userId) // Filter by user ID
                                     .OrderByDescending(o => o.OrderDate) // Show most recent orders first
                                     .ToListAsync();

            return View(userOrders);
        }

        // --- NEW: OrderDetails Action ---
        // This action does not need [Authorize] if the controller itself is [Authorize]
        public async Task<ActionResult> OrderDetails(int id)
        {
            string userId = User.Identity.GetUserId();

            var order = await db.Orders
                                .Include(o => o.Potion) // Include Potion details
                                .FirstOrDefaultAsync(o => o.OrderId == id && o.ApplicationUserId == userId);

            if (order == null)
            {
                // Either the order doesn't exist or doesn't belong to the current user
                return HttpNotFound();
            }

            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null) // <--- Safely dispose the DbContext
                {
                    db.Dispose();
                    db = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}