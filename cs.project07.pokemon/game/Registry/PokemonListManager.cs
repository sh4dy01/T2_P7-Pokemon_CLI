using cs.project07.pokemon.game.entites;

namespace cs.project07.pokemon.game.Registry
{
    internal static class PokemonListManager
    {
        private static int _activePokemon;

        private static List<Pokemon> _pokemonCaptured;
        private static Pokemon[] _battleTeam;
        public static Pokemon[] BattleTeam { get => _battleTeam; }
        public static void SetBattleTeam(Pokemon[] battleTeam) { _battleTeam = battleTeam; }
        public static List<Pokemon> PokemonCaptured { get => _pokemonCaptured; }
        public static void SetPokemonCaptured(List<Pokemon> PokemonCaptured) { _pokemonCaptured = PokemonCaptured; }
        public static Pokemon ActivePokemon { get => _battleTeam[_activePokemon]; private set => _battleTeam[_activePokemon] = value; }
        public static int ActivePokemonIndex { get => _activePokemon; }


        public static void Init()
        {
            _battleTeam = new Pokemon[6];
            _pokemonCaptured = new List<Pokemon>();
            _activePokemon = -1;
        }

        public static void SetStarter(Pokemon pokemon)
        {
            pokemon.SetId();
            _battleTeam[0] = pokemon;
        }

        public static void AddPokemon(Pokemon pokemon)
        {
            pokemon.SetId();
            if (!SetPokemonInBattleTeam(pokemon))
                _pokemonCaptured.Add(pokemon);
        }

        public static void UpdatePokemon(Pokemon pokemonToUpdate)
        {
            if (_activePokemon is < 0 or > 5)
                return;

            ActivePokemon = pokemonToUpdate;
        }

        public static void SetActivePokemon(Pokemon pokemon)
        {
            _activePokemon = Array.IndexOf(_battleTeam, pokemon);
        }

        public static void EndCombat()
        {
            _activePokemon = -1;
        }

        public static int GetAverageLevel()
        {
            float avgLevel = 0;
            int pokemonInTeam = 0;

            foreach (var pokemon in BattleTeam)
            {
                if (pokemon != null)
                {
                    pokemonInTeam++;
                    avgLevel += pokemon.Level;
                }
            }
            avgLevel /= pokemonInTeam;

            return (int)avgLevel;
        }

        public static bool IsAllPokemonDead()
        {
            foreach (var pokemon in BattleTeam)
            {
                if (!pokemon.IsDead)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool SetPokemonInBattleTeam(Pokemon pokemonToAdd)
        {
            if (_battleTeam.Last() != null) return false;

            for (int i = 0; i < _battleTeam.Length; i++)
            {
                if (_battleTeam[i] == null)
                {
                    _battleTeam[i] = pokemonToAdd;
                    return true;
                }
            }

            return false;
        }

        internal static int GetPokemonInBattleCount()
        {
            int pokemonInBattle = 0;

            foreach (var pokemon in _battleTeam)
            {
                if (pokemon is not null)
                {
                    pokemonInBattle++;
                }
            }

            return pokemonInBattle;
        }

        internal static int GetNextId()
        {
            return GetPokemonInBattleCount() + _pokemonCaptured.Count + 1;
        }
    }
}
