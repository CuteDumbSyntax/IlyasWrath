using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace IlyasWrath.Content.NPCs
{

	public class IlyasServant : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 1;

		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 50;
			NPC.damage = 50;
			NPC.defense = 6;
			NPC.lifeMax = 100000;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;

			AIType = -1; 
			AnimationType = -1; 
			
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement(
					"You can't see it.")
			});
		}

		public override void AI()
		{
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];

			// Despawn if no valid target
			if (!player.active || player.dead)
			{
				NPC.velocity.Y -= 0.2f;
				NPC.timeLeft = 10;
				return;
			}

			// Fly toward the player
			Vector2 direction = player.Center - NPC.Center;
			float speed = 8f;
			float inertia = 20f;

			float distance = direction.Length();

			if (distance > speed)
				direction *= speed / distance;

			NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
		}
	}
}