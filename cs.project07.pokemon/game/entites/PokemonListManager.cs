using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    internal class PokemonListManager
    {
        private List<Pokemon> _pokemonCaptured;
        private Pokemon[] _battleTeam;

        PokemonListManager()
        {
            _battleTeam = new Pokemon[6];
            _pokemonCaptured = new List<Pokemon>();
        }

        public void AddPokemon(Pokemon pokemon)
        {
            _pokemonCaptured.Add(pokemon);
        }
    }
}
