using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace IlyasWrath.Content.Systems
{
    public class BossRushPlayer : ModPlayer
    {
        public override bool PreKill(
            double damage,
            int hitDirection,
            bool pvp,
            ref bool playSound,
            ref bool genGore,
            ref PlayerDeathReason damageSource)
        {
            if (BossRushSystem.Active)
                BossRushSystem.Stop(false);

            return true;
        }

        public override void OnEnterWorld()
        {
            BossRushSystem.Stop(false);
        }
    }
}