using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    internal class PokemonCaptured
    {
        PokedexEntry _dex;

        private int _currentHealth;
        private int _level;
        private int _experience;

        public PokemonCaptured(PokedexEntry dex)
        {
            _dex = dex;
            _currentHealth = _dex.MaxHealth;
            _level = 1;
            _experience = 0;
        }
    }
}
