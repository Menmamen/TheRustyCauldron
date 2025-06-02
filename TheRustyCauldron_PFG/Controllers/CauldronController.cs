using System;
using System.Collections.Generic;
using System.Data.Entity; // Keep this for EF6
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using TheRustyCauldron_PFG.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
// Removed System.Numerics as it's not used and can cause ambiguity with System.Drawing.Point or similar if not careful

namespace TheRustyCauldron_PFG.Controllers
{
    public class CauldronController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static readonly Dictionary<string, string> effectMap = new Dictionary<string, string>
        {
            { "-30,0", "Acid" },
            { "-30,10", "Acid Protection" },
            { "-30,20", "Anti-Magic" },
            { "-20,30", "Charm" },
            { "-10,30", "Curse" },
            { "0,30", "Dexterity" },
            { "10,30", "Enlargement" },
            { "20,30", "Explosion" },
            { "30,20", "Fear" },
            { "30,10", "Fire" },
            { "30,0", "Fire Protection" },
            { "30,-10", "Fragrance" },
            { "30,-20", "Frost" },
            { "20,-30", "Frost Protection" },
            { "10,-30", "Gluing" },
            { "0,-30", "Hallucinations" },
            { "-10,-30", "Healing" },
            { "-20,-30", "Inspiration" },
            { "-30,-20", "Invisibility" },
            { "-30,-10", "Levitation" },
            { "-20,-10", "Libido" },
            { "-10,-10", "Light" },
            { "-10,0", "Lightning" },
            { "-10,10", "Lightning Protection" },
            { "-20,0", "Luck" },
            { "-20,20", "Magical Vision" },
            { "-10,20", "Mana" },
            { "0,20", "Necromancy" },
            { "10,20", "Poisoning" },
            { "20,20", "Poison Protection" },
            { "20,10", "Rage" },
            { "20,0", "Rejuvenation" },
            { "20,-10", "Shrinking" },
            { "10,-10", "Sleep" },
            { "0,-10", "Slipperiness" },
            { "-10,-20", "Slowness" },
            { "-20,-20", "Stench" },
            { "-30,-30", "Stone Skin" },
            { "-20,10", "Strength" },
            { "0,10", "Swiftness" },
            { "10,0", "Wild Growth" }
        };

        [HttpPost]
        public ActionResult AddToCauldron(int ingredientId)
        {
            var ingredient = db.Ingredients.Find(ingredientId);
            if (ingredient == null)
            {
                return Json(new { success = false, message = "Ingredient not found." });
            }

            List<Ingredient> cauldron = GetCauldron();
            cauldron.Add(ingredient);
            SaveCauldron(cauldron);

            return Json(new { success = true, message = $"{ingredient.Name} added to your cauldron!" });
        }

