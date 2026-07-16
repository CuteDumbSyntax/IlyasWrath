using Terraria;
using Terraria.ModLoader;

namespace IlyasWrath.Common.Players
{
    public class RootPlayer : ModPlayer
    {
        public bool RootAccessory;

        public override void ResetEffects()
        {
            RootAccessory = false;
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (RootAccessory && !npc.boss)
                return false;

            return true;
        }
    }
}