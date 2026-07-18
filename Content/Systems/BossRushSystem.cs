using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using IlyasWrath.Content.Systems;

namespace IlyasWrath.Content.Systems
{
    public class BossRushSystem : ModSystem
    {
        public static bool Active;

        public static int CurrentBoss = 0;
        public static bool ForceCorruption;
        public static bool ForceCrimson;
        public static bool ForceJungle;
        public static bool ForceUndergroundJungle;
        private static int spawnTimer;

        public static Point CorruptionArena;
        public static Point CrimsonArena;
        public static Point JungleArena;
        public static Point TempleArena;
        public static Point UnderworldArena;


        public static Vector2 ArenaPosition;

        private static readonly List<int> BossOrder = new()
        {
            NPCID.KingSlime,
            NPCID.EyeofCthulhu,

            // Evil boss handled separately
            -1,

            NPCID.QueenBee,
            NPCID.Deerclops,
            NPCID.SkeletronHead,
            NPCID.WallofFlesh,

            NPCID.QueenSlimeBoss,

            NPCID.Retinazer, // Twins
            NPCID.TheDestroyer,
            NPCID.SkeletronPrime,

            NPCID.Plantera,
            NPCID.Golem,

            NPCID.DukeFishron,

            NPCID.HallowBoss, // Empress

            NPCID.CultistBoss,

            NPCID.MoonLordCore,

            ModContent.NPCType<Content.NPCs.Ilya>()
        };

        public static void Start()
        {
            Player player = Main.LocalPlayer;

            ArenaPosition = player.Center;

            CorruptionArena = FindCorruptionArena();
            CrimsonArena = FindCrimsonArena();
            JungleArena = FindJungleArena();
            TempleArena = FindLihzahrdTemple();
            UnderworldArena = FindUnderworldArena();

            if (Active)
                return;

            Active = true;
            CurrentBoss = 0;
            spawnTimer = 180;

            Main.NewText("Good luck", Color.MediumPurple);
        }

        public static void Stop(bool showMessage = true)
        {
            Active = false;
            CurrentBoss = 0;
            spawnTimer = 0;

            if (showMessage)
                Main.NewText("Boss Rush has ended.", Color.MediumPurple);
        }

        public override void PostUpdateWorld()
        {
            if (!Active)
                return;

            // Wait before spawning
            if (spawnTimer > 0)
            {
                spawnTimer--;

                if (spawnTimer == 0)
                    SpawnCurrentBoss();
            }

            if (NPC.LunarApocalypseIsUp)
            {
                NPC.LunarApocalypseIsUp = false;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (!npc.active)
                        continue;

                    switch (npc.type)
                    {
                        case NPCID.LunarTowerSolar:
                        case NPCID.LunarTowerVortex:
                        case NPCID.LunarTowerNebula:
                        case NPCID.LunarTowerStardust:
                            npc.active = false;
                            break;
                    }
                }
            }

            // If waiting for boss
            if (spawnTimer == 0)
            {
                if (!BossAlive())
                {
                    int previousBoss = GetCurrentBossID();

                    CurrentBoss++;

                    if (CurrentBoss >= BossOrder.Count)
                    {
                        Main.NewText("You survived.", Color.Gold);
                        Stop();
                        return;
                    }

                    Player player = Main.LocalPlayer;

                    if (BossNeedsSpecialArena(previousBoss) &&
                        !BossNeedsSpecialArena(GetCurrentBossID()))
                    {
                        player.Teleport(ArenaPosition);
                    }

                    spawnTimer = 120; // 2 seconds
                }
            }
        }

        private static bool BossAlive()
        {
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active)
                    continue;

                int id = GetCurrentBossID();

                if (npc.type == id)
                    return true;

