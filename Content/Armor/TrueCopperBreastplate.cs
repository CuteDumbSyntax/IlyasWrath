using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace IlyasWrath.Content.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class TrueCopperBreastplate : ModItem
	{
		public static readonly int MaxManaIncrease = 20;
		public static readonly int MaxMinionIncrease = 1;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxManaIncrease, MaxMinionIncrease);

		public override void SetDefaults()
		{
			Item.width = 18; 
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 99); 
			Item.rare = ItemRarityID.Purple;
			Item.defense = 100; 
		}

		public override void UpdateEquip(Player player)
		{
			player.buffImmune[BuffID.OnFire] = true; // Make the player immune to Fire
			player.statManaMax2 += MaxManaIncrease; // Increase how many mana points the player can have by 20
			player.maxMinions += MaxMinionIncrease; // Increase how many minions the player can have by one
		}
	}
}