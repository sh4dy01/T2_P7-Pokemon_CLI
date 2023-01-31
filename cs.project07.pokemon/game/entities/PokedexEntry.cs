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
		private int _pokedexID;
		private string _name;
		private int _maxHealth;
		private Element _element;
		private Attack[] _attacks;

		public string Name { get => _name; }

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