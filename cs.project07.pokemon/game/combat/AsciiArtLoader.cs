using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.combat
{
    internal static class AsciiArtLoader
    {
        public static string[] GetPlayerSpriteByName(string name)
        {
            string[] lines = File.ReadAllLines(@"game\assets\ascii\" + name.ToLower() + "_back.ans");
            return lines;
        }

        public static string[] GetEnemySpriteByName(string name)
        {
            string[] lines = System.IO.File.ReadAllLines(@"game\assets\ascii\" + name.ToLower() + "_front.ans");
            return lines;
        }
    }
}
