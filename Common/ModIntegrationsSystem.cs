using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace IlyasWrath.Common
{
	
	public class ModIntegrationsSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			DoBossChecklistIntegration();
		}

		private void DoBossChecklistIntegration()
		{
			
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
			{
				return;
			}
			
			if (bossChecklistMod.Version < new Version(1, 6))
			{
				return;
			}

			// The "LogBoss" method requires many parameters, defined separately below:

			// Your entry key can be used by other developers to submit mod-collaborative data to your entry. It should not be changed once defined
			string internalName = "Ilya";

			// Value inferred from boss progression, see the wiki for details
			float weight = 100f;

			// Used for tracking checklist progress
			Func<bool> downed = () => DownedBossSystem.downedIlya;

			// The NPC type of the boss
			int bossType = ModContent.NPCType<Content.NPCs.Ilya>();

			// The item used to summon the boss with (if available)
			int spawnItem = ModContent.ItemType<Content.Items.BlackFlower>();

			// "collectibles" like relic, trophy, mask, pet
			List<int> collectibles = new List<int>()
			{
				ModContent.ItemType<Content.Placeable.IlyaRelic>()
				//ModContent.ItemType<Content.Pets.MinionBossPet.MinionBossPetItem>(),
				//ModContent.ItemType<Content.Items.Placeable.Furniture.MinionBossTrophy>(),
				//ModContent.ItemType<Content.Items.Armor.Vanity.MinionBossMask>()
			};

			// By default, it draws the first frame of the boss, omit if you don't need custom drawing
			// But we want to draw the bestiary texture instead, so we create the code for that to draw centered on the intended location
			var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
				Texture2D texture = ModContent.Request<Texture2D>("IlyasWrath/Content/NPCs/Ilya_Preview").Value;
				Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
				sb.Draw(texture, centered, color);
			};

			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					["spawnItems"] = spawnItem,
					["collectibles"] = collectibles,
					["customPortrait"] = customPortrait
					// Other optional arguments as needed are inferred from the wiki
				}
			);

			// Other bosses or additional Mod.Call can be made here.
		}
	}
}