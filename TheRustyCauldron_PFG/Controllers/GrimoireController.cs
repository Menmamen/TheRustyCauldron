using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using TheRustyCauldron_PFG.Models;

namespace TheRustyCauldron_PFG.Controllers
{
    [Authorize]
    public class GrimoireController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId(); // Get the ID of the currently logged-in user

            // This query fetches UserPotion objects, eagerly loading the associated Potion data.
            var userPotions = await db.UserPotions
                                    .Where(up => up.ApplicationUserId == userId)
                                    .Include(up => up.Potion) // Eagerly load the related Potion for each UserPotion
                                    .OrderByDescending(up => up.DiscoveryDate) // Order by the discovery date
                                    .ToListAsync(); // Convert to a list of UserPotion objects

            return View(userPotions); // Pass a list of UserPotion objects to the view
        }

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