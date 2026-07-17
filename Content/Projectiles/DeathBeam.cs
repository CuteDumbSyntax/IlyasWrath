using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace IlyasWrath.Content.Projectiles
{
    public class DeathBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 5000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.hostile = true;
            Projectile.friendly = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.penetrate = -1;

            Projectile.timeLeft = 300;

           // Projectile.hide = true;
        }

        public override void AI()
        {
            int bossID = (int)Projectile.ai[0];

            if (bossID < 0 || bossID >= Main.maxNPCs)
            {
                Projectile.Kill();
                return;
            }

            NPC boss = Main.npc[bossID];

            if (!boss.active)
            {
                Projectile.Kill();
                return;
            }

            // Stay on boss
            Projectile.Center = boss.Center;

            // Rotate
            Projectile.rotation += 0.02f;

            // Lighting
            Lighting.AddLight(
                Projectile.Center,
                0.2f,
                0.6f,
                1.2f);

            // Dust
            if (Main.rand.NextBool(3))
            {
                Vector2 pos = Projectile.Center +
                    Projectile.rotation.ToRotationVector2() *
                    Main.rand.NextFloat(100f, 2500f);

                
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            
            Texture2D start =
                ModContent.Request<Texture2D>(
                    "IlyasWrath/Content/Projectiles/DeathBeamStart"
                ).Value;

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            float beamLength = 3500f;

            Vector2 origin = new Vector2(0, texture.Height / 2f);

            Vector2 scale = new Vector2(
                beamLength / texture.Width, // Stretch horizontally
                3f                          // Make beam 3x thicker
            );

            Vector2 startOrigin = start.Size() / 2f;

            

            // Main sprite
            Main.EntitySpriteDraw(
                start,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                startOrigin,
                1f,
                SpriteEffects.None,
                0);

            // Main beam
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                scale,
                SpriteEffects.None,
                0);

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center +
                    Projectile.rotation.ToRotationVector2() * 3500f,
                32f,
                ref point);
        }
    }
}