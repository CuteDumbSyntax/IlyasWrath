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
	public class BloomingFlower : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 72; 
			Projectile.height = 29; 
			Projectile.aiStyle = 0; 

		}

        public override void AI()
        {
            int boss = (int)Projectile.ai[0];

            if (boss < 0 || boss >= Main.maxNPCs || !Main.npc[boss].active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Main.npc[boss].Center + new Vector2(0, -120);

            if (++Projectile.localAI[0] >= 6)
            {
                Projectile.localAI[0] = 0;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 velocity = new Vector2(
                            Main.rand.NextFloat(-8f, 8f),
                            Main.rand.NextFloat(-14f, -8f));

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromAI(),
                            Projectile.Center,
                            velocity,
                            ModContent.ProjectileType<FallingPetal>(),
                            60,
                            0f);
                    }
                }
            }
        }

    }
}