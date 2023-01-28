using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.combat
{
    internal class TypeChart
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

        public static int GetDamageMultiplier(Type attack, Type defense)
        {
            return Chart[attack.ToString().ToLower()][defense.ToString().ToLower()];
        }
    }
}
