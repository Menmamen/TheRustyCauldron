using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheRustyCauldron_PFG.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
        [Range(-10, 10)]
        public int DX { get; set; }
        [Range(-10, 10)]
        public int DY { get; set; }
        [Required]
        public IngredientType Type { get; set; }

        [JsonIgnore]
        public virtual ICollection<PotionIngredient> PotionIngredients { get; set; }
    }
    public enum IngredientType { Herb, Mushroom, Mineral }
}