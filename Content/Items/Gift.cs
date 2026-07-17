using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using IlyasWrath.Common.Players;

namespace IlyasWrath.Content.Items
{
    public class Gift : ModItem
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
            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.UseSound = SoundID.Item4;

            Item.maxStack = 1;
            Item.consumable = true;

            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 50);
        }

        public override bool? UseItem(Player player)
        {
            MoreHealth modPlayer = player.GetModPlayer<MoreHealth>();

            if (modPlayer.UltraLife >= 1000)
                return false;

            modPlayer.UltraLife += 1000;

            player.statLife += 1000;

            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;

            player.HealEffect(1000);

            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<MoreHealth>().UltraLife < 1000;
        }
    }
}