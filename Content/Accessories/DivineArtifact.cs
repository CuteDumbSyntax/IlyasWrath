using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace IlyasWrath.Content.Accessories
{

	public class DivineArtifact : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Purple;
			Item.accessory = true;

			Item.defense = 15;
			Item.lifeRegen = 5;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 10f; // Increase ALL player damage by 100%
			player.GetDamage(DamageClass.Magic) += 6f;
			player.GetDamage(DamageClass.Summon) += 8f;
			player.GetDamage(DamageClass.Ranged) += 9f;

		}

	}
}