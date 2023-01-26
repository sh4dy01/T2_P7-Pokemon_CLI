using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    public class Pokemon
    {
        PokedexEntry _dex;

        private int _currentHealth;
        private int _level;
        private int _experience;
        private int _requiredExperience;

        public string Name { get => _dex.Name; }
        public int Level { get => _level; }
        public int Currenthealth { get => _currentHealth; }
        public int MaxHealth { get => _dex.MaxHealth; }

        public Attack[] Attacks { get => _dex.Attacks; }


        public Pokemon(PokedexEntry dex)
        {
            _dex = dex;
            _currentHealth = _dex.MaxHealth;
            _level = 1;
            _experience = 0;
        }

        public void DealDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0) _currentHealth = 0;
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;
            if (_currentHealth >= _dex.MaxHealth) _currentHealth = _dex.MaxHealth;
        }

        public void GainExperience(int experience)
        {
            _experience += experience;

            if (_experience >= _requiredExperience)
            {
                _experience = 0;
                _requiredExperience += 20;
                _level++;
            }
        }
    }
}
