using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace IlyasWrath.Content.Projectiles
{
    public class WarningLine : ModProjectile
    {
        private const float LineHeight = 3000f;

        public override void SetStaticDefaults()
        {
            // Tell Terraria this projectile can draw far outside its hitbox.
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 4000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.hostile = false;
            Projectile.friendly = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.penetrate = -1;

            Projectile.timeLeft = 60;
            Projectile.hide = false;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            Lighting.AddLight(Projectile.Center, 1f, 0f, 0f);

            // Small flicker effect
            Projectile.scale = 1f + Main.rand.NextFloat(-0.05f, 0.05f);
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Main.instance.CameraModifiers.Add(
                new PunchCameraModifier(
                    Projectile.Center,
                    Main.rand.NextVector2CircularEdge(1f, 1f),
                    15f,
                    8f,
                    150,
                    1200f));

            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);

            Projectile.NewProjectile(
                Projectile.GetSource_Death(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DeathPillar>(),
                80,
                0f,
                Main.myPlayer);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(tex.Width / 2f, 0f);

            Vector2 scale = new Vector2(
                2f,                       // thickness
                LineHeight / tex.Height); // height

            // Glow
            for (int i = 0; i < 5; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(2f, 2f);

                Main.EntitySpriteDraw(
                    tex,
                    Projectile.Center - Main.screenPosition + offset,
                    null,
                    Color.Red * 0.4f,
                    0f,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0);
            }

            // Main line
            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0);

            return false;
        }
    }
}