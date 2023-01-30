using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    public static class PokemonRegistry
    {
        private static readonly Dictionary<int, PokedexEntry> _pokemons = new()
        {
            {1, new PokedexEntry(1, "BULBASAUR", Type.GRASS, 45, new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                })
            },
            {4, new PokedexEntry(4, "CHARMANDER", Type.FIRE, 39, new Attack[]
                {
                    AttackRegistry.EMBER,
                    AttackRegistry.SCRATCH
                })
            },
            {7, new PokedexEntry(1, "SQUIRTLE", Type.WATER, 44, new Attack[]
                {
                    AttackRegistry.WATER_GUN,
                    AttackRegistry.TACKLE
                })
            },
            {19, new PokedexEntry(19, "RATTATA", Type.NORMAL, 30, new Attack[]
                {
                    AttackRegistry.TACKLE,
                    AttackRegistry.QUICK_ATTACK
                })
            },
            {25, new PokedexEntry(25, "PIKACHU", Type.ELECTRIC, 45, new Attack[]
                {
                    AttackRegistry.THUNDER_SHOCK,
                    AttackRegistry.QUICK_ATTACK
                })
            },
            {27, new PokedexEntry(27, "SANDSHREW", Type.GROUND, 50, new Attack[]
                {
                    AttackRegistry.ROLLOUT,
                    AttackRegistry.SCRATCH
                })
            },
            {52, new PokedexEntry(52, "MEOWTH", Type.NORMAL, 40, new Attack[]
                {
                    AttackRegistry.QUICK_ATTACK,
                    AttackRegistry.TACKLE
                })
            },
            {63, new PokedexEntry(63, "ABRA", Type.PSYCHIC, 25, new Attack[]
                {
                    AttackRegistry.CONFUSION,
                })
            },
            {129, new PokedexEntry(129, "MAGIKARP", Type.WATER, 20, new Attack[]
                {
                    AttackRegistry.SPLASH,
                })
            },
            {493, new PokedexEntry(439, "ARCEUS", Type.NORMAL, 120, new Attack[]
                {
                    AttackRegistry.HYPER_BEAM,
                    AttackRegistry.JUDGMENT
                })
            },
        };

        public static PokedexEntry GetPokemonByPokedexId(int pokedexID)
        {
            if(_pokemons.TryGetValue(pokedexID, out var pkmn))
            {
                return pkmn;
            }
            else
            {
                throw new ArgumentException("Pokemon doesn't exist : " + pokedexID);
            }
        }
        public static PokedexEntry GetRandomPokemon()
        {
            return _pokemons.ElementAt(new Random().Next(0, _pokemons.Count)).Value;
        }
    }
}