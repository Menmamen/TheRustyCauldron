using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity; // Make sure this is present for Database.SetInitializer
using TheRustyCauldron_PFG.Models; // For ApplicationDbContext
using TheRustyCauldron_PFG.Data; // For DbInitializer // <-- ADD THIS LINE IF NOT PRESENT

namespace TheRustyCauldron_PFG
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // --- Database Initialization ---
            // This line tells Entity Framework to use your custom initializer.
            // The CreateDatabaseIfNotExists strategy will create the database
            // and run your DbInitializer.Initialize method if the DB doesn't exist.
            // If the DB exists, it won't do anything unless your models change,
            // and you use DropCreateDatabaseIfModelChanges.
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            // OR, for more aggressive re-seeding during development:
            // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            // OR, for every application restart (use ONLY for active dev, very aggressive):
            // Database.SetInitializer(new DropCreateDatabaseAlways<ApplicationDbContext>());


            // Now, manually invoke your static Initialize method.
            // This ensures your seed data is applied.
            // We need to create a context instance to pass to your static Initialize method.
            using (var context = new ApplicationDbContext())
            {
                DbInitializer.Initialize(context);
            }
        }
    }
}