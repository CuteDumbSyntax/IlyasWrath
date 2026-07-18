using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using IlyasWrath.Content.Systems;

namespace IlyasWrath.Content.Items
{
    public class CthulhusHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.UseSound = SoundID.Roar;

            Item.rare = ItemRarityID.Purple;
            Item.value = Item.buyPrice(1);

            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            // Replace this with your own downed boss check
            // Example:
            // return DownedSystem.downedDestroyerOfStars;

            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            if (BossRushSystem.Active)
            {
                BossRushSystem.Stop();
            }
            else
            {
                BossRushSystem.Start();
            }

            return true;
        }

        //public override void AddRecipes()
        //{
        //    CreateRecipe()
        //        .AddIngredient(ItemID.LunarBar, 20)
        //        .AddIngredient(ItemID.FragmentSolar, 15)
        //        .AddIngredient(ItemID.FragmentVortex, 15)
        //        .AddIngredient(ItemID.FragmentNebula, 15)
        //        .AddIngredient(ItemID.FragmentStardust, 15)
        //        .AddTile(TileID.LunarCraftingStation)
        //        .Register();
        //}
    }
}