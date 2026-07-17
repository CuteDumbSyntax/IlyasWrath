using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace IlyasWrath.Content.Items.Pets
{
    public class FunnyIlya : ModProjectile
    {
        private int moveTimer;
        private Vector2 wanderOffset;

        private static readonly string[] Dialogue =
        {
            "Why do I meow",
            "Meow.",
            "Haha, you can't see what's behind the black square",
            "You can't catch me!",
            "Oooo, ya vsyo ponyal",
            "Wear maid suit! Please"
            
        };

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] =
                ProjectileID.Sets.SimpleLoop(0, 5, 6);
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 40;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
        }

        public override bool? CanDamage() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Keep pet alive
            if (player.dead || !player.active)
                return;

            Projectile.timeLeft = 2;

            // Animation
            Projectile.frameCounter++;

            if (Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            moveTimer--;

            if (moveTimer <= 0)
            {
                moveTimer = Main.rand.Next(30, 80);

                wanderOffset = new Vector2(
                    Main.rand.NextFloat(-200f, 200f),
                    Main.rand.NextFloat(-150f, 150f));
            }

            Vector2 target = player.Center + wanderOffset;

            Vector2 move = target - Projectile.Center;

            float speed = 12f;

            if (move.Length() > speed)
                move = Vector2.Normalize(move) * speed;

            Projectile.velocity =
                Vector2.Lerp(
                    Projectile.velocity,
                    move,
                    0.08f);

            // Face movement
            if (Projectile.velocity.X > 0.2f)
                Projectile.spriteDirection = -1;
            else if (Projectile.velocity.X < -0.2f)
                Projectile.spriteDirection = 1;

            // Teleport if too far
            if (Vector2.Distance(Projectile.Center, player.Center) > 1200f)
            {
                Projectile.Center = player.Center;
                Projectile.velocity = Vector2.Zero;
            }

            // Random talking
            if (Main.rand.NextBool(900))
            {
                SoundEngine.PlaySound(SoundID.Meowmere, Projectile.Center);

                CombatText.NewText(
                    Projectile.Hitbox,
                    new Color(180, 80, 255),
                    Dialogue[Main.rand.Next(Dialogue.Length)],
                    true);
            }

            Lighting.AddLight(
                Projectile.Center,
                0.35f,
                0.1f,
                0.45f);
        }
    }
}