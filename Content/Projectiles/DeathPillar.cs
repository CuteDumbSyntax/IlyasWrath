using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace IlyasWrath.Content.Projectiles
{
    public class DeathPillar : ModProjectile
    {
        private const float BeamLength = 3500f;
        private const float BeamWidth = 48f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 5000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.hostile = true;
            Projectile.friendly = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            Lighting.AddLight(Projectile.Center, 0.2f, 0.7f, 1.3f);

            
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center + Vector2.UnitY * BeamLength,
                BeamWidth,
                ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D beam =
                TextureAssets.Projectile[Projectile.type].Value;

            Texture2D start =
                ModContent.Request<Texture2D>(
                    "IlyasWrath/Content/Projectiles/DeathPillarStart"
                ).Value;

            Vector2 emitter = Projectile.Center;

            Vector2 beamOrigin = new Vector2(0, beam.Height / 2f);

            Vector2 beamScale = new Vector2(
                BeamLength / beam.Width,
                4f);

            // Rotate the horizontal texture to vertical
            float rotation = MathHelper.PiOver2;

            // Beam glow
            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(4f, 4f);

                Main.EntitySpriteDraw(
                    beam,
                    emitter - Main.screenPosition + offset,
                    null,
                    Color.Cyan * 0.35f,
                    rotation,
                    beamOrigin,
                    beamScale,
                    SpriteEffects.None,
                    0);
            }

            // Main beam
            Main.EntitySpriteDraw(
                beam,
                emitter - Main.screenPosition,
                null,
                Color.White,
                rotation,
                beamOrigin,
                beamScale,
                SpriteEffects.None,
                0);

            // Draw glowing start texture
            Vector2 startOrigin = start.Size() / 2f;

            for (int i = 0; i < 5; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(3f, 3f);

                Main.EntitySpriteDraw(
                    start,
                    emitter - Main.screenPosition + offset,
                    null,
                    Color.Cyan * 0.4f,
                    0f,
                    startOrigin,
                    1.2f,
                    SpriteEffects.None,
                    0);
            }

            Main.EntitySpriteDraw(
                start,
                emitter - Main.screenPosition,
                null,
                Color.White,
                0f,
                startOrigin,
                1f,
                SpriteEffects.None,
                0);

            return false;
        }
    }
}