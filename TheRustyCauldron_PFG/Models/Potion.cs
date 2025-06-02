using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// REMOVE: using System.ComponentModel.DataAnnotations.Schema; // You might not need this anymore if no other [Column] attributes are used

namespace TheRustyCauldron_PFG.Models
{
    public class Potion
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Effect { get; set; }
        public int FinalX { get; set; }
        public int FinalY { get; set; }
        public string ImageUrl { get; set; }
        public string BottleImageUrl { get; set; }

        // REMOVE THIS LINE: [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public virtual ICollection<UserPotion> UserPotions { get; set; }
        public virtual ICollection<PotionIngredient> PotionIngredients { get; set; }
    }
}