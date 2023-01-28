using Newtonsoft.Json;

namespace cs.project07.pokemon.game.combat
{
    public class TypeChart
    {
        private static dynamic Chart { get; set; }

        public static void Init()
        {
            using (StreamReader r = new("game/combat/type-chart.json"))
            {
                string json = r.ReadToEnd();
                Chart = JsonConvert.DeserializeObject(json);
            }
        }

        public static float GetDamageMultiplier(Type attack, Type defense)
        {
            
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()];
        }
    }
}
