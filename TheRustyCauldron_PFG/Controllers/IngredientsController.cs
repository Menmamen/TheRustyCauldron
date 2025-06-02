using System.Data.Entity; // For Entity Framework 6
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks; // For async/await if you use them with EF6
using TheRustyCauldron_PFG.Models; // Your models

namespace TheRustyCauldron_PFG.Controllers
{
    public class IngredientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext(); // Instantiate your DbContext

        // GET: Ingredients
        public async Task<ActionResult> Index()
        {
            // Use .ToListAsync() for async in EF6, requires System.Data.Entity
            var ingredients = await db.Ingredients.ToListAsync();
            return View(ingredients);
        }

        // IMPORTANT: Dispose of the DbContext
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}