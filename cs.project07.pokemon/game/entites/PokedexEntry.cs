using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
	public enum Element
	{
		NORMAL,
		FIRE,
		WATER,
		GRASS,
		ELECTRIC,
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
		protected int _pokedexID;
        protected string _name;
        protected int _maxHealth;
        protected Element _element;
        protected Attack[] _attacks;

		public int PokedexID { get => _pokedexID; }
        public string Name { get => _name; }
        public int MaxHealth { get => _maxHealth; }
        public Element Element { get => _element; }
        public Attack[] Attacks { get => _attacks; }

        public PokedexEntry(int pokedexId, string name, Element element, int maxHealth, Attack[] attacks)
        {
			_pokedexID = pokedexId;
			_name = name;
			_maxHealth = maxHealth;
			_element = element;
			_attacks = attacks;
        }
    }
}