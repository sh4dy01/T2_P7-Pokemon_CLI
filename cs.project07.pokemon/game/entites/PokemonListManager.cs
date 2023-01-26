using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    internal class PokemonListManager
    {
        private readonly List<PokemonCaptured> _pokemonCaptured;

        PokemonListManager()
        {
            _pokemonCaptured= new List<PokemonCaptured>();
        }
    }
}
