using TheRustyCauldron_PFG.Models; 
using System.Linq;
using System.Data.Entity; 
using System.Data.Entity.Migrations; 

namespace TheRustyCauldron_PFG.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {

            var ingredients = new Ingredient[]
            {
                // Herbs
                new Ingredient{Name="Firebell", Description="Slightly hot to the touch. Often used in fiery concoctions.", Price=32.8M, ImageUrl="/Content/img/Firebell.webp", DX=5, DY=0, Type=IngredientType.Herb}, // Towards Fire
                new Ingredient{Name="Windbloom", Description="A gentle breeze seems to follow it. Good for airy potions.", Price=38.8M, ImageUrl="/Content/img/Windbloom.webp", DX=0, DY=5, Type=IngredientType.Herb}, // Towards Levitation/Swiftness
                new Ingredient{Name="Waterbloom", Description="A common aquatic plant, often found near calm waters. Useful for cooling brews.", Price=32.8M, ImageUrl="/Content/img/Waterbloom.webp", DX=-3, DY=0, Type=IngredientType.Herb}, // Towards Frost
                new Ingredient{Name="Terraria", Description="This herb seems to hum with the energy of the earth. Stable and grounding.", Price=26.8M, ImageUrl="/Content/img/Terraria.webp", DX=0, DY=-3, Type=IngredientType.Herb}, // Towards Healing/Stench
                new Ingredient{Name="Lifeleaf", Description="Known for its vibrant green hue, it feels alive to the touch. Restorative properties.", Price=24M, ImageUrl="/Content/img/Lifeleaf.webp", DX=-2, DY=-2, Type=IngredientType.Herb}, // Towards Healing
                new Ingredient{Name="Tangleweed", Description="A surprisingly resilient and thorny plant. Can be used for binding effects.", Price=51.6M, ImageUrl="/Content/img/Tangleweed.webp", DX=0, DY=-4, Type=IngredientType.Herb}, // Towards Gluing/Slowness
                new Ingredient{Name="Goldthorn", Description="Its thorns shimmer with a golden glint. Sometimes used for luck.", Price=76.8M, ImageUrl="/Content/img/Goldthorn.webp", DX=-4, DY=0, Type=IngredientType.Herb}, // Towards Luck
                new Ingredient{Name="Goodberry", Description="A small, sweet berry known for its nourishing properties. Gentle and beneficial.", Price=36M, ImageUrl="/Content/img/Goodberry.webp", DX=-1, DY=-1, Type=IngredientType.Herb}, // Towards Healing/Rejuvenation
                new Ingredient{Name="Hairy Banana", Description="A peculiar fruit with a fuzzy skin. Sticky to the touch.", Price=62.4M, ImageUrl="/Content/img/HairyBanana.webp", DX=2, DY=-4, Type=IngredientType.Herb}, // Towards Gluing
                new Ingredient{Name="Icefruit", Description="Chilly to the touch, it sparkles like frost. Excellent for cold effects.", Price=46.8M, ImageUrl="/Content/img/Icefruit.webp", DX=3, DY=-4, Type=IngredientType.Herb}, // Towards Frost Protection/Frost
                new Ingredient{Name="Thunder Thistle", Description="Crackles faintly with static electricity. Ideal for lightning and shock.", Price=70M, ImageUrl="/Content/img/ThunderThistle.webp", DX=-4, DY=2, Type=IngredientType.Herb}, // Towards Lightning
                new Ingredient{Name="Bloodthorn", Description="Its vibrant red thorns seem to weep a crimson dew. Potent and dangerous.", Price=65.2M, ImageUrl="/Content/img/Bloodthorn.webp", DX=4, DY=-1, Type=IngredientType.Herb}, // Towards Rage/Fear
                new Ingredient{Name="Dream Beet", Description="Said to induce vivid dreams, or sometimes nightmares.", Price=73.6M, ImageUrl="/Content/img/DreamBeet.webp", DX=-2, DY=3, Type=IngredientType.Herb}, // Towards Hallucinations/Charm
                new Ingredient{Name="Druid's Rosemary", Description="An aromatic herb, favored by those who walk ancient paths. Balances natural forces.", Price=50.4M, ImageUrl="/Content/img/DruidsRosemary.webp", DX=-3, DY=1, Type=IngredientType.Herb}, // Towards Mana/Magical Vision
                new Ingredient{Name="Featherbloom", Description="Its petals are as soft and light as a feather. Light and agile.", Price=83.2M, ImageUrl="/Content/img/Featherbloom.webp", DX=1, DY=4, Type=IngredientType.Herb}, // Towards Levitation/Dexterity
                new Ingredient{Name="Lava Root", Description="Warm and slightly brittle, often found in volcanic regions. Strongly tied to fire.", Price=57.6M, ImageUrl="/Content/img/LavaRoot.webp", DX=5, DY=1, Type=IngredientType.Herb}, // Towards Fire
                new Ingredient{Name="Coldleaf", Description="Always cool to the touch, even in the warmest climates. Good for chilling effects.", Price=77.6M, ImageUrl="/Content/img/Coldleaf.webp", DX=2, DY=-5, Type=IngredientType.Herb}, // Towards Frost
                new Ingredient{Name="Flameweed", Description="Its leaves flicker as if touched by fire. Volatile and intense.", Price=70.4M, ImageUrl="/Content/img/Flameweed.webp", DX=4, DY=-2, Type=IngredientType.Herb}, // Towards Fear/Explosion
                new Ingredient{Name="Grasping Root", Description="Its tendrils seem to reach out, as if searching for something. Good for control.", Price=75.6M, ImageUrl="/Content/img/GraspingRoot.webp", DX=1, DY=-5, Type=IngredientType.Herb}, // Towards Gluing/Slowness
                new Ingredient{Name="Thornstick", Description="A gnarled branch covered in sharp thorns. Offers defense or aggression.", Price=72.8M, ImageUrl="/Content/img/Thornstick.webp", DX=-1, DY=-4, Type=IngredientType.Herb}, // Towards Stench/Slowness
                new Ingredient{Name="Whirlweed", Description="Spins gently, even in still air. Enhances movement.", Price=104M, ImageUrl="/Content/img/Whirlweed.webp", DX=2, DY=4, Type=IngredientType.Herb}, // Towards Swiftness/Dexterity
                new Ingredient{Name="Boombloom", Description="A surprisingly explosive plant if not handled carefully. Highly volatile.", Price=113.2M, ImageUrl="/Content/img/Boombloom.webp", DX=6, DY=3, Type=IngredientType.Herb}, // Towards Explosion
                new Ingredient{Name="Dragon Pepper", Description="Fiery hot, leaving a tingling sensation on the tongue. Extremely potent.", Price=112.8M, ImageUrl="/Content/img/DragonPepper.webp", DX=6, DY=1, Type=IngredientType.Herb}, // Towards Fire
                new Ingredient{Name="Fluffbloom", Description="Soft and downy, like a cloud. Light and airy.", Price=99.6M, ImageUrl="/Content/img/Fluffbloom.webp", DX=0, DY=6, Type=IngredientType.Herb}, // Towards Levitation
                new Ingredient{Name="Healer's Heather", Description="Long used in medicinal remedies. Soothing and restorative.", Price=77.2M, ImageUrl="/Content/img/HealersHeather.webp", DX=-3, DY=-3, Type=IngredientType.Herb}, // Towards Healing
                new Ingredient{Name="Spellbloom", Description="Emits a faint magical aura. Enhances magical properties.", Price=132M, ImageUrl="/Content/img/Spellbloom.webp", DX=-1, DY=5, Type=IngredientType.Herb}, // Towards Mana/Magical Vision
                new Ingredient{Name="Evergreen Fern", Description="Its fronds are always green, no matter the season. Provides endurance.", Price=86.4M, ImageUrl="/Content/img/EvergreenFern.webp", DX=0, DY=-2, Type=IngredientType.Herb}, // Towards Rejuvenation
                new Ingredient{Name="Mageberry", Description="A tart berry, often sought by spellcasters. Amplifies arcane energy.", Price=152.4M, ImageUrl="/Content/img/Mageberry.webp", DX=-2, DY=4, Type=IngredientType.Herb}, // Towards Mana
                new Ingredient{Name="Terror Bud", Description="It's said to induce fear in those who touch it. Highly unsettling.", Price=65.2M, ImageUrl="/Content/img/TerrorBud.webp", DX=3, DY=2, Type=IngredientType.Herb}, // Towards Fear

                // Mushrooms
                new Ingredient{Name="Dryad's Saddle", Description="A large, shelf-like mushroom, said to be a favored seat of dryads. Earthy and strong.", Price=33.6M, ImageUrl="/Content/img/DryadsSaddle.webp", DX=-2, DY=-2, Type=IngredientType.Mushroom}, // Towards Healing/Stench
                new Ingredient{Name="Mad Mushroom", Description="Its vibrant colors hint at unpredictable effects. Often used for chaotic brews.", Price=36.4M, ImageUrl="/Content/img/MadMushroom.webp", DX=4, DY=2, Type=IngredientType.Mushroom}, // Towards Explosion/Rage
                new Ingredient{Name="Marshroom", Description="Grows in damp, muddy areas, smelling faintly of the swamp. Sticky and slow.", Price=52.4M, ImageUrl="/Content/img/Marshroom.webp", DX=1, DY=-4, Type=IngredientType.Mushroom}, // Towards Gluing/Slowness
                new Ingredient{Name="Mudshroom", Description="Covered in a fine layer of rich, dark earth. Grounding, but can also be mucky.", Price=86.4M, ImageUrl="/Content/img/Mudshroom.webp", DX=-1, DY=-5, Type=IngredientType.Mushroom}, // Towards Stench/Slowness
                new Ingredient{Name="Stink Mushroom", Description="Emits a rather pungent odor. Disagreeable but effective.", Price=30M, ImageUrl="/Content/img/StinkMushroom.webp", DX=-5, DY=-1, Type=IngredientType.Mushroom}, // Towards Stench/Acid
                new Ingredient{Name="Sulphur Shelf", Description="Its bright yellow hue and faint odor give it away. Pungent and acidic.", Price=46.8M, ImageUrl="/Content/img/SulphurShelf.webp", DX=-4, DY=0, Type=IngredientType.Mushroom}, // Towards Acid
                new Ingredient{Name="Witch Mushroom", Description="Often used in potions for its unusual properties. Slightly arcane.", Price=42.4M, ImageUrl="/Content/img/WitchMushroom.webp", DX=-3, DY=2, Type=IngredientType.Mushroom}, // Towards Magic Vision/Mana
                new Ingredient{Name="Shadow Chanterelle", Description="Thrives in dimly lit areas, almost absorbing the light around it. Good for concealment.", Price=83.2M, ImageUrl="/Content/img/ShadowChanterelle.webp", DX=-4, DY=-3, Type=IngredientType.Mushroom}, // Towards Invisibility
                new Ingredient{Name="Weirdshroom", Description="Its cap is twisted in an unusual, almost grotesque way. Can have strange effects.", Price=46M, ImageUrl="/Content/img/Weirdshroom.webp", DX=2, DY=2, Type=IngredientType.Mushroom}, // Towards Hallucinations/Charm
                new Ingredient{Name="Foggy Parasol", Description="Its cap is often damp and misty, like a small cloud. Excellent for obscuring effects.", Price=88.8M, ImageUrl="/Content/img/FoggyParasol.webp", DX=-2, DY=4, Type=IngredientType.Mushroom}, // Towards Invisibility/Levitation
                new Ingredient{Name="Goblin Shroom", Description="A hardy mushroom, often found in caves and forgotten places. Earthy and resilient.", Price=42M, ImageUrl="/Content/img/GoblinShroom.webp", DX=1, DY=-3, Type=IngredientType.Mushroom}, // Towards Healing/Stench
                new Ingredient{Name="Moss Shroom", Description="Covered in a soft, verdant layer of moss. Grounding and natural.", Price=52.4M, ImageUrl="/Content/img/MossShroom.webp", DX=0, DY=-2, Type=IngredientType.Mushroom}, // Towards Rejuvenation
                new Ingredient{Name="Phantom Skirt", Description="Its delicate gills resemble a ghostly skirt. Ethereal and elusive.", Price=144M, ImageUrl="/Content/img/PhantomSkirt.webp", DX=-5, DY=-4, Type=IngredientType.Mushroom}, // Towards Invisibility
                new Ingredient{Name="Poopshroom", Description="Unsavory in appearance, but surprisingly useful. Potent for certain strong effects.", Price=36M, ImageUrl="/Content/img/Poopshroom.webp", DX=1, DY=-5, Type=IngredientType.Mushroom}, // Towards Stench
                new Ingredient{Name="Watercap", Description="Thrives in moist environments, often near water sources. Cooling and fluid.", Price=105.6M, ImageUrl="/Content/img/Watercap.webp", DX=-4, DY=-2, Type=IngredientType.Mushroom}, // Towards Frost/Slipperiness
                new Ingredient{Name="Kraken Mushroom", Description="Its tendrils resemble the arms of a mythical sea beast. Powerful and unpredictable.", Price=78.8M, ImageUrl="/Content/img/KrakenMushroom.webp", DX=2, DY=5, Type=IngredientType.Mushroom}, // Towards Swiftness/Dexterity
                new Ingredient{Name="Lust Mushroom", Description="Emits a subtle, enticing aroma. Known for its charm.", Price=122.4M, ImageUrl="/Content/img/LustMushroom.webp", DX=-3, DY=4, Type=IngredientType.Mushroom}, // Towards Charm/Libido
                new Ingredient{Name="Magma Morel", Description="Warm to the touch, and often found near volcanic vents. Intense heat.", Price=106.4M, ImageUrl="/Content/img/MagmaMorel.webp", DX=5, DY=0, Type=IngredientType.Mushroom}, // Towards Fire
                new Ingredient{Name="Grave Truffle", Description="A rare mushroom, often found in ancient burial grounds. Associated with necromancy.", Price=126M, ImageUrl="/Content/img/GraveTruffle.webp", DX=0, DY=4, Type=IngredientType.Mushroom}, // Towards Necromancy
                new Ingredient{Name="Rainbow Cap", Description="Its cap shimmers with all the colors of the rainbow. Unpredictable and vibrant.", Price=174.4M, ImageUrl="/Content/img/RainbowCap.webp", DX=1, DY=1, Type=IngredientType.Mushroom}, // Balanced, can lead to many effects

                // Minerals
                new Ingredient{Name="Cloud Crystal", Description="Light as air, it hums with a faint energy. Ideal for upward movement.", Price=443.2M, ImageUrl="/Content/img/CloudCrystal.webp", DX=0, DY=6, Type=IngredientType.Mineral}, // Towards Levitation
                new Ingredient{Name="Earth Pyrite", Description="A metallic ore, often mistaken for gold due to its luster. Grounding and heavy.", Price=307.2M, ImageUrl="/Content/img/EarthPyrite.webp", DX=0, DY=-5, Type=IngredientType.Mineral}, // Towards Stone Skin/Stench
                new Ingredient{Name="Frost Sapphire", Description="Gleams with a cold, blue light, and is always chilling to the touch. Strong cooling.", Price=375.2M, ImageUrl="/Content/img/FrostSapphire.webp", DX=4, DY=-4, Type=IngredientType.Mineral}, // Towards Frost Protection
                new Ingredient{Name="Fire Citrine", Description="Radiates a warm, golden glow. Powerful for fiery effects.", Price=375.2M, ImageUrl="/Content/img/FireCitrine.webp", DX=5, DY=-1, Type=IngredientType.Mineral}, // Towards Fire
                new Ingredient{Name="Blood Ruby", Description="A deep red gemstone, said to pulse faintly with life. Enhances vigor.", Price=404M, ImageUrl="/Content/img/BloodRuby.webp", DX=-3, DY=-4, Type=IngredientType.Mineral}, // Towards Healing/Rage
                new Ingredient{Name="Arcane Crystal", Description="Flickers with an inner light, suggesting powerful magical properties. Highly magical.", Price=471.2M, ImageUrl="/Content/img/ArcaneCrystal.webp", DX=-2, DY=5, Type=IngredientType.Mineral}, // Towards Mana/Magical Vision
                new Ingredient{Name="Life Crystal", Description="Emits a faint, invigorating warmth. Promotes vitality.", Price=269.2M, ImageUrl="/Content/img/LifeCrystal.webp", DX=-3, DY=-3, Type=IngredientType.Mineral}, // Towards Healing/Rejuvenation
                new Ingredient{Name="Plague Stibnite", Description="A dark, unsettling mineral, best handled with care. Associated with decay and poison.", Price=336.8M, ImageUrl="/Content/img/PlagueStibnite.webp", DX=2, DY=3, Type=IngredientType.Mineral}, // Towards Poisoning/Curse
                new Ingredient{Name="Fable Bismuth", Description="Its iridescent surface shifts with all the colors of a dream. Extremely rare and powerful.", Price=1438.4M, ImageUrl="/Content/img/FableBismuth.webp", DX=0, DY=0, Type=IngredientType.Mineral} // Neutral, or can be used for very specific high-tier effects
            };

            foreach (Ingredient i in ingredients)
            {

                context.Ingredients.AddOrUpdate(
                    p => p.Name, // This tells Entity Framework to use the 'Name' property to identify existing ingredients.
                                 // If an ingredient with the same Name already exists, it will be updated.
                                 // If not, it will be added.
                    i
                );
            }
            context.SaveChanges(); // Save changes after all ingredients are processed

        }
    }
}