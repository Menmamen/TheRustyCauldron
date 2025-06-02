using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using TheRustyCauldron_PFG.Models; // Ensure this matches your project's root namespace for models

namespace TheRustyCauldron_PFG.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<UserPotion> UserPotions { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Potion> Potions { get; set; }
        public DbSet<UserPotion> UserPotions { get; set; }
        public DbSet<PotionIngredient> PotionIngredients { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // IMPORTANT: Call the base method first for Identity tables to be configured
            base.OnModelCreating(modelBuilder);

            // Corrected decimal type configurations:
            // REMOVED .HasColumnType("decimal") as it's redundant and can cause conflicts with HasPrecision
            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Price)
                .HasColumnType("decimal")
                .HasPrecision(18, 2); // Set precision and scale for Ingredient Price

            modelBuilder.Entity<Potion>()
                .Property(p => p.Price)
                .HasColumnType("decimal")
                .HasPrecision(18, 2); // Set precision and scale for Potion Price

            // Configure the Potion-Ingredient many-to-many relationship
            modelBuilder.Entity<PotionIngredient>()
                .HasKey(pi => new { pi.PotionId, pi.IngredientId });

            modelBuilder.Entity<PotionIngredient>()
                .HasRequired(pi => pi.Potion)
                .WithMany(p => p.PotionIngredients)
                .HasForeignKey(pi => pi.PotionId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<PotionIngredient>()
                .HasRequired(pi => pi.Ingredient)
                .WithMany(i => i.PotionIngredients)
                .HasForeignKey(pi => pi.IngredientId)
                .WillCascadeOnDelete(false);

            // Your existing UserPotion configuration
            modelBuilder.Entity<UserPotion>()
                .HasKey(up => new { up.ApplicationUserId, up.PotionId });

            modelBuilder.Entity<UserPotion>()
                .HasRequired(up => up.ApplicationUser)
                .WithMany(u => u.UserPotions)
                .HasForeignKey(up => up.ApplicationUserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<UserPotion>()
                .HasRequired(up => up.Potion)
                .WithMany(p => p.UserPotions)
                .HasForeignKey(up => up.PotionId)
                .WillCascadeOnDelete(true);
        }
    }
}