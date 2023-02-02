using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    internal class BossPokemon : Pokemon
    {
        public BossPokemon(PokedexEntry dex) : base(dex)
        {
            Level = 10;
        }

        public override void InitEnemyStats()
        {
            LevelUpStat();
        }
    }
}
