using Terraria;
using Terraria.ModLoader;

namespace IlyasWrath.Content.Systems
{
    public class BossRushScene : ModSceneEffect
    {
        public override int Music
            => MusicLoader.GetMusicSlot(Mod, "Music/BossSrush");

        public override SceneEffectPriority Priority
    => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player)
            => BossRushSystem.Active;
    }
}