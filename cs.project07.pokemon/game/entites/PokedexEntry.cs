using cs.project07.pokemon.game.combat;

namespace cs.project07.pokemon
{
	public enum ElementType
	{
		NORMAL,
		FIRE,
		WATER,
        ELECTRIC,
		GRASS,
		ICE,
		FIGHTING,
		POISON,
		GROUND,
		FLYING,
		PSYCHIC,
		BUG,
		ROCK,
		GHOST,
		DRAGON,
		DARK,
		STEEL
	};

	public class PokedexEntry
    {
		private readonly int _pokedexID;
        private readonly string _name;
        private readonly Stat _stat;

        private readonly ElementType _element;
        private readonly Attack[] _attacks;

		public int PokedexID => _pokedexID;
        public string Name => _name;
        public Stat Stat => _stat;

        public ElementType Element => _element;
        public Attack[] Attacks => _attacks;

        public PokedexEntry(int pokedexId, string name, ElementType element, (float, float, float, float, float, float) stat, Attack[] attacks)
        {
			_pokedexID = pokedexId;
			_name = name;
            _stat = new Stat(stat);
            _element = element;
			_attacks = attacks;
        }
    }
}