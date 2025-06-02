// Models/Order.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // For ForeignKey attribute
using TheRustyCauldron_PFG.Models; // Ensure this is present if ApplicationUser is in this namespace

namespace TheRustyCauldron_PFG.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        // Foreign key to the Potion model
        public int PotionId { get; set; }
        [ForeignKey("PotionId")]
        public virtual Potion Potion { get; set; } // Navigation property

        [Required]
        [StringLength(255)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Shipping Address")]
        public string CustomerAddress { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        [Display(Name = "Email Address")]
        public string CustomerEmail { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } // Store the price at the time of order

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        // --- UNCOMMENT THESE LINES ---
        public string ApplicationUserId { get; set; } // If you want to link orders to logged-in users
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } // Use ApplicationUser instead of User for consistency with Identity
    }
}