                // Twins
                if (id == NPCID.Retinazer)
                {
                    if (npc.type == NPCID.Retinazer ||
                        npc.type == NPCID.Spazmatism)
                        return true;
                }
            }

            return false;
        }

        private static int GetCurrentBossID()
        {
            if (BossOrder[CurrentBoss] != -1)
                return BossOrder[CurrentBoss];

            return WorldGen.crimson
                ? NPCID.BrainofCthulhu
                : NPCID.EaterofWorldsHead;
        }

        private static void SpawnCurrentBoss()
        {
            Player player = Main.LocalPlayer;

            SetTimeForBoss();

            int boss = GetCurrentBossID();

            TeleportPlayer(player, boss);

            if (boss == NPCID.Retinazer)
            {
                NPC.SpawnOnPlayer(
                    player.whoAmI,
                    NPCID.Retinazer);

                NPC.SpawnOnPlayer(
                    player.whoAmI,
                    NPCID.Spazmatism);

                return;
            }

            NPC.SpawnOnPlayer(
                player.whoAmI,
                boss);
        }

        private static void SetTimeForBoss()
        {
            int boss = GetCurrentBossID();
            ForceCorruption = false;
            ForceCrimson = false;
            ForceJungle = false;
            ForceUndergroundJungle = false;

            switch (boss)
            {
                case NPCID.EaterofWorldsHead:
                    ForceCorruption = true;
                    break;

                case NPCID.BrainofCthulhu:
                    ForceCrimson = true;
                    break;

                case NPCID.QueenBee:
                    ForceJungle = true;
                    break;

                case NPCID.Plantera:
                    ForceUndergroundJungle = true;
                    break;
            }

            switch (boss)
            {
                case NPCID.EyeofCthulhu:
                case NPCID.SkeletronHead:
                case NPCID.Retinazer:
                case NPCID.TheDestroyer:
                case NPCID.SkeletronPrime:
                case NPCID.HallowBoss:

                    Main.dayTime = false;
                    Main.time = 0;

                    break;

                default:

                    Main.dayTime = true;
                    Main.time = 27000;

                    break;
            }
        }

        public override void OnWorldUnload()
        {
            Stop();
        }

        private static void TeleportPlayer(Player player, int boss)
        {
            Vector2 pos;

            switch (boss)
            {
                case NPCID.EaterofWorldsHead:
                    pos = CorruptionArena.ToWorldCoordinates(8, 16);
                    break;

                case NPCID.BrainofCthulhu:
                    pos = CrimsonArena.ToWorldCoordinates(8, 16);
                    break;

                case NPCID.QueenBee:
                case NPCID.Plantera:
                    pos = JungleArena.ToWorldCoordinates(8, 16);
                    break;

                case NPCID.Golem:
                    pos = TempleArena.ToWorldCoordinates(8, 16);
                    break;

                case NPCID.WallofFlesh:
                    pos = UnderworldArena.ToWorldCoordinates(8, 16);
                    break;

                default:
                    return;
            }

            player.Teleport(pos);

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(
                    MessageID.TeleportEntity,
                    number: player.whoAmI);
            }
        }

        private static Point FindCorruptionArena()
        {
            return FindEvilArena(TileID.Ebonstone);
        }

        private static Point FindCrimsonArena()
        {
            return FindEvilArena(TileID.Crimstone);
        }

        private static Point FindEvilArena(ushort evilStone)
        {
            int bestLeft = -1;
            int bestRight = -1;

            int currentLeft = -1;

            for (int x = 150; x < Main.maxTilesX - 150; x++)
            {
                bool found = false;

                // Search from just below the surface down to the cavern layer
                for (int y = (int)Main.worldSurface; y < Main.rockLayer; y++)
                {
                    Tile tile = Main.tile[x, y];

                    if (tile != null &&
                        tile.HasTile &&
                        tile.TileType == evilStone)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    if (currentLeft == -1)
                        currentLeft = x;
                }
                else
                {
                    if (currentLeft != -1)
                    {
                        if (bestLeft == -1 || (x - currentLeft) > (bestRight - bestLeft))
                        {
                            bestLeft = currentLeft;
                            bestRight = x;
                        }

                        currentLeft = -1;
                    }
                }
            }

            if (bestLeft == -1)
                return new Point(Main.spawnTileX, Main.spawnTileY);

            int centerX = (bestLeft + bestRight) / 2;

            return FindSurfaceAt(centerX);
        }

        private static Point FindSurfaceAt(int x)
        {
            for (int y = 50; y < Main.worldSurface; y++)
            {
                Tile tile = Main.tile[x, y];

                if (tile == null)
                    continue;

                if (!tile.HasTile)
                    continue;

                if (!Main.tileSolid[tile.TileType])
                    continue;

                bool clear = true;

                for (int i = 1; i <= 8; i++)
                {
                    Tile above = Main.tile[x, y - i];

                    if (above != null &&
                        above.HasTile &&
                        Main.tileSolid[above.TileType])
                    {
                        clear = false;
                        break;
                    }
                }

                if (clear)
                    return new Point(x, y - 3);
            }

            return new Point(Main.spawnTileX, Main.spawnTileY);
        }

        private static Point FindJungleArena()
        {
            int left = -1;
            int right = -1;

            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                bool found = false;

                for (int y = (int)Main.worldSurface - 30;
                         y < (int)Main.worldSurface + 50;
                         y++)
                {
                    Tile tile = Main.tile[x, y];

                    if (!tile.HasTile)
                        continue;

                    if (tile.TileType == TileID.JungleGrass)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    if (left == -1)
                        left = x;

                    right = x;
                }
            }

            if (left == -1)
                return new Point(Main.spawnTileX, Main.spawnTileY);

            return FindSafeGround((left + right) / 2);
        }


        private static Point FindUnderworldArena()
        {
            for (int x = 200; x < Main.maxTilesX - 200; x++)
            {
                for (int y = Main.maxTilesY - 250; y < Main.maxTilesY - 20; y++)
                {
                    Tile tile = Main.tile[x, y];

                    if (!tile.HasTile)
                        continue;

                    if (tile.TileType != TileID.ObsidianBrick)
                        continue;

                    bool clear = true;

                    for (int i = 1; i <= 6; i++)
                    {
                        Tile above = Main.tile[x, y - i];

                        if (above.HasTile &&
                            Main.tileSolid[above.TileType])
                        {
                            clear = false;
                            break;
                        }
                    }

                    if (clear)
                        return new Point(x, y - 4);
                }
            }

            return new Point(Main.maxTilesX / 2, Main.maxTilesY - 150);
        }

        private static Point FindLihzahrdTemple()
        {
            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                for (int y = (int)Main.worldSurface; y < Main.maxTilesY - 100; y++)
                {
                    Tile tile = Main.tile[x, y];

                    if (tile != null &&
                        tile.HasTile &&
                        tile.TileType == TileID.LihzahrdAltar)
                    {
                        return new Point(x, y - 8);
                    }
                }
            }

            return new Point(Main.spawnTileX, Main.spawnTileY);
        }

        private static bool BossNeedsSpecialArena(int boss)
        {
            switch (boss)
            {
                case NPCID.EaterofWorldsHead:
                case NPCID.BrainofCthulhu:
                case NPCID.QueenBee:
                case NPCID.Plantera:
                case NPCID.Golem:
                case NPCID.WallofFlesh:
                    return true;

                default:
                    return false;
            }
        }

        private static Point FindSafeGround(int x)
        {
            for (int y = (int)Main.worldSurface - 20;
                 y < (int)Main.worldSurface + 80;
                 y++)
            {
                Tile tile = Main.tile[x, y];

                if (!tile.HasTile)
                    continue;

                if (!Main.tileSolid[tile.TileType])
                    continue;

                bool clear = true;

                for (int i = 1; i <= 6; i++)
                {
                    Tile above = Main.tile[x, y - i];

                    if (above.HasTile && Main.tileSolid[above.TileType])
                    {
                        clear = false;
                        break;
                    }
                }

                if (clear)
                    return new Point(x, y - 4);
            }

            return new Point(Main.spawnTileX, Main.spawnTileY);
        }

        private static Point FindSurfaceBiome(ushort tileType)
        {
            int left = -1;
            int right = -1;

            // Find biome bounds
            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                bool found = false;

                for (int y = (int)Main.worldSurface; y < Main.maxTilesY - 300; y++)
                {
                    Tile tile = Main.tile[x, y];

                    if (tile != null &&
                        tile.HasTile &&
                        tile.TileType == tileType)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    if (left == -1)
                        left = x;

                    right = x;
                }
            }

            if (left == -1)
                return new Point(Main.spawnTileX, Main.spawnTileY);

            int centerX = (left + right) / 2;

            // Find surface
            for (int y = 50; y < Main.worldSurface; y++)
            {
                Tile tile = Main.tile[centerX, y];

                if (tile != null &&
                    tile.HasTile &&
                    Main.tileSolid[tile.TileType])
                {
                    return new Point(centerX, y - 5);
                }
            }

            return new Point(centerX, (int)Main.worldSurface - 5);
        }
        

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (!Active)
                return;

            tileColor *= 0.75f;
            backgroundColor *= 0.0f;
        }
    }
}