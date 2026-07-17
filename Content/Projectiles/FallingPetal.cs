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
	public class FallingPetal : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 32; // The width of projectile hitbox
			Projectile.height = 32; // The height of projectile hitbox
			Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
			Projectile.friendly = false; // Can the projectile deal damage to enemies?
			Projectile.hostile = true; // Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Melee; // Makes the projectile deal ranged damage. You can set in to DamageClass.Throwing, but that is not used by any vanilla items
			Projectile.penetrate = -1; // How many monsters the projectile can penetrate.
			Projectile.timeLeft = 2; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false; // Can the projectile collide with tiles?
			Projectile.timeLeft = 240; // 4 seconds
		}

		public override void AI()
		{
			Projectile.rotation += 0.3f;

			// Gravity
			Projectile.velocity.Y += 0.45f;

			// Slight air resistance
			Projectile.velocity.X *= 0.995f;

			// Maximum falling speed
			if (Projectile.velocity.Y > 18f)
				Projectile.velocity.Y = 18f;
		}

	}
}