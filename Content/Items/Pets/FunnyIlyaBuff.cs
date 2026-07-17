using Terraria;
using Terraria.ModLoader;
using IlyasWrath.Content.Items.Pets;
using Microsoft.Xna.Framework;

namespace IlyasWrath.Content.Items.Pets
{
	public class FunnyIlyaBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;

			if (player.whoAmI == Main.myPlayer)
			{
				if (player.ownedProjectileCounts[ModContent.ProjectileType<FunnyIlya>()] <= 0)
				{
					Projectile.NewProjectile(
						player.GetSource_Buff(buffIndex),
						player.Center,
						Vector2.Zero,
						ModContent.ProjectileType<FunnyIlya>(),
						0,
						0f,
						player.whoAmI);
				}
			}
		}
	}
}