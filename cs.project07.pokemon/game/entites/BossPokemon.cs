using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    internal class BossPokemon : Pokemon
    {
        public BossPokemon(PokedexEntry dex) : base(dex, 10)
        {
            Level = 10;
        }

        public override void InitEnemyStats()
        {
            LevelUpStat();
        }
    }
}
