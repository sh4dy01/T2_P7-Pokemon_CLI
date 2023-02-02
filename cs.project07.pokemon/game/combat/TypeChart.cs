using Newtonsoft.Json;

namespace cs.project07.pokemon.game.combat
{
    public static class TypeChart
    {
        public static readonly Dictionary<ElementType, ConsoleColor> TypeColor = new()
        {
            {ElementType.NORMAL, ConsoleColor.White},
            {ElementType.FIRE, ConsoleColor.Red},
            {ElementType.WATER, ConsoleColor.Blue},
            {ElementType.ELECTRIC, ConsoleColor.Yellow},
            {ElementType.GRASS, ConsoleColor.Green},
            {ElementType.ICE, ConsoleColor.Cyan},
            {ElementType.FIGHTING, ConsoleColor.DarkRed},
            {ElementType.POISON, ConsoleColor.DarkMagenta},
            {ElementType.GROUND, ConsoleColor.DarkYellow},
            {ElementType.FLYING, ConsoleColor.DarkGray},
            {ElementType.PSYCHIC, ConsoleColor.Magenta},
            {ElementType.BUG, ConsoleColor.DarkGreen},
            {ElementType.ROCK, ConsoleColor.DarkGray},
            {ElementType.GHOST, ConsoleColor.DarkGray},
            {ElementType.DRAGON, ConsoleColor.DarkGray},
            {ElementType.DARK, ConsoleColor.DarkGray},
            {ElementType.STEEL, ConsoleColor.DarkGray},
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

        public static bool IsSuperEffective(ElementType attack, ElementType defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()] > 1;
        }

        public static bool IsEffective(ElementType attack, ElementType defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()] == 1;
        }

        public static bool IsNotEffective(ElementType attack, ElementType defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()] < 1;
        }

        public static float GetDamageMultiplier(ElementType attack, ElementType defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()];
        }
    }
}
