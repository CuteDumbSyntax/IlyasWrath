using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace IlyasWrath.Common.Players
{
    public class MoreHealth : ModPlayer
    {

        
        public int UltraLife = 0;

        public override void ResetEffects()
        {
            Player.statLifeMax2 += UltraLife;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["UltraLife"] = UltraLife;
           
        }

        public override void LoadData(TagCompound tag)
        {
            UltraLife = tag.GetInt("UltraLife");
            
        }
    }
}