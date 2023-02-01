using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private readonly float _maxHealth;
        private readonly float _attack;
        private readonly float _spAttack;
        private readonly float _defense;
        private readonly float _spDefense;
        private readonly float _speed;

        private readonly ElementType _type;
        private readonly Attack[] _attacks;

		public int PokedexID => _pokedexID;
        public string Name => _name;
        public float MaxHealth => _maxHealth;
        public float Attack => _attack;
        public float SPAttack => _spAttack;
        public float Defense => _defense;
        public float SPDefense => _spDefense;
        public float Speed => _speed;
        public ElementType Type => _type;
        public Attack[] Attacks => _attacks;

        public PokedexEntry(int pokedexId, string name, ElementType type, float maxHealth, float attack, float defense, float spAttack, float spDefense, float speed, Attack[] attacks)
        {
			_pokedexID = pokedexId;
			_name = name;
			_maxHealth = maxHealth;
            _attack = attack;
			_spAttack = spAttack;
            _defense = defense;
            _spDefense = spDefense;
            _speed = speed;
            _type = type;
			_attacks = attacks;
        }
    }
}