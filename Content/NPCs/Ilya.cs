using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader.Utilities;
using Terraria.ModLoader;
using IlyasWrath.Content.NPCs;
using IlyasWrath.Common;
using Terraria.DataStructures;
using IlyasWrath.Content.Projectiles;
using System;
using System.Collections.Generic;
using IlyasWrath.Content.Items;
using IlyasWrath.Content.Placeable;
using Terraria.Graphics.CameraModifiers;

namespace IlyasWrath.Content.NPCs
{
    [AutoloadBossHead]
    public partial class Ilya : ModNPC
    {
        

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 10;


            

        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            AnimationType = -1;

            NPC.width = 500;
            NPC.height = 400;

            NPC.damage = 180;
            NPC.defense = 90;
            NPC.lifeMax = 4000000;

            NPC.knockBackResist = 0f;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.boss = true;

            NPC.value = Item.buyPrice(0, 50);

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = null;

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod,"Music/DestroyerOfStars");
            }

            }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement(
                    "...")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            npcLoot.Add(notExpertRule);
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CoolBox>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Placeable.IlyaRelic>()));

        }


        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter >= 2)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * 10)
                    NPC.frame.Y = 0;
            }
        }

        enum AttackState
        {
            HoverShoot,
            Dash,
            SummonAdds,
            CircleShoot,
            DeathBeam
        }

        private bool HasWitchBroom(Player player)
        {
            // Inventory
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type == ItemID.WitchBroom)
                    return true;
            }

            // Mount / misc equipment slots
            for (int i = 0; i < player.miscEquips.Length; i++)
            {
                if (player.miscEquips[i].type == ItemID.WitchBroom)
                    return true;
            }

            return false;
        }
        private void RemoveWitchBroom(Player player)
        {
            // Inventory
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type == ItemID.WitchBroom)
                {
                    player.inventory[i].TurnToAir();
                    return;
                }
            }

            // Mount / misc equipment slots
            for (int i = 0; i < player.miscEquips.Length; i++)
            {
                if (player.miscEquips[i].type == ItemID.WitchBroom)
                {
                    player.miscEquips[i].TurnToAir();
                    return;
                }
            }
        }
        private bool introPlayed = false;
        private bool introActive = false;
        private int broomProjectile = -1;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            if (!introPlayed){

        introPlayed = true;

        if (HasWitchBroom(player))
        {
        RemoveWitchBroom(player);

        introActive = true;

        NPC.damage = 0;
        NPC.velocity = Vector2.Zero;
        NPC.ai[0] = -2; // Intro state
        NPC.ai[1] = 0;

        if (Main.netMode != NetmodeID.MultiplayerClient){
        
            Projectile.NewProjectile(
                NPC.GetSource_FromAI(),
                player.Center + new Vector2(0, -60),
                Vector2.Zero,
                ModContent.ProjectileType<BroomProjectile>(),
                0,
                0f);
            }
            }
            }
        if (introActive)
        {
         NPC.velocity = Vector2.Zero;
         NPC.damage = 0;
                NPC.dontTakeDamage = true;
                NPC.ai[1]++;
                player.mount.Dismount(player);


                if (NPC.ai[1] == 1)
         {
             Main.NewText("...", 200, 80, 80);
         }

         if (NPC.ai[1] == 120)
            {
                Main.NewText("Get that broom away from my boss fight!", 200, 80, 80);
                    SoundEngine.PlaySound(
                    new SoundStyle("IlyasWrath/Content/Sounds/ilyaTalks1"),
                    NPC.Center
                    );
                    broomProjectile = 1;
                    if (broomProjectile != -1 &&
                        Main.projectile[broomProjectile].active)
                    {
                        Main.projectile[broomProjectile].Kill();
                    }
                }


                if (NPC.ai[1] >= 2000)
            {
                introActive = false;

             NPC.damage = NPC.defDamage;

             NPC.ai[0] = (int)AttackState.HoverShoot;
             NPC.ai[1] = 0;


                    NPC.dontTakeDamage = false;
                    NPC.netUpdate = true;
            }

            return;
            }
            if (fakeDeath)
            {
                NPC.velocity *= 0.95f;

                NPC.ai[1]++;

                // Your custom cutscene logic here
                if (NPC.ai[1] == 300 && Main.netMode != NetmodeID.MultiplayerClient) // 10 seconds
                {
                    Main.NewText("...", 200, 50, 50);
                }


                if (NPC.ai[1] == 1000 && Main.netMode != NetmodeID.MultiplayerClient) // 10 seconds
                {
                    SoundEngine.PlaySound(
                    new SoundStyle("IlyasWrath/Content/Sounds/IlyaV1"),
                    NPC.Center
                    );
                    Main.NewText("Take your reward", 200, 50, 50);
                }

                if (NPC.ai[1] == 1200) // 20 seconds
                {
                    Main.instance.CameraModifiers.Add(
                    new PunchCameraModifier(
                    NPC.Center,
                    Main.rand.NextVector2CircularEdge(1f, 1f),
                    15f,
                    8f,
                    150,
                    1200f
                    )
                    );
                    NPC.StrikeInstantKill();
                    DownedBossSystem.downedIlya = true;
                }

                return;
            }
            

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);

                if (!player.active || player.dead)
                {
                    NPC.velocity.Y -= 0.2f;
                    NPC.timeLeft = 10;
                    return;
                }
            }

            

            // Phase check

            int phase = 1;

            if (NPC.life < NPC.lifeMax * 0.5f)
                phase = 2;

            if (NPC.life < 100000)
                phase = 3;

            switch ((AttackState)(int)NPC.ai[0])
            {
                case AttackState.HoverShoot:
                    HoverShoot(player, phase);
                    break;

                case AttackState.Dash:
                    Dash(player, phase);
                    break;

                case AttackState.SummonAdds:
                    SummonAdds(player, phase);
                    break;

                case AttackState.CircleShoot:
                    CircleShoot(player, phase);
                    break;

                case AttackState.DeathBeam:
                    DeathBeam(player, phase);
                    break;
                
            }
            float targetRotation = NPC.velocity.X * 0.03f;

            // Clamp so it doesn't spin too far
            targetRotation = MathHelper.Clamp(targetRotation, -0.4f, 0.4f);

            // Smoothly rotate
            NPC.rotation = MathHelper.Lerp(NPC.rotation, targetRotation, 0.1f);
            NPC.ai[1]++;
        }


        private void NextAttack(int phase)
        {
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;

            if (phase == 1)
            {
                if (NPC.ai[0] == (int)AttackState.HoverShoot)
                    NPC.ai[0] = (int)AttackState.Dash;
                else
                    NPC.ai[0] = (int)AttackState.HoverShoot;
            }
            else
            {
                List<int> attacks = new()
                {
                    (int)AttackState.HoverShoot,
                    (int)AttackState.Dash,
                    (int)AttackState.SummonAdds,
                    (int)AttackState.CircleShoot,
                    (int)AttackState.DeathBeam
                };

                attacks.Remove((int)NPC.ai[0]);

                NPC.ai[0] = Main.rand.Next(attacks);
            }

            NPC.netUpdate = true;
        }

        private void HoverShoot(Player player, int phase)
        {
            float distance = 350f;

            Vector2 targetPos = player.Center;

            Vector2 dir = NPC.Center - player.Center;

            if (dir == Vector2.Zero)
                dir = Vector2.UnitY;

            dir.Normalize();

            targetPos += dir * distance;

            Move(targetPos, phase);

            int fireRate = phase == 3 ? 20 : 30;

            int warningRate = phase == 3 ? 18 : 30;

            if (NPC.ai[1] % warningRate == 0 &&
                Main.netMode != NetmodeID.MultiplayerClient)
            {

                SoundEngine.PlaySound(
                    new SoundStyle("IlyasWrath/Content/Sounds/warning"),
                    NPC.Center
                    );


                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    new Vector2(player.Center.X, player.Center.Y - 1000),
                    Vector2.Zero,
                    ModContent.ProjectileType<WarningLine>(),
                    0,
                    0f,
                    Main.myPlayer);
            }

            if (NPC.ai[1] > 300)
                NextAttack(phase);
        }

        private void Dash(Player player, int phase)
        {
            if (NPC.ai[1] == 1)
                NPC.ai[2] = 0;

            if (NPC.ai[1] % 45 == 0)
            {
                Vector2 dir = player.Center - NPC.Center;
                dir.Normalize();

                float speed = phase == 3 ? 22f : 18f;

                NPC.velocity = dir * speed;

                NPC.ai[2]++;
            }

            if (NPC.ai[1] % 45 > 20)
            {
                NPC.velocity *= 0.95f;
            }

            if (NPC.ai[2] >= 5)
                NextAttack(phase);
        }

        private int rainPortal = -1;

        private void SummonAdds(Player player, int phase)
        {
            Move(player.Center + new Vector2(0, -550), phase);

            if (NPC.ai[1] == 1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                rainPortal = Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center + new Vector2(0, -120),
                    Vector2.Zero,
                    ModContent.ProjectileType<BloomingFlower>(),
                    0,
                    0f,
                    Main.myPlayer,
                    NPC.whoAmI);
            }

            if (NPC.ai[1] > 300)
            {
                if (rainPortal != -1 &&
                    Main.projectile[rainPortal].active)
                {
                    Main.projectile[rainPortal].Kill();
                }

                NextAttack(phase);
            }
        }

        private void CircleShoot(Player player, int phase)
        {
            const int totalDashes = 4;

            int dashNumber = (int)NPC.localAI[0];
            int timer = (int)NPC.ai[1];

            // Finished?
            if (dashNumber >= totalDashes)
            {
                NPC.localAI[0] = 0;
                NextAttack(phase);
                return;
            }

            // Teleport
            if (timer == 1)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);

                Vector2 offset = angle.ToRotationVector2() * 500f;

                NPC.Center = player.Center + offset;

                NPC.velocity = Vector2.Zero;

              

                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);

                NPC.netUpdate = true;
            }

            // Small delay before dash
            if (timer < 50)
            {
                NPC.velocity *= 0.9f;
                return;
            }

            // Dash
            if (timer == 50)
            {
                Vector2 dir = player.Center - NPC.Center;

                if (dir != Vector2.Zero)
                    dir.Normalize();

                float speed = phase == 3 ? 34f : 28f;

                NPC.velocity = dir * speed;

                // SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }

            // End dash
            if (timer > 90)
            {
                NPC.ai[1] = 0;
                NPC.localAI[0]++;
            }
        }

        private void DeathBeam(Player player, int phase)
        {
            float hoverHeight = 600f;

            // Fly above player
            if (NPC.ai[1] < 80)
            {
                Vector2 target = player.Center + new Vector2(0, -hoverHeight);

                Vector2 move = target - NPC.Center;

                float speed = 22f;

                if (move.Length() > speed)
                    move = Vector2.Normalize(move) * speed;

                NPC.velocity = move;
            }

            // Charge
            else if (NPC.ai[1] < 120)
            {
                NPC.velocity *= 0.9f;

                if (NPC.ai[1] == 80)
                {
                    SoundEngine.PlaySound(SoundID.Item122, NPC.Center);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<DeathBeam>(),
                        80,
                        0f,
                        Main.myPlayer,
                        NPC.whoAmI
                        );
                    }
                }
            }

            // Fire prediction projectiles
            else if (NPC.ai[1] < 360)
            {
                NPC.velocity = Vector2.Zero;

                if (NPC.ai[1] % 25 == 0)
                {
                    Vector2 future =
                        player.Center +
                        player.velocity * 18f;

                    Vector2 dir = future - NPC.Center;

                    dir.Normalize();

                    int p = Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        dir * 12,
                        ModContent.ProjectileType<Petal>(),
                        40,
                        0f, Main.myPlayer);

                    Main.projectile[p].timeLeft = 600;
                }
            }

            if (NPC.ai[1] > 420)
                NextAttack(phase);
        }

        private void Move(Vector2 destination, int phase)
        {
            float speed = phase == 3 ? 22f : 9f;
            float inertia = 34f;

            Vector2 move = destination - NPC.Center;

            float length = move.Length();

            if (length > speed)
                move *= speed / length;

            NPC.velocity = (NPC.velocity * (inertia - 1) + move) / inertia;
        }

        private bool fakeDeath = false;

        public override bool CheckDead()
        {
            if (!fakeDeath)
            {
                fakeDeath = true;

                NPC.life = 1;
                NPC.dontTakeDamage = true;
                NPC.velocity = Vector2.Zero;

                NPC.ai[0] = -1;      // Cutscene state
                NPC.ai[1] = 0;
                NPC.damage = 0;
                NPC.netUpdate = true;

                return false; // Prevent death
            }

            return true;
        }







    }
}