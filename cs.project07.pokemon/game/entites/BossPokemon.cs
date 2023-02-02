
namespace cs.project07.pokemon.game.entites
{
    internal class BossPokemon : Pokemon
    {
        public BossPokemon(PokedexEntry dex) : base(dex, 10)
        {
            Level = 10;
            InitStat();
        }
    }
}
