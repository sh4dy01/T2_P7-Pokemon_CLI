namespace cs.project07.pokemon.game.entites
{
    internal class PokemonListManager
    {
        private List<Pokemon> _pokemonCaptured;
        
        static private Pokemon[] _battleTeam;
        static public Pokemon[] BattleTeam { get => _battleTeam; }

        public PokemonListManager()
        {
            _battleTeam = new Pokemon[6];
            _pokemonCaptured = new List<Pokemon>();
            SetStarter();
        }

        private void SetStarter()
        {
            _battleTeam[0] = new Pokemon(PokemonRegistry.GetRandomStarter());
        }

        public void AddPokemon(Pokemon pokemon)
        {
            _pokemonCaptured.Add(pokemon);
            SetPokemonInBattleTeam(pokemon);
        }

        public void SetPokemonInBattleTeam(Pokemon pokemonToAdd)
        {
            if (_battleTeam.Last() != null) return;
            
            for (int i = 0; i < _battleTeam.Length; i++)
            {
                if (_battleTeam[i] == null)
                {
                    _battleTeam[i] = pokemonToAdd;
                    break;
                }
            }
        }
    }
}
