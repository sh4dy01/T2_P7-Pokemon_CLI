using Newtonsoft.Json;

namespace cs.project07.pokemon.game.combat
{
    public class TypeChart
    {
        public static readonly Dictionary<Type, ConsoleColor> TypeColor = new()
        {
            {Type.NORMAL, ConsoleColor.White},
            {Type.FIRE, ConsoleColor.Red},
            {Type.WATER, ConsoleColor.Blue},
            {Type.ELECTRIC, ConsoleColor.Yellow},
            {Type.GRASS, ConsoleColor.Green},
            {Type.ICE, ConsoleColor.Cyan},
            {Type.FIGHTING, ConsoleColor.DarkRed},
            {Type.POISON, ConsoleColor.DarkMagenta},
            {Type.GROUND, ConsoleColor.DarkYellow},
            {Type.FLYING, ConsoleColor.DarkGray},
            {Type.PSYCHIC, ConsoleColor.Magenta},
            {Type.BUG, ConsoleColor.DarkGreen},
            {Type.ROCK, ConsoleColor.DarkGray},
            {Type.GHOST, ConsoleColor.DarkGray},
            {Type.DRAGON, ConsoleColor.DarkGray},
            {Type.DARK, ConsoleColor.DarkGray},
            {Type.STEEL, ConsoleColor.DarkGray},
        };
        
        private static dynamic Chart { get; set; }

        public static void Init()
        {
            using (StreamReader r = new("game/combat/type-chart.json"))
            {
                string json = r.ReadToEnd();
                Chart = JsonConvert.DeserializeObject(json);
            }
        }

        public static bool IsEffective(Type attack, Type defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()] > 1;
        }

        public static bool IsNotEffective(Type attack, Type defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()] < 1;
        }

        public static float GetDamageMultiplier(Type attack, Type defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()];
        }
    }
}
