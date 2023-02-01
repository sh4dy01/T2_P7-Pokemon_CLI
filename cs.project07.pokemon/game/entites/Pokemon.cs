using cs.project07.pokemon.game.combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    public class Pokemon
    {
        public const int LEVEL_UP_STEP = 20;

        PokedexEntry _dex;

        private bool _isDead = false;
        private float _currentHealth;
        private float _maxHealth;
        private int _level;
        private float _experience;
        private float _requiredExperience;

        private float? _maxEnemyHealth;

        //TODO: Create a stat object to store all stats -> pokedex not settable / pokemon yes

        public bool IsDead { get => _isDead; set => _isDead = value; }
        public string Name { get => _dex.Name; }
        public int Level { get => _level; }
        public float Currenthealth { get => _currentHealth; }
        public float Experience { get => _experience; }
        public float RequiredExp { get => _requiredExperience; }
        public float MaxHealth { get => _maxHealth; }
        public float Attack { get => _dex.Attack; }
        public float SPAttack { get => _dex.SPAttack; }
        public float Defense { get => _dex.Defense; }
        public float SPDefense { get => _dex.SPDefense; }
        public float Speed { get => _dex.Speed; }

        public ElementType Type { get => _dex.Type; }
        public Attack[] Attacks { get => _dex.Attacks; }


        public Pokemon(PokedexEntry dex)
        {
            _dex = dex;
            _currentHealth = _dex.MaxHealth;
            _level = 1;
            _experience = 0;
            _maxHealth = _dex.MaxHealth;
            _requiredExperience = LEVEL_UP_STEP;
        }

        public void InitEnemyStats(int level)
        {
            _level = level;
            _maxEnemyHealth = _dex.MaxHealth;
            for (int i = 0; i < level; i++)
            {
                _maxEnemyHealth += _maxEnemyHealth * 0.1f;
            }
        }

        public void InitHealth(int amount)
        {
            if (amount < 0) amount = 0;
            
            _currentHealth = amount;
        }

        public void TakeDamage(float damage)
        {
            if (damage < 0) return;
            
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _isDead = true;
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0) return;
            if (_currentHealth <= 0) return;

            _currentHealth += amount;
            if (_currentHealth >= _maxHealth) _currentHealth = _maxHealth;
        }

        public void HealMax()
        {
            _currentHealth = _maxHealth;
        }

        public void GainExperience(int experience)
        {
            _experience += experience;

            while (_experience >= _requiredExperience)
            {
                _level++;
                _requiredExperience += LEVEL_UP_STEP;
                _experience -= _requiredExperience;
            }
        }

        public Attack ChooseRandomAttack()
        {
            Random random = new Random();
            int index = random.Next(0, Attacks.Length);
            return Attacks[index];
        }
        
        public Attack ChooseBestAttack(ElementType defenderType)
        {
            Attack? attack = null;
            float maxDamage = 0;
            bool isAttackSuperEffective = false;

            foreach (Attack a in Attacks)
            {
                if (TypeChart.IsSuperEffective(a.ElementType, defenderType))
                {
                    if (a.Power > maxDamage)
                    {
                        isAttackSuperEffective = true;
                        attack = a;
                        maxDamage = a.Power;
                    }
                }
                else if (TypeChart.IsEffective(a.ElementType, defenderType) && !isAttackSuperEffective)
                {
                    if (a.Power > maxDamage)
                    {
                        attack = a;
                        maxDamage = a.Power;
                    }
                }
                else if (TypeChart.IsNotEffective(a.ElementType, defenderType))
                {
                    continue;
                }
            }
            
            attack ??= ChooseRandomAttack();
            
            return attack;
        }

    }
}
