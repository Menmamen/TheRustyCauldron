using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // For ForeignKey attribute if needed, but HasKey in fluent API is better.

namespace TheRustyCauldron_PFG.Models
{
    public class UserPotion
    {
        // Foreign Keys (no need for [Key] here as we define composite key in DbContext)
        public string ApplicationUserId { get; set; }
        public int PotionId { get; set; }

        public DateTime DiscoveryDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Potion Potion { get; set; }
    }
}