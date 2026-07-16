using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace IlyasWrath.Content.Projectiles
{
	// This projectile showcases advanced AI code. Of particular note is a showcase on how projectiles can stick to NPCs in a manner similar to the behavior of vanilla weapons such as Bone Javelin, Daybreak, Blood Butcherer, Stardust Cell Minion, and Tentacle Spike. This code is modeled closely after Bone Javelin.
	public class BroomProjectile : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 72; 
			Projectile.height = 29; 
			Projectile.aiStyle = 0; 

		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Projectile.Center = player.Center + new Vector2(0, -70);

			Projectile.timeLeft = 1800;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 100; i++)
			{
				Dust.NewDustPerfect(
					Projectile.Center,
					DustID.GoldFlame,
					Main.rand.NextVector2Circular(6f, 6f));
			}

			SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
		}
	}
}