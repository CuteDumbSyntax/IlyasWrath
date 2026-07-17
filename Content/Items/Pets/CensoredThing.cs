using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using IlyasWrath.Content.Items.Pets;

namespace IlyasWrath.Content.Items.Pets
{
    public class CensoredThing : ModItem
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
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.UseSound = SoundID.Item2;

            Item.rare = ItemRarityID.Purple;
            Item.value = Item.buyPrice(0, 10);

            Item.noMelee = true;

            // Pet settings
            Item.buffType = ModContent.BuffType<FunnyIlyaBuff>();
            Item.shoot = ModContent.ProjectileType<FunnyIlya>();
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(Item.buffType, 3600);
            return true;
        }
    }
}