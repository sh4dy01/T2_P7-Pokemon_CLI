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

        private bool _isDead = false;
        private float _currentHealth;
        private int _level;
        private float _experience;
        private float _requiredExperience;

        public bool IsDead { get => _isDead; set => _isDead = value; }
        public string Name { get => _dex.Name; }
        public int Level { get => _level; }
        public float Currenthealth { get => _currentHealth; }
        public float MaxHealth { get => _dex.MaxHealth; }
        public Type Type { get => _dex.Type; }
        public Attack[] Attacks { get => _dex.Attacks; }


        public Pokemon(PokedexEntry dex)
        {
            _dex = dex;
            _currentHealth = _dex.MaxHealth;
            _level = 1;
            _experience = 0;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _isDead = true;
            }
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

        public Attack ChooseRandomAttack()
        {
            Random random = new Random();
            int index = random.Next(0, Attacks.Length);
            return Attacks[index];
        }
    }
}
