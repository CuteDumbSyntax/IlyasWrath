using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using IlyasWrath.Common.Players;

namespace IlyasWrath.Common.GlobalNPCs
{
    public class RootNPCs : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            // Ignore inactive NPCs
            if (!npc.active)
                return;

            // Ignore bosses
            if (npc.boss)
                return;

            // Ignore town NPCs and friendly NPCs
            if (npc.friendly || npc.townNPC)
                return;

            // Ignore critters
            if (npc.damage <= 0)
                return;

            Player player = Main.LocalPlayer;

            if (!player.active || player.dead)
                return;

            if (!player.GetModPlayer<RootPlayer>().RootAccessory)
                return;

            // Forget the player
            npc.target = -1;

            // Run away
            Vector2 flee = npc.Center - player.Center;

            if (flee == Vector2.Zero)
                flee = Vector2.UnitX;

            flee.Normalize();

            float fleeSpeed = 5f;

            npc.velocity += flee * 0.35f;

            if (npc.velocity.Length() > fleeSpeed)
                npc.velocity = Vector2.Normalize(npc.velocity) * fleeSpeed;
        }
    }
}