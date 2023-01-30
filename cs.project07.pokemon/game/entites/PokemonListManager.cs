using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    internal class PokemonListManager
    {
        private readonly List<Pokemon> _pokemonCaptured;
        private readonly Pokemon[] _battleTeam = new Pokemon[6];

        PokemonListManager()
        {
            _pokemonCaptured = new List<Pokemon>();
        }

        public void AddPokemon(Pokemon pokemon)
        {
            _pokemonCaptured.Add(pokemon);
        }
    }
}