        public ActionResult Index()
        {
            List<Ingredient> cauldron = GetCauldron();
            return View(cauldron);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BrewPotion(string SelectedBottle)
        {
            List<Ingredient> cauldron = GetCauldron();
            if (!cauldron.Any())
            {
                TempData["ErrorMessage"] = "Your cauldron is empty! Add some ingredients first.";
                return RedirectToAction("Index", "Cauldron");
            }

            // Get selected ingredient IDs from the cauldron (which stores full Ingredient objects)
            // This assumes your `Ingredient` model has an `Id` property.
            List<int> selectedIngredientIds = cauldron.Select(i => i.Id).ToList();

            // Retrieve the full ingredient details from the database to ensure you have the price.
            // This is safer than relying solely on the Session-stored Ingredient objects,
            // as prices might change or the session might not have the most up-to-date data.
            var selectedIngredientsFromDb = await db.Ingredients
                                                    .Where(i => selectedIngredientIds.Contains(i.Id))
                                                    .ToListAsync();

            if (selectedIngredientsFromDb.Count != selectedIngredientIds.Count)
            {
                TempData["ErrorMessage"] = "One or more selected ingredients could not be found in the database. Please try again.";
                ClearCauldron(); // Clear cauldron if there's a data inconsistency
                return RedirectToAction("Index", "Cauldron");
            }

            // CALCULATE THE PRICE HERE
            decimal totalPrice = selectedIngredientsFromDb.Sum(i => i.Price);


            int totalDX = cauldron.Sum(i => i.DX);
            int totalDY = cauldron.Sum(i => i.DY);

            string potionEffect = GetPotionEffect(totalDX, totalDY);
            string effectImagePath = GetPotionEffectImageUrl(potionEffect);
            string bottleImagePath = GetPotionBottleImageUrl(SelectedBottle);
            string potionName = potionEffect;

            var existingPotion = await db.Potions
                                            .FirstOrDefaultAsync(p => p.Effect == potionEffect
                                                                    && p.FinalX == totalDX
                                                                    && p.FinalY == totalDY
                                                                    && p.BottleImageUrl == bottleImagePath);

            Potion discoveredPotion;

            if (existingPotion != null)
            {
                discoveredPotion = existingPotion;
                // THIS IS THE CRUCIAL ADDITION/FIX: Update the price of the existing potion
                // This ensures that if it was created before price tracking, its price gets updated.
                if (discoveredPotion.Price != totalPrice) // Only update if it's different
                {
                    discoveredPotion.Price = totalPrice;
                    // Mark as modified if it's not automatically tracked, though for an entity retrieved by FirstOrDefaultAsync,
                    // EF typically tracks changes automatically. Explicitly setting it ensures it.
                    db.Entry(discoveredPotion).State = EntityState.Modified;
                }
            }
            else
            {
                discoveredPotion = new Potion
                {
                    Name = potionName,
                    Description = $"A potion with the effect of {potionEffect}.",
                    Effect = potionEffect,
                    FinalX = totalDX,
                    FinalY = totalDY,
                    ImageUrl = effectImagePath,
                    BottleImageUrl = bottleImagePath,
                    Price = totalPrice // ASSIGN THE CALCULATED PRICE for NEW potions
                };
                db.Potions.Add(discoveredPotion);
                await db.SaveChangesAsync(); // Save the new potion to get its Id
            }

            // Link the potion to its ingredients in the PotionIngredients table
            // This block runs whether the potion is newly discovered or existing.
            // First, remove existing links to avoid duplicates if the potion is re-brewed.
            var existingPotionIngredients = await db.PotionIngredients
                                                    .Where(pi => pi.PotionId == discoveredPotion.Id)
                                                    .ToListAsync();
            db.PotionIngredients.RemoveRange(existingPotionIngredients);
            // No need for SaveChanges here if the next SaveChanges will cover it.
            // But if you're sure you want to commit deletion immediately, keep it.
            // For robustness, I'll keep it as a separate save.
            await db.SaveChangesAsync(); // Save changes to remove existing links

            foreach (var ingredient in selectedIngredientsFromDb) // Use ingredients fetched from DB
            {
                db.PotionIngredients.Add(new PotionIngredient
                {
                    PotionId = discoveredPotion.Id,
                    IngredientId = ingredient.Id
                });
            }
            // THIS SAVE CHANGES will also persist the price update for existing potions
            // if you added 'db.Entry(discoveredPotion).State = EntityState.Modified;' above.
            await db.SaveChangesAsync(); // Save the new linking entries and any updated Potion price.

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                var userPotion = await db.UserPotions
                                            .FirstOrDefaultAsync(up => up.ApplicationUserId == userId && up.PotionId == discoveredPotion.Id);

                if (userPotion == null)
                {
                    db.UserPotions.Add(new UserPotion
                    {
                        ApplicationUserId = userId,
                        PotionId = discoveredPotion.Id,
                        DiscoveryDate = DateTime.Now
                    });
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"You discovered {discoveredPotion.Name} (Price: {discoveredPotion.Price.ToString("C")}) and it has been added to your grimoire!";
                }
                else
                {
                    // This message now reflects the potentially updated price for re-brewed potions
                    TempData["InfoMessage"] = $"You brewed {discoveredPotion.Name} again (Price: {discoveredPotion.Price.ToString("C")}). It's already in your grimoire!";
                }
            }
            else
            {
                TempData["InfoMessage"] = $"You brewed {discoveredPotion.Name} (Price: {discoveredPotion.Price.ToString("C")}). Log in to save it to your grimoire!";
            }

            ClearCauldron();
            return RedirectToAction("PotionDetails", new { id = discoveredPotion.Id });
        }

        public ActionResult ClearCauldron()
        {
            Session["Cauldron"] = null;
            TempData["InfoMessage"] = "Your cauldron has been emptied.";
            return RedirectToAction("Index", "Cauldron");
        }

        private List<Ingredient> GetCauldron()
        {
            var cauldronJson = Session["Cauldron"] as string;
            return cauldronJson == null ? new List<Ingredient>() : JsonConvert.DeserializeObject<List<Ingredient>>(cauldronJson);
        }

        private void SaveCauldron(List<Ingredient> cauldron)
        {
            Session["Cauldron"] = JsonConvert.SerializeObject(cauldron);
        }

        private string GetPotionEffect(int totalX, int totalY)
        {
            if (totalX == 0 && totalY == 0)
            {
                return "Dud Potion";
            }

            string key = $"{totalX},{totalY}";
            if (effectMap.ContainsKey(key))
            {
                return effectMap[key];
            }

            return FindClosestEffect(totalX, totalY);
        }

