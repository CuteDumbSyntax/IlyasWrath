using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using IlyasWrath.Content.Systems;

namespace IlyasWrath.Content.NPCs
{
    public class BossRushGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool buffApplied = false;

        public override void ApplyDifficultyAndPlayerScaling(
            NPC npc,
            int numPlayers,
            float balance,
            float bossAdjustment)
        {
            if (!BossRushSystem.Active)
                return;

            if (!npc.boss)
                return;

            if (buffApplied)
                return;

            buffApplied = true;

            npc.lifeMax *= 5;
            npc.life = npc.lifeMax;

            npc.damage = (int)(npc.damage * 1.2f);
            npc.defense = (int)(npc.defense * 1.15f);
        }

        public override void AI(NPC npc)
        {
            if (!BossRushSystem.Active || !npc.boss)
                return;

            npc.velocity *= 1.01f;

            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                case NPCID.BrainofCthulhu:
                case NPCID.QueenBee:
                case NPCID.Plantera:
                    npc.DiscourageDespawn(999999);
                    break;
            }
        }
    }
}