using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    static public class PokemonRegistry
    {
        static private Dictionary<int, PokedexEntry> _pokemons = new Dictionary<int, PokedexEntry>()
        {
            {1, new PokedexEntry(1, "BULBASAUR", Element.GRASS, 45, new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                })
            },
            {4, new PokedexEntry(4, "CHARMANDER", Element.FIRE, 39, new Attack[]
                {
                    AttackRegistry.EMBER,
                    AttackRegistry.SCRATCH
                })
            },
            {7, new PokedexEntry(1, "SQUIRTLE", Element.WATER, 44, new Attack[]
                {
                    AttackRegistry.WATER_GUN,
                    AttackRegistry.TACKLE
                })
            },
            {19, new PokedexEntry(19, "RATTATA", Element.NORMAL, 30, new Attack[]
                {
                    AttackRegistry.TACKLE,
                    AttackRegistry.QUICK_ATTACK
                })
            },
            {25, new PokedexEntry(25, "PIKACHU", Element.ELECTRIC, 45, new Attack[]
                {
                    AttackRegistry.THUNDER_SHOCK,
                    AttackRegistry.QUICK_ATTACK
                })
            },
            {27, new PokedexEntry(27, "SANDSHREW", Element.GROUND, 50, new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                })
            },
            {52, new PokedexEntry(52, "MEOWTH", Element.NORMAL, 40, new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                })
            },
            {63, new PokedexEntry(63, "ABRA", Element.PSYCHIC, 25, new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                })
            },
            {129, new PokedexEntry(129, "MAGIKARP", Element.WATER, 20, new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                })
            },
            {493, new PokedexEntry(439, "ARCEUS", Element.NORMAL, 120, new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                })
            },
        };

        static public PokedexEntry GetPokemonByPokedexId(int pokedexID)
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
        static public PokedexEntry GetRandomPokemon()
        {
            return _pokemons.ElementAt(new Random().Next(0, _pokemons.Count)).Value;
        }
    }
}