using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using IlyasWrath.Common.Players;

namespace IlyasWrath.Content.Accessories
{

	public class Root : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Purple;
			Item.accessory = true;
			Item.defense = 1;
			
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RootPlayer>().RootAccessory = true;
		}

	}
}