using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
	public enum Type
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
        private readonly float _maxHealth;
        private readonly Type _type;
        private readonly Attack[] _attacks;

		public int PokedexID => _pokedexID;
        public string Name => _name;
        public float MaxHealth => _maxHealth;
        public Type Type => _type;
        public Attack[] Attacks => _attacks;

        public PokedexEntry(int pokedexId, string name, Type type, float maxHealth, Attack[] attacks)
        {
			_pokedexID = pokedexId;
			_name = name;
			_maxHealth = maxHealth;
			_type = type;
			_attacks = attacks;
        }
    }
}