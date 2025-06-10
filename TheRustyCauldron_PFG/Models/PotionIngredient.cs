using System.ComponentModel.DataAnnotations.Schema;

namespace TheRustyCauldron_PFG.Models
{
    public class PotionIngredient
    {
        // Composite Primary Key
        public int PotionId { get; set; }
        public int IngredientId { get; set; }

        // Navigation Properties
        public virtual Potion Potion { get; set; }
        public virtual Ingredient Ingredient { get; set; }

    }
}