        private string FindClosestEffect(int totalX, int totalY)
        {
            double minDist = double.MaxValue;
            string closestEffect = "Unknown Potion";

            foreach (var entry in effectMap)
            {
                string[] coords = entry.Key.Split(',');
                int mapX = int.Parse(coords[0]);
                int mapY = int.Parse(coords[1]);

                double dist = System.Math.Sqrt(System.Math.Pow(totalX - mapX, 2) + System.Math.Pow(totalY - mapY, 2));

                if (dist < minDist)
                {
                    minDist = dist;
                    closestEffect = entry.Value;
                }
            }
            return closestEffect;
        }

        private string GetPotionBottleImageUrl(string selectedBottleName)
        {
            if (string.IsNullOrEmpty(selectedBottleName) ||
                !new string[] { "bottle1.png", "bottle2.png", "bottle3.png", "bottle4.png" }
                    .Contains(selectedBottleName, StringComparer.OrdinalIgnoreCase))
            {
                return "~/Content/img/potions/bottle1.png";
            }
            return $"~/Content/img/potions/{selectedBottleName}";
        }

        private string GetPotionEffectImageUrl(string potionEffect)
        {
            switch (potionEffect)
            {
                case "Acid": return "~/Content/img/effects/Acid.webp";
                case "Acid Protection": return "~/Content/img/effects/AcidProtection.webp";
                case "Anti-Magic": return "~/Content/img/effects/AntiMagic.webp";
                case "Charm": return "~/Content/img/effects/Charm.webp";
                case "Curse": return "~/Content/img/effects/Curse.webp";
                case "Dexterity": return "~/Content/img/effects/Dexterity.webp";
                case "Enlargement": return "~/Content/img/effects/Enlargement.webp";
                case "Explosion": return "~/Content/img/effects/Explosion.webp";
                case "Fear": return "~/Content/img/effects/Fear.webp";
                case "Fire": return "~/Content/img/effects/Fire.webp";
                case "Fire Protection": return "~/Content/img/effects/FireProtection.webp";
                case "Fragrance": return "~/Content/img/effects/Fragrance.webp";
                case "Frost": return "~/Content/img/effects/Frost.webp";
                case "Frost Protection": return "~/Content/img/effects/FrostProtection.webp";
                case "Gluing": return "~/Content/img/effects/Gluing.webp";
                case "Hallucinations": return "~/Content/img/effects/Hallucinations.webp";
                case "Healing": return "~/Content/img/effects/Healing.webp";
                case "Inspiration": return "~/Content/img/effects/Inspiration.webp";
                case "Invisibility": return "~/Content/img/effects/Invisibility.webp";
                case "Levitation": return "~/Content/img/effects/Levitation.webp";
                case "Libido": return "~/Content/img/effects/Libido.webp";
                case "Light": return "~/Content/img/effects/Light.webp";
                case "Lightning": return "~/Content/img/effects/Lightning.webp";
                case "Lightning Protection": return "~/Content/img/effects/LightningProtection.webp";
                case "Luck": return "~/Content/img/effects/Luck.webp";
                case "Magical Vision": return "~/Content/img/effects/MagicalVision.webp";
                case "Mana": return "~/Content/img/effects/Mana.webp";
                case "Necromancy": return "~/Content/img/effects/Necromancy.webp";
                case "Poisoning": return "~/Content/img/effects/Poisoning.webp";
                case "Poison Protection": return "~/Content/img/effects/PoisonProtection.webp";
                case "Rage": return "~/Content/img/effects/Rage.webp";
                case "Rejuvenation": return "~/Content/img/effects/Rejuvenation.webp";
                case "Shrinking": return "~/Content/img/effects/Shrinking.webp";
                case "Sleep": return "~/Content/img/effects/Sleep.webp";
                case "Slipperiness": return "~/Content/img/effects/Slipperiness.webp";
                case "Slowness": return "~/Content/img/effects/Slowness.webp";
                case "Stench": return "~/Content/img/effects/Stench.webp";
                case "Stone Skin": return "~/Content/img/effects/StoneSkin.webp";
                case "Strength": return "~/Content/img/effects/Strength.webp";
                case "Swiftness": return "~/Content/img/effects/Swiftness.webp";
                case "Wild Growth": return "~/Content/img/effects/WildGrowth.webp";
                case "Dud Potion": return "~/Content/img/effects/DudPotion.webp";
                default: return "~/Content/img/effects/Poisoning.webp";
            }
        }

        public async Task<ActionResult> PotionDetails(int id)
        {
            // Eagerly load PotionIngredients and Ingredients if you want to display them in PotionDetails view
            var potion = await db.Potions
                                 .Include(p => p.PotionIngredients)
                                 .FirstOrDefaultAsync(p => p.Id == id);

            if (potion == null)
            {
                return HttpNotFound();
            }
            return View(potion);
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