using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    public static class PokemonRegistry
    {
        public enum StarterPokemon { Bulbasaur, Charmander, Squirtle }

        // Stats (HP, Attack, Defense, Sp Attack, Sp Defense, Speed)
        private static readonly Dictionary<int, PokedexEntry> Pokemons = new()
        {
            {1, new PokedexEntry(1, "BULBASAUR", ElementType.GRASS, (45, 49, 49, 65, 65, 45), new Attack[]
                {
                    AttackRegistry.VINE_WHIP,
                    AttackRegistry.TACKLE
                }, 45)
            },
            {4, new PokedexEntry(4, "CHARMANDER", ElementType.FIRE, (39, 52, 43, 60, 50, 65), new Attack[]
                {
                    AttackRegistry.EMBER,
                    AttackRegistry.SCRATCH
                }, 45)
            },
            {7, new PokedexEntry(1, "SQUIRTLE", ElementType.WATER, (44, 48, 65, 50, 64, 43), new Attack[]
                {
                    AttackRegistry.WATER_GUN,
                    AttackRegistry.TACKLE
                }, 45)
            },
            {19, new PokedexEntry(19, "RATTATA", ElementType.NORMAL, (30, 56, 35, 25, 35, 72), new Attack[]
                {
                    AttackRegistry.TACKLE,
                    AttackRegistry.QUICK_ATTACK
                }, 255)
            },
            {25, new PokedexEntry(25, "PIKACHU", ElementType.ELECTRIC, (35, 55, 30, 50, 40, 90), new Attack[]
                {
                    AttackRegistry.THUNDER_SHOCK,
                    AttackRegistry.QUICK_ATTACK
                }, 190)
            },
            {27, new PokedexEntry(27, "SANDSHREW", ElementType.GROUND, (50, 75, 85, 20, 30, 40), new Attack[]
                {
                    AttackRegistry.ROLLOUT,
                    AttackRegistry.SCRATCH
                }, 255)
            },
            {52, new PokedexEntry(52, "MEOWTH", ElementType.NORMAL, (40, 45, 35, 40, 40, 90), new Attack[]
                {
                    AttackRegistry.QUICK_ATTACK,
                    AttackRegistry.TACKLE
                }, 255)
            },
            {63, new PokedexEntry(63, "ABRA", ElementType.PSYCHIC, (25, 20, 15, 105, 55, 90), new Attack[]
                {
                    AttackRegistry.CONFUSION,
                }, 200)
            },
            {129, new PokedexEntry(129, "MAGIKARP", ElementType.WATER, (20, 10, 55, 15, 20, 80), new Attack[]
                {
                    AttackRegistry.SPLASH,
                }, 255)
            },
            {493, new PokedexEntry(439, "ARCEUS", ElementType.NORMAL, (120, 120, 120, 120, 120, 120), new Attack[]
                {
                    AttackRegistry.HYPER_BEAM,
                    AttackRegistry.JUDGMENT,
                    AttackRegistry.CONFUSION
                }, 3)
            },
        };

        public static PokedexEntry[] _starterPokemons = {
            GetStarterPokemon(StarterPokemon.Bulbasaur),
            GetStarterPokemon(StarterPokemon.Charmander),
            GetStarterPokemon(StarterPokemon.Squirtle),
        };


        public static PokedexEntry GetRandomStarter() => GetStarterPokemon((StarterPokemon)new Random().Next(0, 3));

        public static PokedexEntry GetStarterPokemon(StarterPokemon starter)
        {
            switch (starter)
            {
                case StarterPokemon.Bulbasaur:
                    return GetPokemonByPokedexId(1);
                case StarterPokemon.Charmander:
                    return GetPokemonByPokedexId(4);
                case StarterPokemon.Squirtle:
                    return GetPokemonByPokedexId(7);
                default:
                    throw new ArgumentException("Starter pokemon doesn't exist : " + starter);
            }
        }

        public static PokedexEntry GetPokemonByPokedexId(int pokedexID)
        {
            if(Pokemons.TryGetValue(pokedexID, out var pkmn))
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
            return Pokemons.ElementAt(new Random().Next(0, Pokemons.Count - 1)).Value;
        }
    }
}